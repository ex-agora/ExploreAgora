using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography; 
using UnityEngine;
using System;

public static class TestEncrypt
{
    static byte[] key;
    private static string hash = "skdjjlkxne3DAFe27ytx37@#387dh48cjiGHGHJG7iy3gjhb873_.d467dh4tx3%$2";

    public static string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider mds = new MD5CryptoServiceProvider())
        {
            string x = "f615a3a8d03a78573e9f541096082e51";
            byte[] key = System.Convert.FromBase64String(x);

            //key = mds.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider mds = new MD5CryptoServiceProvider())
        {
            string x = "f615a3a8d03a78573e9f541096082e51";
            byte[] key = System.Convert.FromBase64String(x);
            //byte[] key = mds.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }
}
