using System;
using System.Text;
using System.Security.Cryptography;

namespace ClassLib.DataStructures
{
    /// <summary>
    /// Klasa haszująca oraz sprawdzająca hasła użytkowników systemu.
    /// <remarks>Używa metodę PBKDF2-SHA1. Oparte na rozwiązaniu na https://crackstation.net/hashing-security.htm#aspsourcecode </remarks>
    /// </summary>
    public sealed class HashClass
    {
        /// <summary>
        /// Rozmiar, w bajtach, ziarnka soli
        /// </summary>
        public const int SALT_BYTE_SIZE = 16;
        /// <summary>
        /// Rozmiar, w bajtach, hasha
        /// </summary>
        public const int HASH_BYTE_SIZE = 24;
        /// <summary>
        /// Liczba iteracji haszowania PBKDF2
        /// </summary>
        public const int PBKDF2_ITERATIONS = 500;

        // Hasła są składowane w postaci LICZBA_ITERACJI:ZIARNKO_SOLI:HASH_HASLA
        public const int ITERATION_INDEX = 0;
        public const int SALT_INDEX = 1;
        public const int PBKDF2_INDEX = 2;

        public const char DELIMITER = ':';

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2DerivedBytes = new Rfc2898DeriveBytes(password, salt);
            pbkdf2DerivedBytes.IterationCount = iterations;
            return pbkdf2DerivedBytes.GetBytes(outputBytes);
        }

        private static bool SlowEquals(byte[] hashA, byte[] hashB)
        {
            uint diff = (uint)hashA.Length ^ (uint)hashB.Length;
            for (int i = 0; i < hashA.Length && i < hashB.Length; i++)
                diff |= (uint)(hashA[i] ^ hashB[i]);
            return diff == 0;
        }

        public static string CreateHash(string password)
        {
            // Generowanie ziarenka soli
            RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            cryptoServiceProvider.GetBytes(salt);

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);

            return String.Format("{0}{1}{2}{1}{3}", PBKDF2_ITERATIONS, DELIMITER, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            string[] split = correctHash.Split(new char[] { DELIMITER });
            int iterations = Int32.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }
    }
}
