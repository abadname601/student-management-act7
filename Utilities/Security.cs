using System;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementSystem.Utilities
{
    public class Security
    {
        /// <summary>
        /// Computes SHA-256 hash of a given password with salt
        /// </summary>
        public static string HashPassword(string password, string salt = null)
        {
            if (string.IsNullOrEmpty(salt))
            {
                salt = GenerateSalt();
            }

            // Combine password and salt
            string combinedString = password + salt;
            
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
                
                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                
                // Return salt + hash (so we can verify later)
                return salt + ":" + sb.ToString();
            }
        }

        /// <summary>
        /// Verifies a password against a stored hash
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            // Extract the salt
            string[] parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            string salt = parts[0];
            string hash = parts[1];
            
            // Hash the input password with the same salt
            string newHash = HashPassword(password, salt);
            
            // Compare the newly generated hash with the stored hash
            return newHash == storedHash;
        }

        /// <summary>
        /// Generates a random salt for password hashing
        /// </summary>
        public static string GenerateSalt(int length = 16)
        {
            byte[] saltBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Generates a random password with specified complexity
        /// </summary>
        public static string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";
            
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            
            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(validChars.Length);
                sb.Append(validChars[index]);
            }
            
            return sb.ToString();
        }
    }
}
