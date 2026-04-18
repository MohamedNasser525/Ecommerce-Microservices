//using Org.BouncyCastle.Crypto.Engines;
//using Org.BouncyCastle.Crypto.Parameters;
//using Org.BouncyCastle.Crypto;
using System.Text;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AuthServer.Helper
{
    public class RC4
    {
        public static byte[] RC4Decrypt(byte[] data) 
        {
            return RC4Encrypt(data);
        }
        public static byte[] RC4Encrypt(byte[] data) 
        {
            var builder = WebApplication.CreateBuilder();
            string keyText = builder.Configuration["SecretKey"]; ;

            byte[] key = Encoding.UTF8.GetBytes(keyText);
            byte[] s = new byte[256];
            for (int i = 0; i < 256; i++)
                s[i] = (byte)i;

            // KSA
            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i % key.Length]) & 255;
                (s[i], s[j]) = (s[j], s[i]);
            }

            // PRGA
            byte[] result = new byte[data.Length];
            int iIndex = 0;
            j = 0;

            for (int k = 0; k < data.Length; k++)
            {
                iIndex = (iIndex + 1) & 255;
                j = (j + s[iIndex]) & 255;
                (s[iIndex], s[j]) = (s[j], s[iIndex]);

                byte rnd = s[(s[iIndex] + s[j]) & 255];
                result[k] = (byte)(data[k] ^ rnd);
            }

            return result;
        }
    }
}
