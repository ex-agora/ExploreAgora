using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSecurity : MonoBehaviour
{
    #region Fields
    string t;
    #endregion Fields

    #region Methods
    public void TestDecryption(string output)
    {
        if (output == string.Empty) output = t;
        Debug.Log(TestTEstEncrypt.Decrypt(output, "abcdefghijklmnop"));
    }

    public void TestEncryption(string input)
    {
        t = TestTEstEncrypt.Encrypt(input, "abcdefghijklmnop");
        Debug.Log(t);
    }
    #endregion Methods
}
