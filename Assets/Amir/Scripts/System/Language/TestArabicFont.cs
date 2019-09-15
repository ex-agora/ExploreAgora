using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using ArabicSupport;
public class TestArabicFont : MonoBehaviour
{

    Text myText; //You can also make this public and attach your UI text here.

    string sampleString = "امير طارق";
    
    void Awake()
    {
        myText = GetComponent<Text>();
    }

    void Start()
    {

        List<string> listofWords = sampleString.Split('\n').ToList(); //Extract words from the sentence
        myText.text = ArabicFixer.Fix(sampleString, false, false);

        //Font f = myText.font;
        
        //myText.font.characterInfo = ReverseFont(f.characterInfo);
        //foreach (string s in listofWords)
        //{
        //    myText.text += Reverse(s) + "\n"; //Add a new line feed at the end, since we cannot accomodate more characters here.
        //}

    }
    public static CharacterInfo[] ReverseFont(CharacterInfo[] infos) {
        Array.Reverse(infos);
        return infos;
    }

    public static string Reverse(string s)
    {
        
        string reverse = String.Empty;
        for (int i = s.Length - 1; i > -1; i--)
        {
            reverse += (char)(1*s[i]);
        }
        return reverse;
    }
}
