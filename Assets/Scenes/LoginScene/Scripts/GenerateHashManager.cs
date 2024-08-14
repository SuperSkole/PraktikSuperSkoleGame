using System;
using System.Security.Cryptography;
using System.Text;

namespace Scenes.LoginScene.Scripts
{
    public class GenerateHashManager
    {
        // Method to generate a hash from a password and salt
        public static string GenerateHash(string password, string salt)
        {
            // Kombinerer password og salt før hashing
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Method to generate a bit of random salt
        public static string GenerateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}
