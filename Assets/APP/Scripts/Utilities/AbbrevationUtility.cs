using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class AbbrevationUtility
{
    private static readonly SortedDictionary<long, string> abbrevations = new SortedDictionary<long, string>
     {
         {1000,"K"},
         {1000000, "M" },
         {1000000000, "B" },
         {1000000000000,"T"}
     };

    public static string AbbreviateNumber(float number)
    {
        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<long, string> pair = abbrevations.ElementAt(i);
            if (Mathf.Abs(number) >= pair.Key)
            {
                number *= 10;
                float roundedNumber = Mathf.Floor(number / pair.Key);
                roundedNumber /= 10;
                return roundedNumber.ToString() + pair.Value;
            }
        }
        return number.ToString();
    }
}