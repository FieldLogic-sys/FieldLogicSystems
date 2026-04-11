import chromadb
from chromadb.utils import embedding_functions

DB_Path = "./vector_db"

def ask_the_brain(question):
    client = chromadb.PersistentClient(path=DB_Path)
    ef = embedding_functions.DefaultEmbeddingFunction()
    collection = client.get_collection(name="field_knowledge", embedding_function=ef)

    # QUERY EXPANSION: If Tony types just a number, make it a high-fidelity search
    if question.isdigit():
        processed_query = f"Error Code {question}"
    else:
        processed_query = question

    print(f"[*] Searching for: {processed_query}")
    results = collection.query(query_texts=[processed_query], n_results=1)

    distance = results['distances'][0][0]

    # 1.10 is the 'Sweet Spot' for this manual
    if distance > 1.10:
        print(f"\n[!] NO DIRECT MATCH (Confidence: {distance:.2f})")
        print("This may be in a different MDE or requires an LLM for interpretation.")
    else:
        print(f"\n--- Verified Field Logic (Confidence: {distance:.2f}) ---")
        print(results['documents'][0][0])

if __name__ == "__main__":
    # Test your IS-HUB here now!
    ask_the_brain("24")