# Kugou-Lyrics-Decoder
KRC decoder, with a simple demo  

可通过 Id 和 AccessKey 直接通过网络获取歌词 `GetEncryptedLyrics(string, string)`；  
也可以直接解密 KRC 歌词字符串 `DecryptLyrics(string)`。  

`language` tag 里是翻译，Base64解密后即可使用。
