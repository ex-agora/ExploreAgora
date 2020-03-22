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
    public string GetCountryName() {
        CountriesCodedInfo countriesInfo = CountryHolder.Instance.GetCountries();
        return countriesInfo.countries[countryDD.value].country;
    }
}
