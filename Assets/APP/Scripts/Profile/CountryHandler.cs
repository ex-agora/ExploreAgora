using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryHandler : MonoBehaviour
{
    [SerializeField] Dropdown countryDD;
    int selectedCountryindex = -1;
    private void Start()
    {
        countryDD.ClearOptions();
        CountriesCodedInfo countriesInfo = CountryHolder.Instance.GetCountries();
        int countrySize = countriesInfo.countries.Length;
        List<string> countryNames = new List<string>();
        for (int i = 0; i < countrySize; i++) {
            countryNames.Add(countriesInfo.countries[i].country);
        }
        countryDD.AddOptions(countryNames);
    }
    public void SetCountry(string _country) {
        CountriesCodedInfo countriesInfo = CountryHolder.Instance?.GetCountries();
        if (countriesInfo != null && countriesInfo?.countries !=null)
        {
            for (int i = 0; i < countriesInfo.countries.Length; i++)
            {
                if (countriesInfo.countries[i].country == _country)
                {
                    countryDD.value = i;
                    break;
                }
            }
        }
    }
    public string GetCountryName() {
        CountriesCodedInfo countriesInfo = CountryHolder.Instance.GetCountries();
        return countriesInfo.countries[countryDD.value].country;
    }
}
