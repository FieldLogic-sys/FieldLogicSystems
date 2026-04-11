import chromadb
from chromadb.utils import embedding_functions

# --- CONFIG ---
DB_Path = "./vector_db"

def ask_the_brain(question):
    # 1. Connect to the existing library
    client = chromadb.PersistentClient(path=DB_Path)
    ef = embedding_functions.DefaultEmbeddingFunction()
    collection = client.get_collection(name="field_knowledge", embedding_function=ef)

    # 2. Search for the top 2 closest matches
    print(f"[*] Searching for: {question}")
    results = collection.query(
        query_texts=[question],
        n_results=2
    )

    # 3. Print the results
    for i, doc in enumerate(results['documents'][0]):
        source = results['metadatas'][0][i]['source']
        print(f"\n--- Result {i+1} (Source: {source}) ---")
        print(doc)

if __name__ == "__main__":
    # TEST: Let's see if it finds the 5150 steps
    ask_the_brain("What are the troubleshooting steps for error 5198?")