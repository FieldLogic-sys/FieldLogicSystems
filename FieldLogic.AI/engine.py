import os, re, chromadb
from pypdf import PdfReader
from chromadb.utils import embedding_functions

def rebuild_high_density_db():
    # 1. Setup Chroma with Persistence
    client = chromadb.PersistentClient(path="./vector_db")

    # 2. Use the default embedding function (the Librarian's brain)
    ef = embedding_functions.DefaultEmbeddingFunction()
    collection = client.create_collection(name="field_knowledge", embedding_function=ef)

    # 3. Get your clean list of 67 files
    files = [f for f in os.listdir("./mde_files") if f.endswith(".pdf")]

    print(f"[*] Starting High-Density Ingestion...")
    print(f"[*] Total Files to Process: {len(files)}")

    for idx, filename in enumerate(files, 1):
        print(f"[*] [{idx}/{len(files)}] Processing: {filename}")
        try:
            reader = PdfReader(os.path.join("./mde_files", filename))
            text = ""
            for page in reader.pages:
                page_text = page.extract_text() or ""
                # Minimal cleaning to keep technical tables intact
                text += page_text + "\n"

            # 4. WINDOWED CHUNKING (The "Broad-Spectrum" Fix)
            # 1500 chars is roughly 250 words; 200 char overlap keeps context
            chunk_size = 1500
            overlap = 200
            chunks = [text[i:i + chunk_size] for i in range(0, len(text), chunk_size - overlap)]

            for i, chunk in enumerate(chunks):
                # We still try to tag error codes so your 'Hard-Lock' filter works
                code_match = re.search(r'Error Code (\d+)', chunk)
                code_label = f"Error Code {code_match.group(1)}" if code_match else "General Logic"

                collection.add(
                    documents=[f"SOURCE: {filename}\n\n{chunk}"],
                    metadatas=[{"source": filename, "code": code_label, "type": "manual_entry"}],
                    ids=[f"{filename}_seg_{i}"]
                )
        except Exception as e:
            print(f"[!] Error on {filename}: {e}")

    print(f"\n[+] MISSION COMPLETE.")
    print(f"[+] Final Segment Count: {collection.count()}")

if __name__ == "__main__":
    rebuild_high_density_db()