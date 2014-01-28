using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomAuth.Data
{
    //public class Program
    //{
    //    public static void Main()
    //    {
    //        var password = "FooBar@!";
    //        var salt = PasswordHasher.GenerateSalt();
    //        var hashedPassword = PasswordHasher.HashPassword(password, salt);

    //        //later in time
    //        var userEnteredPassword = "FooBar@!";
    //        var newHash = PasswordHasher.HashPassword(userEnteredPassword, salt);
    //        bool doesMatch = newHash == hashedPassword;
    //        Console.WriteLine(doesMatch);

    //        Console.WriteLine(hashedPassword);
    //        Console.WriteLine(newHash);
    //        Console.ReadKey(true);

    //    }
    //}

    public static class PasswordHasher
    {
        public static string GenerateSalt()
        {
            byte[] bytes = new byte[10];
            new RNGCryptoServiceProvider().GetNonZeroBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] combined = new byte[passwordBytes.Length + saltBytes.Length];
            for (int i = 0; i < passwordBytes.Length; i++)
            {
                combined[i] = passwordBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                combined[passwordBytes.Length + i] = saltBytes[i];
            }
            
            var algorithm = new SHA256Managed();
            byte[] hashed = algorithm.ComputeHash(combined);
            return Convert.ToBase64String(hashed);
        }
    }
}
