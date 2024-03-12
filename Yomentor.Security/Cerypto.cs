using System.Security.Cryptography;
using System.Text;

namespace RLV.Security.Lib
{
    public abstract class OperationResult
    {
        public bool Success { get; set; }
        public string ExceptionMessage { get; set; }
    }

    public class AsymmetricKeyPairGenerationResult : OperationResult
    {
        public string PublicKeyXml { get; set; }
        public string PublicPrivateKeyPairXml { get; set; }
    }

    public class AsymmetricEncryptionService
    {
        public AsymmetricKeyPairGenerationResult GenerateKeysAsXml(int keySizeBits)
        {
            AsymmetricKeyPairGenerationResult asymmetricKeyPairGenerationResult = new AsymmetricKeyPairGenerationResult();
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(keySizeBits);
            try
            {
                asymmetricKeyPairGenerationResult.PublicKeyXml = rsaProvider.ToXmlString(false);
                asymmetricKeyPairGenerationResult.PublicPrivateKeyPairXml = rsaProvider.ToXmlString(true);
                asymmetricKeyPairGenerationResult.Success = true;
            }
            catch (CryptographicException cex)
            {
                string NL = Environment.NewLine;
                StringBuilder validKeySizeBuilder = new StringBuilder();
                KeySizes[] validKeySizes = rsaProvider.LegalKeySizes;
                foreach (KeySizes keySizes in validKeySizes)
                {
                    validKeySizeBuilder.Append("Min: ")
                        .Append(keySizes.MinSize).Append(NL)
                        .Append("Max: ").Append(keySizes.MaxSize).Append(NL)
                        .Append("Step: ").Append(keySizes.SkipSize);
                }
                asymmetricKeyPairGenerationResult.ExceptionMessage =
                    $"Cryptographic exception when generating a key-pair of size {keySizeBits}. Exception: {cex.Message}{NL}Make sure you provide a valid key size. Here are the valid key size boundaries:{NL}{validKeySizeBuilder.ToString()}";
            }
            catch (Exception otherEx)
            {
                asymmetricKeyPairGenerationResult.ExceptionMessage =
                    $"Other exception caught while generating the key pair: {otherEx.Message}";
            }
            return asymmetricKeyPairGenerationResult;
        }

        public AsymmetricEncryptionResult EncryptWithPublicKeyXml(string message, string publicKeyAsXml)
        {
            AsymmetricEncryptionResult asymmetricEncryptionResult = new AsymmetricEncryptionResult();
            try
            {
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
                rsaProvider.FromXmlString(publicKeyAsXml);
                byte[] encryptedAsBytes = rsaProvider.Encrypt(Encoding.UTF8.GetBytes(message), true);
                string encryptedAsBase64 = Convert.ToBase64String(encryptedAsBytes);
                asymmetricEncryptionResult.EncryptedAsBase64 = encryptedAsBase64;
                asymmetricEncryptionResult.EncryptedAsBytes = encryptedAsBytes;
                asymmetricEncryptionResult.Success = true;
            }
            catch (Exception ex)
            {
                asymmetricEncryptionResult.ExceptionMessage =
                    $"Exception caught while encrypting the message: {ex.Message}";
            }
            return asymmetricEncryptionResult;
        }

        public AsymmetricDecryptionResult DecryptWithFullKeyXml(byte[] cipherBytes, string fullKeyPairXml)
        {
            AsymmetricDecryptionResult asymmetricDecryptionResult = new AsymmetricDecryptionResult();
            try
            {
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
                rsaProvider.FromXmlString(fullKeyPairXml);
                byte[] decryptBytes = rsaProvider.Decrypt(cipherBytes, true);
                asymmetricDecryptionResult.DecryptedMessage = Encoding.UTF8.GetString(decryptBytes);
                asymmetricDecryptionResult.Success = true;
            }
            catch (Exception ex)
            {
                asymmetricDecryptionResult.ExceptionMessage =
                    $"Exception caught while decrypting the cipher: {ex.Message}";
            }
            return asymmetricDecryptionResult;
        }
    }

    public class AsymmetricEncryptionResult : OperationResult
    {
        public byte[] EncryptedAsBytes { get; set; }
        public string EncryptedAsBase64 { get; set; }

    }

    public class AsymmetricDecryptionResult : OperationResult
    {
        public string DecryptedMessage { get; set; }
    }

    public class SymmetricAesEncryptionService
    {
        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="secretKey">A password used to generate a key for encryption.</param>
        public static string Encrypt(string secretKey, string plainText)
        {
            byte[] salt = new byte[32] { 2, 34, 246, 8, 70, 132, 239, 136, 153, 50, 157, 219, 26, 43, 0, 32, 34, 36, 68, 25, 42, 104, 166, 18, 170, 187, 84, 191, 253, 60, 77, 34 };
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(secretKey, salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="secretKey">A password used to generate a key for decryption.</param>
        public static string Decrypt(string secretKey, string cipherText)
        {
            byte[] salt = new byte[32] { 2, 34, 246, 8, 70, 132, 239, 136, 153, 50, 157, 219, 26, 43, 0, 32, 34, 36, 68, 25, 42, 104, 166, 18, 170, 187, 84, 191, 253, 60, 77, 34 };
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(secretKey, salt);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
    }

}
