namespace Kugou_Lyrics_Decoder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Demo
            var lyrics = KugouDecoder.Helper.GetLyrics("49037603", "92A72D6AAD4C1A4736169984EBB64ADB");
            Console.WriteLine(lyrics);
        }
    }
}