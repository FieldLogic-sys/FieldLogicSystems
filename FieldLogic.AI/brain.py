import os
import time
import chromadb
from langchain_google_genai import ChatGoogleGenerativeAI
from langchain_core.prompts import ChatPromptTemplate
from chromadb.utils import embedding_functions

# 1. THE POWER SOURCE
os.environ["GOOGLE_API_KEY"] = its_a_secret

def ask_expert(question):
    start_time = time.time()
    try:
        # Connect to the Librarian
        client = chromadb.PersistentClient(path="./vector_db")
        ef = embedding_functions.DefaultEmbeddingFunction()
        collection = client.get_collection(name="field_knowledge", embedding_function=ef)

        # 2. DETERMINISTIC RETRIEVAL (The 1:10 Global Elite Fix)
        if question.isdigit():
            target_label = f"Error Code {question}"
            # Hard-filter: Only look for the exact tag
            results = collection.query(
                query_texts=[target_label],
                n_results=1,
                where={"code": target_label}
            )
        else:
            results = collection.query(query_texts=[question], n_results=3)

        # Verify Integrity
        if not results['documents'] or not results['documents'][0]:
            context_text = f"CRITICAL: {question} is not in the MDE-2584 (Encore/Eclipse) registry."
        else:
            context_text = results['documents'][0][0]

        # 3. REASONING ENGINE
        llm = ChatGoogleGenerativeAI(model="gemini-3-flash-preview", temperature=0.1)

        prompt = ChatPromptTemplate.from_template("""
        You are a Senior Gilbarco Field Engineer. 
        If the context indicates a code is NOT found, explain that it may be a legacy Advantage/Two-Wire error.
        Otherwise, provide a professional, clean troubleshooting report for the tech's Samsung S26 Ultra.
        
        CONTEXT: {context}
        QUESTION: {question}
        
        REPORT:
        """)

        chain = prompt | llm
        response = chain.invoke({"context": context_text, "question": question})

        # Clean response format
        content = response.content
        if isinstance(content, list):
            content = content[0].get('text', str(content))

        print(f"\n{'='*45}\n LOGIC RETRIEVED IN: {time.time() - start_time:.2f}s\n{'='*45}")
        print(content)

    except Exception as e:
        print(f"\n[!] SYSTEM ERROR: {e}")

# THIS IS THE PART THAT ENSURES THE PROMPT STICKS
if __name__ == "__main__":
    # OS Check to clear terminal noise
    os.system('cls' if os.name == 'nt' else 'clear')
    print("[*] FieldLogic.AI Brain Active...")
    print("[*] Target Hardware: Gilbarco Sandpiper Electronics")

    while True:
        try:
            query = input("\n[?] Enter Error Code or Component (or 'exit'): ").strip()
            if query.lower() == 'exit':
                print("[*] Shutting down.")
                break
            if query:
                ask_expert(query)
        except KeyboardInterrupt:
            break