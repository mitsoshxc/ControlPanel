using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace VPCustInfo
{
    public static class Extensions
    {
        public static void SetSession<T>(this ISession _session, string key, T value)
        {
            _session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetSession<T>(this ISession _session, string key)
        {
            var _value = _session.GetString(key);

            if (_value == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(_value);
            }

            //return _value == null ? default(T) : JsonConvert.DeserializeObject<T>(_value);
        }

        public static string Encrypt(this string _text)
        {
            var _aes = Aes.Create();
            _aes.BlockSize = 128;
            _aes.KeySize = 256;
            // IV property is BlockSize / 8 length
            _aes.IV = System.Text.Encoding.UTF8.GetBytes(@"$R1H&WU3H#HS*6FU");
            _aes.Key = System.Text.Encoding.UTF8.GetBytes(@"JK^JG0DNW(JT$GKV)P9NV!ZW4TVN#IFK");
            _aes.Mode = CipherMode.CBC;
            _aes.Padding = PaddingMode.PKCS7;

            byte[] _src = System.Text.Encoding.Unicode.GetBytes(_text);

            using (ICryptoTransform _encrypt = _aes.CreateEncryptor())
            {
                byte[] _dest = _encrypt.TransformFinalBlock(_src, 0, _src.Length);

                return System.Convert.ToBase64String(_dest);
            }
        }

        public static string Decrypt(this string _text)
        {
            var _aes = Aes.Create();
            _aes.BlockSize = 128;
            _aes.KeySize = 256;
            _aes.IV = System.Text.Encoding.UTF8.GetBytes(@"$R1H&WU3H#HS*6FU");
            _aes.Key = System.Text.Encoding.UTF8.GetBytes(@"JK^JG0DNW(JT$GKV)P9NV!ZW4TVN#IFK");
            _aes.Mode = CipherMode.CBC;
            _aes.Padding = PaddingMode.PKCS7;

            byte[] _src = System.Convert.FromBase64String(_text);

            using (ICryptoTransform _decrypt = _aes.CreateDecryptor())
            {
                byte[] _dest = _decrypt.TransformFinalBlock(_src, 0, _src.Length);

                return System.Text.Encoding.Unicode.GetString(_dest);
            }
        }
    }
}