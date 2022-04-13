using TodoApi.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TodoApi.Utils
{
    public class PasswordUtil
    {
        public static void HashUserPassword(User user) {
            // Documentation: https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-6.0
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(salt);
            }

            string hashed = GetHashedPassword(salt, user.Password);

            user.Salt = Convert.ToBase64String(salt);
            user.Password = hashed;
        }

        public static bool IsPasswordCorrect(User user, string password) {
            byte[] salt = Convert.FromBase64String(user.Salt);
            string hashedPassword = GetHashedPassword(salt, password);

            return hashedPassword == user.Password;
        }

        private static string GetHashedPassword(byte[] salt, string password) {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));
        }
    }
}