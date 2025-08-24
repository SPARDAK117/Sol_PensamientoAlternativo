using System.Security.Cryptography;
namespace PensamientoAlternativo.Application.Helpers
{
    public static class Bcrypt
    {
        public static string Hash(string plaintext, int workFactor = 12) =>
            BCrypt.Net.BCrypt.HashPassword(plaintext, workFactor);

        public static bool Verify(string plaintext, string storedHash) =>
            BCrypt.Net.BCrypt.Verify(plaintext, storedHash);
    }

    public static class PasswordHashing
    {
        private const int SaltSize = 16;   // 128-bit
        private const int KeySize = 32;   // 256-bit
        private const int Iterations = 100_000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        // Para REGISTRO o seed: genera el hash a guardar en BD
        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        // Para LOGIN: verifica texto plano contra hash almacenado
        public static bool Verify(string password, string hashString)
        {
            var parts = hashString.Split('.', 3);
            if (parts.Length != 3) return false;

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] hash = Convert.FromBase64String(parts[2]);

            byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, iterations, Algorithm, hash.Length);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, hash);
        }
    }

}
