import os
import re
import chromadb
from pypdf import PdfReader
from chromadb.utils import embedding_functions

DB_Path = "./vector_db"
MDE_FOLDER = "./mde_files"

def clean_text(text):
    # Removes distracting headers/footers
    text = re.sub(r"Initial Release: \d{2}/\d{2}/\d{4}.*Page \d+ of \d+", "", text)
    return text.strip()

def initialize_brain():
    client = chromadb.PersistentClient(path=DB_Path)
    ef = embedding_functions.DefaultEmbeddingFunction()
    collection = client.get_or_create_collection(name="field_knowledge", embedding_function=ef)

    if collection.count() == 0:
        print("[*] Memory empty. Starting Hybrid Extraction...")

        for filename in os.listdir(MDE_FOLDER):
            if filename.endswith(".pdf"):
                print(f"[*] Processing: {filename}")
                reader = PdfReader(os.path.join(MDE_FOLDER, filename))

                full_manual_text = ""
                # 1. PAGE-BY-PAGE INDEXING (For parts like IS-HUB)
                for i, page in enumerate(reader.pages):
                    p_text = clean_text(page.extract_text())
                    full_manual_text += p_text + "\n"

                    collection.add(
                        documents=[f"General Info from {filename} Page {i+1}:\n{p_text}"],
                        metadatas=[{"source": filename, "type": "general", "page": i+1}],
                        ids=[f"{filename}_pg_{i}"]
                    )

                # 2. ERROR CODE INDEXING (For precision fixes)
                sections = re.split(r'(?=Error Code \d+)', full_manual_text)
                for i, section in enumerate(sections):
                    if "Error Code" not in section: continue

                    header = section.split('\n')[0].strip()
                    collection.add(
                        documents=[f"SPECIFIC FIX FOR {header}:\n\n{section}"],
                        metadatas=[{"source": filename, "type": "error_fix", "code": header}],
                        ids=[f"{filename}_fix_{i}"]
                    )
        print(f"[+] Hybrid Indexing Complete. Segments: {collection.count()}")
    else:
        print(f"[!] Brain active with {collection.count()} segments.")

if __name__ == "__main__":
    initialize_brain()