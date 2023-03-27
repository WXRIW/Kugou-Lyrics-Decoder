namespace Kugou_Lyrics_Decoder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Demo
            var encryptedLyrics = KugouDecoder.Decrypter.GetEncryptedLyrics("49037603", "92A72D6AAD4C1A4736169984EBB64ADB");
            var lyrics = KugouDecoder.Decrypter.DecryptLyrics(encryptedLyrics!);
            Console.WriteLine(lyrics);
        }
    }
}