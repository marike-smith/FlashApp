namespace FlashApp.Application.Abstractions.Cryptography
{
    public interface IEncryptionService
    {
        string Encrypt(string plaintext);
        string Decrypt(string encryptedText);
    }
}
