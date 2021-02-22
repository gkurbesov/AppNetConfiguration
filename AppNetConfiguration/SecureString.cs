using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppNetConfiguration
{
    /// <summary>
    /// Protected text string
    /// </summary>
    public class SecureString
    {
        /// <summary>
        /// Encrypted content
        /// </summary>
        public string SecureValue
        {
            get { return !string.IsNullOrEmpty(RawValue) ? Encrypt(RawValue, InitSeed, ValueEncoding) : string.Empty; }
            set { RawValue = TryParse(value, out var output) ? output.RawValue : string.Empty; }
        }
        private string RawValue;
        private Encoding ValueEncoding;
        private byte InitSeed;

        public SecureString()
        {
            RawValue = string.Empty;
            ValueEncoding = Encoding.UTF8;
            InitSeed = 0x06;
        }

        public SecureString(string value)
        {
            RawValue = value;
            ValueEncoding = Encoding.UTF8;
            InitSeed = 0x06;
        }

        public SecureString(string value, Encoding encoding)
        {
            RawValue = value;
            ValueEncoding = encoding;
            InitSeed = 0x06;
        }

        public SecureString(string value, byte seed)
        {
            RawValue = value;
            ValueEncoding = Encoding.UTF8;
            InitSeed = seed;
        }

        public SecureString(string value, Encoding encoding, byte seed)
        {
            RawValue = value;
            ValueEncoding = encoding;
            InitSeed = seed;
        }

        private static string Encrypt(string raw, byte init_seed, Encoding encoding)
        {
            var byte_arr = encoding.GetBytes(raw);
            byte seed = init_seed;
            List<byte> enc = new List<byte>();
            for (int i = 0; i < byte_arr.Count(); i++)
            {
                seed = (byte)(byte_arr[i] ^ seed);
                enc.Add(seed);
            }
            return BitConverter.ToString(enc.ToArray()).Replace("-", "");
        }

        private static string Decrypt(string value, byte init_seed, Encoding encoding)
        {
            var enc_arr = Enumerable.Range(0, value.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                     .ToArray();
            List<byte> dec = new List<byte>();
            byte next_seed;
            for (int i = enc_arr.Count() - 1; i >= 0; i--)
            {
                next_seed = (i > 0) ? enc_arr[i - 1] : init_seed;
                dec.Insert(0, (byte)(enc_arr[i] ^ next_seed));
            }
            return encoding.GetString(dec.ToArray());
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <returns></returns>
        public static SecureString Parse(string value)
        {
            return Parse(value, Encoding.UTF8, 0x06);
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <param name="encoding">Text encoding</param>
        /// <returns></returns>
        public static SecureString Parse(string value, Encoding encoding)
        {
            return Parse(value, encoding, 0x06);
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <param name="seed">Byte to initiate encryption</param>
        /// <returns></returns>
        public static SecureString Parse(string value, byte seed)
        {
            return Parse(value, Encoding.UTF8, seed);
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <param name="encoding">Text encoding</param>
        /// <param name="seed">Byte to initiate encryption</param>
        /// <returns></returns>
        public static SecureString Parse(string value, Encoding encoding, byte seed)
        {
            return new SecureString(Decrypt(value, seed, encoding), encoding, seed);
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out SecureString output)
        {
            try
            {
                output = !string.IsNullOrEmpty(value) ? Parse(value) : new SecureString();
                return !string.IsNullOrEmpty(value);
            }
            catch
            {
                output = new SecureString();
                return false;
            }
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <param name="encoding">Text encoding</param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool TryParse(string value, Encoding encoding, out SecureString output)
        {
            try
            {
                output = !string.IsNullOrEmpty(value) ? Parse(value, encoding) : new SecureString();
                return !string.IsNullOrEmpty(value);
            }
            catch
            {
                output = new SecureString();
                return false;
            }
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <param name="seed">Byte to initiate encryption</param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool TryParse(string value, byte seed, out SecureString output)
        {
            try
            {
                output = !string.IsNullOrEmpty(value) ? Parse(value, seed) : new SecureString();
                return !string.IsNullOrEmpty(value);
            }
            catch
            {
                output = new SecureString();
                return false;
            }
        }
        /// <summary>
        /// Converts the secure string representation to its raw string equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">Encrypted string</param>
        /// <param name="encoding">Text encoding</param>
        /// <param name="seed">Byte to initiate encryption</param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool TryParse(string value, Encoding encoding, byte seed, out SecureString output)
        {
            try
            {
                output = !string.IsNullOrEmpty(value) ? Parse(value, encoding, seed) : new SecureString();
                return !string.IsNullOrEmpty(value);
            }
            catch
            {
                output = new SecureString();
                return false;
            }
        }
        /// <summary>
        /// Returns plain string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return RawValue;
        }

        public static implicit operator SecureString(string param)
        {
            return new SecureString(param);
        }

        public static implicit operator string(SecureString param)
        {
            return param.RawValue;
        }
    }
}
