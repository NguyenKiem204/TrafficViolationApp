using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace TrafficViolationApp.config
{
    public class UserCredentialManager
    {
        private static readonly string CredentialFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TrafficViolationApp", "credentials.dat");

        private static readonly byte[] EncryptionKey = CreateEncryptionKey("TrafficAppSecretKey123");

        private static byte[] CreateEncryptionKey(string keySource)
        {
            byte[] keyBytes = new byte[32];
            byte[] sourceBytes = Encoding.UTF8.GetBytes(keySource);
            for (int i = 0; i < 32; i++)
            {
                keyBytes[i] = sourceBytes[i % sourceBytes.Length];
            }
            return keyBytes;
        }

        public static void SaveCredentials(string email, string password)
        {
            try
            {
                string directory = Path.GetDirectoryName(CredentialFile);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string credentials = $"{email}|{password}";
                string encryptedData = Encrypt(credentials);
                File.WriteAllText(CredentialFile, encryptedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể lưu thông tin đăng nhập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool TryGetCredentials(out string email, out string password)
        {
            email = string.Empty;
            password = string.Empty;
            try
            {
                if (File.Exists(CredentialFile))
                {
                    string encryptedData = File.ReadAllText(CredentialFile);
                    string credentials = Decrypt(encryptedData);
                    string[] parts = credentials.Split('|');
                    if (parts.Length == 2)
                    {
                        email = parts[0];
                        password = parts[1];
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    if (File.Exists(CredentialFile))
                        File.Delete(CredentialFile);
                }
                catch { }
            }
            return false;
        }

        public static void ClearCredentials()
        {
            try
            {
                if (File.Exists(CredentialFile))
                    File.Delete(CredentialFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa thông tin đăng nhập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string Encrypt(string plainText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = EncryptionKey;
                    aes.GenerateIV();
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi mã hóa: " + ex.Message, ex);
            }
        }

        private static string Decrypt(string cipherText)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = EncryptionKey;
                    byte[] iv = new byte[16];
                    Array.Copy(buffer, 0, iv, 0, iv.Length);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(
                            new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length),
                            decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi giải mã: " + ex.Message, ex);
            }
        }
    }
}