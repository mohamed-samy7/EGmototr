#pip install cryptography pycryptodome 

from cryptography.hazmat.primitives import hashes
from cryptography.hazmat.primitives.asymmetric import rsa, padding
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.backends import default_backend
import os
import base64

# ================== RSA (Asymmetric Encryption) ==================
def generate_rsa_keys():
    """Generate RSA public and private keys (2048-bit)."""
    private_key = rsa.generate_private_key(
        public_exponent=65537,
        key_size=2048,
        backend=default_backend()
    )
    public_key = private_key.public_key()
    return private_key, public_key

def rsa_encrypt(public_key, plaintext):
    """Encrypt data using RSA public key."""
    ciphertext = public_key.encrypt(
        plaintext.encode(),
        padding.OAEP(
            mgf=padding.MGF1(algorithm=hashes.SHA256()),
            algorithm=hashes.SHA256(),
            label=None
        )
    )
    return base64.b64encode(ciphertext).decode()

def rsa_decrypt(private_key, ciphertext):
    """Decrypt data using RSA private key."""
    ciphertext_bytes = base64.b64decode(ciphertext.encode())
    plaintext = private_key.decrypt(
        ciphertext_bytes,
        padding.OAEP(
            mgf=padding.MGF1(algorithm=hashes.SHA256()),
            algorithm=hashes.SHA256(),
            label=None
        )
    )
    return plaintext.decode()

# ================== AES (Symmetric Encryption) ==================
def generate_aes_key(password: str, salt: bytes = None):
    """Derive a 256-bit AES key from a password using PBKDF2."""
    if salt is None:
        salt = os.urandom(16)  # Random 16-byte salt
    kdf = PBKDF2HMAC(
        algorithm=hashes.SHA256(),
        length=32,  # 256-bit key
        salt=salt,
        iterations=100000,
        backend=default_backend()
    )
    key = kdf.derive(password.encode())
    return key, salt

def aes_encrypt(key: bytes, plaintext: str):
    """Encrypt data using AES-256-CBC."""
    iv = os.urandom(16)  # Random 16-byte IV
    cipher = Cipher(
        algorithms.AES(key),
        modes.CBC(iv),
        backend=default_backend()
    )
    encryptor = cipher.encryptor()
    # Pad plaintext to be a multiple of 16 bytes (AES block size)
    padded_plaintext = plaintext.encode() + b"\0" * (16 - len(plaintext) % 16)
    ciphertext = encryptor.update(padded_plaintext) + encryptor.finalize()
    return base64.b64encode(iv + ciphertext).decode()

def aes_decrypt(key: bytes, ciphertext: str):
    """Decrypt data using AES-256-CBC."""
    data = base64.b64decode(ciphertext.encode())
    iv = data[:16]  # Extract IV (first 16 bytes)
    ciphertext_bytes = data[16:]
    cipher = Cipher(
        algorithms.AES(key),
        modes.CBC(iv),
        backend=default_backend()
    )
    decryptor = cipher.decryptor()
    plaintext_padded = decryptor.update(ciphertext_bytes) + decryptor.finalize()
    plaintext = plaintext_padded.rstrip(b"\0").decode()  # Remove padding
    return plaintext

# ================== Example Usage ==================
if __name__ == "__main__":
    print("=== RSA Encryption/Decryption ===")
    private_key, public_key = generate_rsa_keys()
    message = "Hello, RSA!"
    encrypted_rsa = rsa_encrypt(public_key, message)
    decrypted_rsa = rsa_decrypt(private_key, encrypted_rsa)
    print(f"Original: {message}")
    print(f"Encrypted (RSA): {encrypted_rsa}")
    print(f"Decrypted (RSA): {decrypted_rsa}\n")

    print("=== AES Encryption/Decryption ===")
    password = "my_secure_password"
    aes_key, salt = generate_aes_key(password)
    message = "Hello, AES!"
    encrypted_aes = aes_encrypt(aes_key, message)
    decrypted_aes = aes_decrypt(aes_key, encrypted_aes)
    print(f"Original: {message}")
    print(f"Encrypted (AES): {encrypted_aes}")
    print(f"Decrypted (AES): {decrypted_aes}")