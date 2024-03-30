namespace MediportaKMZadanieRekrutacyjne.Crypto
{
    public static class Decrypter
    {
        /// <summary>
        /// Decrypt StackExchange api key to usable form
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string DecryptKey(string encryptedText)
        {
            var decryptedKey = string.Empty;

            foreach (char c in encryptedText)
            {
                decryptedKey += Convert.ToChar(c + 1);
            }

            return decryptedKey;
        }
    }
}
