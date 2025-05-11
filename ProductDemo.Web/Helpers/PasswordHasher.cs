using System.Security.Cryptography;

namespace ProductDemo.Web.Helpers;

public class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
        byte[] salt = new byte[SaltSize];
        RandomNumberGenerator.Fill(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(KeySize);

        byte[] hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(KeySize);

        for (int i = 0; i < KeySize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}

