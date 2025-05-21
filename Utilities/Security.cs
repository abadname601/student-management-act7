using System;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementSystem.Utilities
{
    public class Security
    {
        /// <summary>
        /// Computes MD5 hash of a given password
        /// </summary>
        public static string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Verifies a password against a stored hash
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            string computedHash = HashPassword(password);
            return computedHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
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
