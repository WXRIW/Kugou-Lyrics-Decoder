using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KugouDecoder
{
    public class Helper
    {
        /// <summary>
        /// 通过 ID 和 AccessKey 获取解密后的歌词
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public static string? GetLyrics(string id, string accessKey)
        {
            var encryptedLyrics = GetEncryptedLyrics(id, accessKey);
            var lyrics = Decrypter.DecryptLyrics(encryptedLyrics!);
            return lyrics;
        }

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


        /// <summary>
        /// 检查 KRC 中是否有翻译
        /// </summary>
        public static bool CheckKrcTranslation(string krc)
        {
            if (!krc.Contains("[language:")) return false;

            try
            {
                var language = krc[(krc.IndexOf("[language:") + "[language:".Length)..];
                language = language[..language.IndexOf(']')];
                var decode = Encoding.ASCII.GetString(Convert.FromBase64String(language));

                var translation = JsonSerializer.Deserialize<KugouTranslation>(decode);
                if (translation!.Content!.Count > 0) return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 提取 KRC 中的翻译
        /// </summary>
        /// <param name="krc">KRC 歌词</param>
        /// <returns>翻译 List，若无翻译，则返回 null</returns>
        public static List<string>? GetTranslationFromKrc(string krc)
        {
            if (!krc.Contains("[language:")) return null;

            var language = krc[(krc.IndexOf("[language:") + "[language:".Length)..];
            language = language[..language.IndexOf(']')];
            var decode = Encoding.ASCII.GetString(Convert.FromBase64String(language));

            var translation = JsonSerializer.Deserialize<KugouTranslation>(decode);

            if (translation == null || translation!.Content == null || translation!.Content!.Count == 0) return null;

            try
            {
                var result = new List<string>();
                for (int i = 0; i < translation!.Content![0].LyricContent!.Count; i++)
                {
                    result.Add(translation!.Content![0].LyricContent![i]![0]);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
