using System.Security.Cryptography;
using System.Text;

namespace MVC.Utils
{
    public static class PasswordHashProvider
    {
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var provider = RandomNumberGenerator.Create())
            {
                provider.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string GetHash(string password, string salt)
        {
            var sha = SHA256.Create();
            var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
