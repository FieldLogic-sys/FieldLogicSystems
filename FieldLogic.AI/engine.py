import os
import chromadb
# NEW: Importing the 'Governor' settings for the engine
from docling.datamodel.base_models import InputFormat
from docling.datamodel.pipeline_options import PdfPipelineOptions
from docling.document_converter import DocumentConverter, PdfFormatOption
from chromadb.utils import embedding_functions

"""Configurations for the brain of the library"""
DB_Path = "./vector_db"
MDE_FOLDER = "./mde_files"

def initialize_brain():
    # 1. Setting up the librarian 
    client = chromadb.PersistentClient(path=DB_Path)
    ef = embedding_functions.DefaultEmbeddingFunction()
    collection = client.get_or_create_collection(name="field_knowledge", embedding_function=ef)

    # 2. Verify if the librarian has read the manuals
    if collection.count() == 0:
        print("[*] Memory is empty. Throttling engine for stability...")

        # --- THE FIX: DISABLING OCR TO SAVE RAM ---
        pipeline_options = PdfPipelineOptions()
        pipeline_options.do_ocr = False  # Stops the 'bad_alloc' error

        converter = DocumentConverter(
            format_options={
                InputFormat.PDF: PdfFormatOption(pipeline_options=pipeline_options)
            }
        )
        # ------------------------------------------

        for filename in os.listdir(MDE_FOLDER):
            if filename.endswith(".pdf"):
                print(f"[*] Ingesting: {filename}")
                file_path = os.path.join(MDE_FOLDER, filename)

                # The heavy lifting
                result = converter.convert(file_path)
                content = result.document.export_to_markdown()

                # Contextual Chunking Logic
                sections = content.split("##")
                current_error_context = "General Information"

                for i, section in enumerate(sections):
                    # If this section contains the string 'Error Code', 
                    # update the context so subsequent chunks are labeled correctly.
                    if "Error Code" in section:
                        # Grab the first line of the section (the header)
                        current_error_context = section.split('\n')[0].strip()

                    # Glue the Header Context to the actual content
                    contextual_chunk = f"Manual: {filename}\nSection: {current_error_context}\n\n{section}"

                    collection.add(
                        documents=[contextual_chunk],
                        metadatas=[{"source": filename, "error_code": current_error_context}],
                        ids=[f"{filename}_{i}"]
                    )
        print(f"[+] Ingesting complete. segments: {collection.count()}")
    else:
        print(f"[!] Brain active with {collection.count()} knowledge segments.")

    return collection

if __name__ == "__main__":
    if not os.path.exists(MDE_FOLDER):
        os.makedirs(MDE_FOLDER)
        print(f"[!] Place your MDE PDFs in {MDE_FOLDER} and run again.")
    else:
        initialize_brain()