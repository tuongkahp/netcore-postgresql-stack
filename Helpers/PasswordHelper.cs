using System.Security.Cryptography;
using System.Text;

namespace Helpers;

public static class PasswordHelper
{
    public static string GenerateHash(string input, string salt)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
        SHA256 sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static bool Equals(string plainTextInput, string hashedInput, string salt)
    {
        string newHashedPin = GenerateHash(plainTextInput, salt);
        return newHashedPin.Equals(hashedInput);
    }
}