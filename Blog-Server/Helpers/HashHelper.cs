using System.Security.Cryptography;
using System.Text;

namespace Blog_Server.Helpers
{
    public class HashHelper
    {
        /// <summary>
        /// Sha256 password encryption
        /// </summary>
        /// <param name="rawString">Raw password string</param>
        /// <returns>Encrypted password string</returns>
        public static string getSHA256(string rawString)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawString));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
