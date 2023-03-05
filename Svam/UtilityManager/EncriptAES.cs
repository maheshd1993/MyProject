using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Threading;
using System.Data;


namespace Svam.UtilityManager
{
    class EncriptAES
    {
        
        public static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
           string DecodeTexts = ConvertTextEncript(plainText);
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            //encryptor.Padding = PaddingMode.Zeros;

            // Set key and IV
            encryptor.Key = key;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(DecodeTexts);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;
        }
        public static string ConvertTextEncript(string InputText)
        {
            string OutPut = "";

            for (int p = 0; p < InputText.Length; p++)
            {
                OutPut = OutPut + ReturnTextEncript(InputText.Substring(p, 1));
            }
             return OutPut;
        }
        public static string ConvertTextDecript(string InputText)
        {
            string OutPut = "";
            //int de = 0;
            for (int p = 0; p < InputText.Length; p=p+2)
            {
                //if (p == 0)
                //    de = 0;
                //else
                //    de = de + 1;
                OutPut = OutPut + ReturnTextDecript(InputText.Substring(p, 2));
            }
            return OutPut;
        }
        public static string ReturnTextEncript(string InputChar)
        {
            string OutPutChar = "";
            if (InputChar == "a")
                OutPutChar = "1p";
            else if (InputChar == "b")
                OutPutChar = "1q";
            else if (InputChar == "c")
                OutPutChar = "1r";
            else if (InputChar == "d")
                OutPutChar = "1s";
            else if (InputChar == "e")
                OutPutChar = "1t";
            else if (InputChar == "f")
                OutPutChar = "1u";
            else if (InputChar == "g")
                OutPutChar = "1v";
            else if (InputChar == "h")
                OutPutChar = "1w";
            else if (InputChar == "i")
                OutPutChar = "1x";
            else if (InputChar == "j")
                OutPutChar = "1y";
            else if (InputChar == "k")
                OutPutChar = "1z";
            else if (InputChar == "l")
                OutPutChar = "1a";
            else if (InputChar == "m")
                OutPutChar = "1b";
            else if (InputChar == "n")
                OutPutChar = "1c";
            else if (InputChar == "o")
                OutPutChar = "1d";
            else if (InputChar == "p")
                OutPutChar = "1e";
            else if (InputChar == "q")
                OutPutChar = "1f";
            else if (InputChar == "r")
                OutPutChar = "1g";
            else if (InputChar == "s")
                OutPutChar = "1h";
            else if (InputChar == "t")
                OutPutChar = "1i";
            else if (InputChar == "u")
                OutPutChar = "1j";
            else if (InputChar == "v")
                OutPutChar = "1k";
            else if (InputChar == "w")
                OutPutChar = "1l";
            else if (InputChar == "x")
                OutPutChar = "1m";
            else if (InputChar == "y")
                OutPutChar = "1n";
            else if (InputChar == "z")
                OutPutChar = "1o";

            else if (InputChar == "A")
                OutPutChar = "3M";
            else if (InputChar == "B")
                OutPutChar = "3N";
            else if (InputChar == "C")
                OutPutChar = "3O";
            else if (InputChar == "D")
                OutPutChar = "3P";
            else if (InputChar == "E")
                OutPutChar = "3Q";
            else if (InputChar == "F")
                OutPutChar = "3R";
            else if (InputChar == "G")
                OutPutChar = "3S";
            else if (InputChar == "H")
                OutPutChar = "3T";
            else if (InputChar == "I")
                OutPutChar = "3U";
            else if (InputChar == "J")
                OutPutChar = "3V";
            else if (InputChar == "K")
                OutPutChar = "3W";
            else if (InputChar == "L")
                OutPutChar = "3X";
            else if (InputChar == "M")
                OutPutChar = "3Y";
            else if (InputChar == "N")
                OutPutChar = "3Z";
            else if (InputChar == "P")
                OutPutChar = "3A";
            else if (InputChar == "Q")
                OutPutChar = "3B";
            else if (InputChar == "R")
                OutPutChar = "3C";
            else if (InputChar == "S")
                OutPutChar = "3D";
            else if (InputChar == "T")
                OutPutChar = "3E";
            else if (InputChar == "U")
                OutPutChar = "3F";
            else if (InputChar == "V")
                OutPutChar = "3G";
            else if (InputChar == "W")
                OutPutChar = "3H";
            else if (InputChar == "X")
                OutPutChar = "3I";
            else if (InputChar == "Y")
                OutPutChar = "3J";
            else if (InputChar == "Z")
                OutPutChar = "3K";
            else if (InputChar == "O")
                OutPutChar = "3L";

            else if (InputChar == "1")
                OutPutChar = "a9";
            else if (InputChar == "2")
                OutPutChar = "b0";
            else if (InputChar == "3")
                OutPutChar = "c1";
            else if (InputChar == "4")
                OutPutChar = "d2";
            else if (InputChar == "5")
                OutPutChar = "e3";
            else if (InputChar == "6")
                OutPutChar = "f4";
            else if (InputChar == "7")
                OutPutChar = "g5";
            else if (InputChar == "8")
                OutPutChar = "h6";
            else if (InputChar == "9")
                OutPutChar = "i7";
            else if (InputChar == "0")
                OutPutChar = "j8";
            else if (InputChar == "`")
                OutPutChar = "AZ";
            else if (InputChar == "~")
                OutPutChar = "BY";
            else if (InputChar == "!")
                OutPutChar = "CX";
            else if (InputChar == "@")
                OutPutChar = "DW";
            else if (InputChar == "#")
                OutPutChar = "EV";
            else if (InputChar == "$")
                OutPutChar = "FU";
            else if (InputChar == "%")
                OutPutChar = "GT";
            else if (InputChar == "^")
                OutPutChar = "HS";
            else if (InputChar == "&")
                OutPutChar = "IR";
            else if (InputChar == "*")
                OutPutChar = "JQ";
            else if (InputChar == "(")
                OutPutChar = "KP";
            else if (InputChar == ")")
                OutPutChar = "LO";
            else if (InputChar == "^")
                OutPutChar = "MN";
            else if (InputChar == "{")
                OutPutChar = "NM";
            else if (InputChar == "}")
                OutPutChar = "OL";
            else if (InputChar == "[")
                OutPutChar = "PK";
            else if (InputChar == "]")
                OutPutChar = "QJ";
            else if (InputChar == "<")
                OutPutChar = "RI";
            else if (InputChar == ">")
                OutPutChar = "SH";
            else if (InputChar == ",")
                OutPutChar = "TG";
            else if (InputChar == ".")
                OutPutChar = "UF";
            else if (InputChar == "?")
                OutPutChar = "VE";
            else if (InputChar == "/")
                OutPutChar = "WD";
            else if (InputChar == "-")
                OutPutChar = "XC";
            else if (InputChar == "_")
                OutPutChar = "YB";
            else if (InputChar == "+")
                OutPutChar = "ZA";
            else if (InputChar == "|")
                OutPutChar = "ha";
            else if (InputChar == "'")
                OutPutChar = "ib";
            else
                OutPutChar = InputChar;

            return OutPutChar;
        }
        public static string ReturnTextDecript(string InputChar)
        {
            string OutPutChar = "";
            if (InputChar == "1p")
                OutPutChar = "a";
            else if (InputChar == "1q")
                OutPutChar = "b";
            else if (InputChar == "1r")
                OutPutChar = "c";
            else if (InputChar == "1s")
                OutPutChar = "d";
            else if (InputChar == "1t")
                OutPutChar = "e";
            else if (InputChar == "1u")
                OutPutChar = "f";
            else if (InputChar == "1v")
                OutPutChar = "g";
            else if (InputChar == "1w")
                OutPutChar = "h";
            else if (InputChar == "1x")
                OutPutChar = "i";
            else if (InputChar == "1y")
                OutPutChar = "j";
            else if (InputChar == "1z")
                OutPutChar = "k";
            else if (InputChar == "1a")
                OutPutChar = "l";
            else if (InputChar == "1b")
                OutPutChar = "m";
            else if (InputChar == "1c")
                OutPutChar = "n";
            else if (InputChar == "1d")
                OutPutChar = "o";
            else if (InputChar == "1e")
                OutPutChar = "p";
            else if (InputChar == "1f")
                OutPutChar = "q";
            else if (InputChar == "1g")
                OutPutChar = "r";
            else if (InputChar == "1h")
                OutPutChar = "s";
            else if (InputChar == "1i")
                OutPutChar = "t";
            else if (InputChar == "1j")
                OutPutChar = "u";
            else if (InputChar == "1k")
                OutPutChar = "v";
            else if (InputChar == "1l")
                OutPutChar = "w";
            else if (InputChar == "1m")
                OutPutChar = "x";
            else if (InputChar == "1n")
                OutPutChar = "y";
            else if (InputChar == "1o")
                OutPutChar = "z";

            else if (InputChar == "3M")
                OutPutChar = "A";
            else if (InputChar == "3N")
                OutPutChar = "B";
            else if (InputChar == "3O")
                OutPutChar = "C";
            else if (InputChar == "3P")
                OutPutChar = "D";
            else if (InputChar == "3Q")
                OutPutChar = "E";
            else if (InputChar == "3R")
                OutPutChar = "F";
            else if (InputChar == "3S")
                OutPutChar = "G";
            else if (InputChar == "3T")
                OutPutChar = "H";
            else if (InputChar == "3U")
                OutPutChar = "I";
            else if (InputChar == "3V")
                OutPutChar = "J";
            else if (InputChar == "3W")
                OutPutChar = "K";
            else if (InputChar == "3X")
                OutPutChar = "L";
            else if (InputChar == "3Y")
                OutPutChar = "M";
            else if (InputChar == "3Z")
                OutPutChar = "N";
            else if (InputChar == "3A")
                OutPutChar = "P";
            else if (InputChar == "3B")
                OutPutChar = "Q";
            else if (InputChar == "3C")
                OutPutChar = "R";
            else if (InputChar == "3D")
                OutPutChar = "S";
            else if (InputChar == "3E")
                OutPutChar = "T";
            else if (InputChar == "3F")
                OutPutChar = "U";
            else if (InputChar == "3G")
                OutPutChar = "V";
            else if (InputChar == "3H")
                OutPutChar = "W";
            else if (InputChar == "3I")
                OutPutChar = "X";
            else if (InputChar == "3J")
                OutPutChar = "Y";
            else if (InputChar == "3K")
                OutPutChar = "Z";
            else if (InputChar == "3L")
                OutPutChar = "O";

            else if (InputChar == "a9")
                OutPutChar = "1";
            else if (InputChar == "b0")
                OutPutChar = "2";
            else if (InputChar == "c1")
                OutPutChar = "3";
            else if (InputChar == "d2")
                OutPutChar = "4";
            else if (InputChar == "e3")
                OutPutChar = "5";
            else if (InputChar == "f4")
                OutPutChar = "6";
            else if (InputChar == "g5")
                OutPutChar = "7";
            else if (InputChar == "h6")
                OutPutChar = "8";
            else if (InputChar == "i7")
                OutPutChar = "9";
            else if (InputChar == "j8")
                OutPutChar = "0";

            else if (InputChar == "AZ")
                OutPutChar = "`";
            else if (InputChar == "BY")
                OutPutChar = "~";
            else if (InputChar == "CX")
                OutPutChar = "!";
            else if (InputChar == "DW")
                OutPutChar = "@";
            else if (InputChar == "EV")
                OutPutChar = "#";
            else if (InputChar == "FU")
                OutPutChar = "$";
            else if (InputChar == "GT")
                OutPutChar = "%";
            else if (InputChar == "HS")
                OutPutChar = "^";
            else if (InputChar == "IR")
                OutPutChar = "&";
            else if (InputChar == "JQ")
                OutPutChar = "*";
            else if (InputChar == "KP")
                OutPutChar = "(";
            else if (InputChar == "LO")
                OutPutChar = ")";
            else if (InputChar == "MN")
                OutPutChar = "^";
            else if (InputChar == "NM")
                OutPutChar = "{";
            else if (InputChar == "OL")
                OutPutChar = "}";
            else if (InputChar == "PK")
                OutPutChar = "[";
            else if (InputChar == "QJ")
                OutPutChar = "]";
            else if (InputChar == "RI")
                OutPutChar = "<";
            else if (InputChar == "SH")
                OutPutChar = ">";
            else if (InputChar == "TG")
                OutPutChar = ",";
            else if (InputChar == "UF")
                OutPutChar = ".";
            else if (InputChar == "VE")
                OutPutChar = "?";
            else if (InputChar == "WD")
                OutPutChar = "/";
            else if (InputChar == "XC")
                OutPutChar = "-";
            else if (InputChar == "YB")
                OutPutChar = "_";
            else if (InputChar == "ZA")
                OutPutChar = "+";
            else if (InputChar == "ha")
                OutPutChar = "|";
            else if (InputChar == "ib")
                OutPutChar = "'";

            else
                OutPutChar = InputChar;

            return OutPutChar;
        }
        public static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            string DecodeTexts = "";
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            //encryptor.Padding = PaddingMode.Zeros;

            // Set key and IV
            encryptor.Key = key;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the encrypted byte array to a base64 encoded string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
                DecodeTexts = ConvertTextDecript(plainText);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the encrypted data as a string
            return DecodeTexts;
        }
        public static byte[] getdcriptkey(out byte[] iv)
        {
            byte[] key;
            string password = "LIVE2SPBNI99545";
            SHA256 mySHA256 = SHA256Managed.Create();
            // Create secret IV
            iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            return key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(password));
        }
    }
   
}
