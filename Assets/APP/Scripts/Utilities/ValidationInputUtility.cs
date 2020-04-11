using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ValidationInputUtility 
{
    public static bool VerifyEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public static bool VerifyPassword(string pw) => VerifyPasswordLength(pw, 8);

    static bool VerifyPasswordLength(string pw, int minSize) => pw.Length > minSize - 1;

    public static bool IsEmptyOrNull(string str) => string.IsNullOrEmpty(str);

    public static bool IsAlpha(string str) => Regex.IsMatch(str, @"^[a-zA-Z]+$");
    public static bool IsDigit(string str) => Regex.IsMatch(str, @"^[0-9]+$");
    public static bool IsAlphOrDigit(string str) => Regex.IsMatch(str, @"^[a-zA-Z0-9]+$");
    

}
