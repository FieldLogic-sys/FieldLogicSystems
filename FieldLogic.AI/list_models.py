import google.generativeai as genai
import os

# Set your key
os.environ["GOOGLE_API_KEY"] = "AIzaSyAz4WVZfR-ZtzrMkCTCVvK22srn_Kveqn4"
genai.configure(api_key=os.environ["GOOGLE_API_KEY"])

print("[*] Querying Google for available models...")
try:
    for m in genai.list_models():
        if 'generateContent' in m.supported_generation_methods:
            print(f"-> {m.name}")
except Exception as e:
    print(f"[!] Error: {e}")