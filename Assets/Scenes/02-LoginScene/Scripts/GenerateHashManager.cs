using System;
using System.Security.Cryptography;
using System.Text;

namespace Scenes._02_LoginScene.Scripts
{
    /// <summary>
    /// This class is responsible for generating cryptographic hashes and salts, typically used for securing passwords.
    /// </summary>
    public class GenerateHashManager
    {
        /// <summary>
        /// Generates a SHA-256 hash from the provided password and salt.
        /// </summary>
        /// <param name="password">The user's password to hash.</param>
        /// <param name="salt">The cryptographic salt to add randomness to the hash.</param>
        /// <returns>A Base64 encoded string representing the hashed password.</returns>
        public static string GenerateHash(string password, string salt)
        {
            // Combine password and salt into a single byte array
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);

            // Create SHA256 instance and compute the hash
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convert the hash to a Base64 encoded string for storage
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Generates a cryptographically secure random salt of the specified size.
        /// </summary>
        /// <param name="size">The size of the salt in bytes.</param>
        /// <returns>A Base64 encoded string representing the salt.</returns>
        public static string GenerateSalt(int size)
        {
            // Ensure the salt size is positive and appropriate
            if (size <= 0)
            {
                throw new ArgumentException("Size must be a positive integer", nameof(size));
            }

            // Instantiate RNGCryptoServiceProvider for secure random generation
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[size];

                // Fill the buffer with cryptographically strong random bytes
                rng.GetBytes(buffer);

                // Convert the random bytes to a Base64 encoded string
                return Convert.ToBase64String(buffer);
            }
        }
    }
}
