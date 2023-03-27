using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KugouDecoder
{
    public class Decrypter
    {
        /// <summary>
        /// 通过 ID 和 AccessKey 获取加密的歌词
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public static string? GetEncryptedLyrics(string id, string accessKey)
        {
            var json = new HttpClient().GetStringAsync($"https://lyrics.kugou.com/download?ver=1&client=pc&id={id}&accesskey={accessKey}&fmt=krc&charset=utf8").Result;
            try
            {
                var response = JsonSerializer.Deserialize<KugouLyricsResponse>(json);
                return response?.Content;
            }
            catch
            {
                return null;
            }
        }

        public readonly static byte[] DecryptKey = { 0x40, 0x47, 0x61, 0x77, 0x5e, 0x32, 0x74, 0x47, 0x51, 0x36, 0x31, 0x2d, 0xce, 0xd2, 0x6e, 0x69 };

        /// <summary>
        /// 解密歌词
        /// </summary>
        /// <param name="encryptedLyrics">加密的歌词</param>
        /// <returns>解密后的 KRC 歌词</returns>
        public static string? DecryptLyrics(string encryptedLyrics)
        {
            var data = Convert.FromBase64String(encryptedLyrics)[4..];

            for (var i = 0; i < data.Length; ++i)
            {
                data[i] = (byte)(data[i] ^ DecryptKey[i % DecryptKey.Length]);
            }

            var res = Encoding.UTF8.GetString(SharpZipLibDecompress(data));
            return res[1..];
        }

        public static byte[] SharpZipLibDecompress(byte[] data)
        {
            var compressed = new MemoryStream(data);
            var decompressed = new MemoryStream();
            var inputStream = new InflaterInputStream(compressed);

            inputStream.CopyTo(decompressed);

            return decompressed.ToArray();
        }
    }
}
