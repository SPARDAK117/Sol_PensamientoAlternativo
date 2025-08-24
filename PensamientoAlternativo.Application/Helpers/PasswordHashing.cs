using System.Security.Cryptography;

namespace PensamientoAlternativo.Application.Helpers
{

    /// <summary>
    /// Hasher de contraseñas con PBKDF2 (SHA-256).
    /// Formato de almacenamiento: "{iterations}.{Base64(salt)}.{Base64(hash)}"
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;        // 128-bit
        private const int KeySize = 32;        // 256-bit
        private const int Iterations = 100_000; // Ajusta según políticas de seguridad
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        /// <summary>
        /// Genera el hash para guardar en base de datos.
        /// </summary>
        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required.", nameof(password));

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Verifica el password en texto plano contra el hash almacenado.
        /// </summary>
        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            var parts = storedHash.Split('.', 3);
            if (parts.Length != 3)
                return false;

            if (!int.TryParse(parts[0], out int iterations) || iterations <= 0)
                return false;

            byte[] salt;
            byte[] hash;

            try
            {
                salt = Convert.FromBase64String(parts[1]);
                hash = Convert.FromBase64String(parts[2]);
            }
            catch
            {
                return false; // Formato inválido
            }

            // Deriva con los mismos parámetros y compara en tiempo constante
            byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, hash.Length);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, hash);
        }

        /// <summary>
        /// (Opcional) Indica si conviene re-hashear con más iteraciones.
        /// Útil si subes Iterations en el futuro.
        /// </summary>
        public static bool NeedsRehash(string storedHash)
        {
            var parts = storedHash.Split('.', 3);
            if (parts.Length != 3) return true;
            return int.TryParse(parts[0], out int iterationsInHash) && iterationsInHash < Iterations;
        }
    }
}
