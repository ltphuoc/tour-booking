using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Common
{
    public class Utils
    {
        // Hashes the password using SHA-256 algorithm
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static string EncodePassword(string password)
        {
            // Create a new instance of the SHA-256 hashing algorithm
            using (var sha256 = SHA256.Create())
            {
                // Convert the password string to a byte array
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash value of the password bytes
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash bytes to a string
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }

        public static bool IsPasswordEncoded(string password)
        {
            // Check if the password is exactly 64 characters long
            return password.Length == 64;
        }
    }
}
