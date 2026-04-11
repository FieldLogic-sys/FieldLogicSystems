import os
import chromadb
from langchain_google_genai import ChatGoogleGenerativeAI
from langchain_core.prompts import ChatPromptTemplate
from chromadb.utils import embedding_functions

# 1. SET THE POWER SOURCE
# No lecture here, Tony! Just plug it in.
os.environ["GOOGLE_API_KEY"] = "AIzaSyAz4WVZfR-ZtzrMkCTCVvK22srn_Kveqn4"

def ask_expert(question):
    # 2. CONNECT TO YOUR LOCAL DATABASE
    client = chromadb.PersistentClient(path="./vector_db")
    ef = embedding_functions.DefaultEmbeddingFunction()
    collection = client.get_collection(name="field_knowledge", embedding_function=ef)

    # 3. PULL THE DATA (Query Expansion included)
    processed_query = f"Error Code {question}" if question.isdigit() else question
    results = collection.query(query_texts=[processed_query], n_results=3)

    # Check if we actually found anything decent
    if results['distances'][0][0] > 1.2:
        context_text = "No direct match found in manuals."
    else:
        context_text = "\n\n".join(results['documents'][0])

    # 4. DEFINE THE 'SENIOR ENGINEER' LOGIC
    # Update this section in brain.py
    llm = ChatGoogleGenerativeAI(
        model="gemini-1.5-flash",
        google_api_key="AIzaSyAz4WVZfR-ZtzrMkCTCVvK22srn_Kveqn4", # Using your provided key
        temperature=0.2
    )
    prompt = ChatPromptTemplate.from_template("""
    SYSTEM: You are an expert Gilbarco Field Engineer. 
    INSTRUCTIONS: Use the provided MDE snippets to answer the tech's question. 
    Format the output for a mobile screen (bullet points, clear headings).
    If the data has weird spaces or gaps, clean them up.
    
    MANUAL CONTEXT:
    {context}
    
    TECH QUESTION: {question}
    
    EXPERT ANSWER:
    """)

    # 5. EXECUTE THE REASONING
    chain = prompt | llm
    response = chain.invoke({"context": context_text, "question": question})

    print(f"\n{'='*40}")
    print(f" FIELD INTELLIGENCE REPORT: {processed_query}")
    print(f"{'='*40}")
    print(response.content)

if __name__ == "__main__":
    while True:
        query = input("\n[?] Enter Error Code or Part Name (or 'exit'): ")
        if query.lower() == 'exit': break
        ask_expert(query)