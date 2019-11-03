using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;
using UnityEngine;
using System;

public static class TestTEstEncrypt
{
    public static string Encrypt(string source, string key)
    {
        TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();

        byte[] byteBuff;

        try
        {
            desCryptoProvider.Key = Encoding.UTF8.GetBytes(key);
            desCryptoProvider.IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH");
            byteBuff = Encoding.UTF8.GetBytes(source);

            string iv = Convert.ToBase64String(desCryptoProvider.IV);
            Console.WriteLine("iv: {0}", iv);

            string encoded =
                Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));

            return encoded;
        }
        catch (Exception except)
        {
            Console.WriteLine(except + "\n\n" + except.StackTrace);
            return null;
        }
    }

    public static string Decrypt(string encodedText, string key)
    {
        TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();

        byte[] byteBuff;

        try
        {
            desCryptoProvider.Key = Encoding.UTF8.GetBytes(key);
            desCryptoProvider.IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH");
            byteBuff = Convert.FromBase64String(encodedText);

            string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return plaintext;
        }
        catch (Exception except)
        {
            Console.WriteLine(except + "\n\n" + except.StackTrace);
            return null;
        }
    }
   /* static void Main(string[] args)
    {
        var securityKey = "abcdefghijklmnop";
        var encrypted = Encrypt("The text to be encrypted", securityKey);

        Console.WriteLine("encrypted as: {0}", encrypted);

        var decrypted = Decrypt(encrypted, securityKey);
        Console.WriteLine("decrypted as: {0}", decrypted);

        Console.ReadLine();
    }*/
}

