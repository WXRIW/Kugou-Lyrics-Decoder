# Kugou Lyrics Decoder
KRC decoder, with a simple demo  

可通过 Id 和 AccessKey 直接通过网络获取歌词 `Helper.GetEncryptedLyrics(string, string)`；  
也可以直接解密 KRC 歌词字符串 `Decrypter.DecryptLyrics(string)`。  

`language` tag 里是翻译，Base64解密后即可使用。  
也可以通过 `Helper.GetTranslationFromKrc(string)` 获取翻译字符串列表。  
