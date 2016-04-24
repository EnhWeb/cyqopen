using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace AdKiller
{
    public class DESCrypt
    {
        //private static DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
        ////默认密钥向量
        //private static byte[] IVKeys = { 0x12, 0xAB, 0x34, 0xCD, 0x98, 0xEF, 0x01, 0x02 };
        /**/
        /// <summary>
        /// DES加密
        /// </summary>
        public static byte[] Crypt(byte[] data, bool isEnCrypt)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(255 - data[i]);
            }
            return data;
            //if (data != null && data.Length > 0 && !string.IsNullOrEmpty(key))
            //{
            //    if (key.Length < 8)
            //    {
            //        key += new string('0', 8 - key.Length);
            //    }
            //    else if (key.Length > 8)
            //    {
            //        key = key.Substring(0, 8);
            //    }
            //    MemoryStream mStream = new MemoryStream();
            //    CryptoStream cStream = null;
            //    try
            //    {
            //        byte[] rgbKey = Encoding.UTF8.GetBytes(key);
            //        ICryptoTransform ic = isEnCrypt ? dCSP.CreateEncryptor(rgbKey, IVKeys) : dCSP.CreateDecryptor(rgbKey, IVKeys);
            //        cStream = new CryptoStream(mStream, ic, CryptoStreamMode.Write);
            //        cStream.Write(data, 0, data.Length);
            //        cStream.FlushFinalBlock();
            //        data = mStream.ToArray();

            //    }
            //    catch (Exception err)
            //    {
            //        DebugLog.WriteError(err);
            //    }
            //    finally
            //    {
            //        mStream.Close();
            //        cStream.Close();
            //    }
            //}
           // return data;
        }

    }
}