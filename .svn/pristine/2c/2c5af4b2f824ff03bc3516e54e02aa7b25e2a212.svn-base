using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace HomotorDepotMgr.Utility
{
    public class SecurityHelp
    {
        public static string GetStrMd5(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString + "55929C89321149DEB027DF43A67EEB3F")));
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}
