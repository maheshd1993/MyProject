using Svam.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Svam.UtilityManager
{
    public class StringCipher
    {

        public  string Encrypt(string clearText)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(clearText);
            return Convert.ToBase64String(encoded);
        }

        public string Decrypt(string cipherText)
        {
            byte[] encoded = Convert.FromBase64String(cipherText);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        public static string DecryptDate(string cipherStartText, string cipherEndText)
        {
            string base64StartDate = EncodeDecodeForBase64.DecodeBase64(cipherStartText);
            string base64EndDate = EncodeDecodeForBase64.DecodeBase64(cipherEndText);
            DateTime ExpireStarDate = DateTime.ParseExact(base64StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime ExpireEndDate = DateTime.ParseExact(base64EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var startEndDate = String.Format("Star Date: {0:d} End Date:\n{1:d}", ExpireStarDate.ToString("yyyy-MM-dd"), ExpireEndDate.ToString("yyyy-MM-dd"));
            return startEndDate;
        }
    }
}