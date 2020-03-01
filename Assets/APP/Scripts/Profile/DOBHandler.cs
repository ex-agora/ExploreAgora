using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DOBHandler : MonoBehaviour
{
    [SerializeField] private Dropdown day;
    [SerializeField] private Dropdown month;
    [SerializeField] private Dropdown year;
    [HideInInspector] [SerializeField] private List<string> temp;
    private int currentYear;

    private DateTime dob;

    public DateTime DOB { get { SetDateOfBirth(); return dob; } set { dob = value; HandleDate(); } }

    private void Start()
    {
        month.onValueChanged.AddListener(delegate { HandleDay(); });
        year.onValueChanged.AddListener(delegate { HandleDay(); });

        InputHandler();
        HandleDay();
    }
    private void InputHandler()
    {
        temp.Clear();

        /* for (int i = 1; i <= 12; i++)
             temp.Add(i.ToString());*/
        foreach (var item in DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames)
        {
            if (item != "")
            {
                temp.Add(item);
            }
        }
        month.ClearOptions();
        month.AddOptions(temp);

        currentYear = DateTime.Now.Year;
        temp.Clear();

        for (int i = currentYear - 50; i <= currentYear; i++)
            temp.Add(i.ToString());

        year.ClearOptions();
        year.AddOptions(temp);

    }

    private void HandleDay()
    {
        temp.Clear();
        for (int i = 1; i <= System.DateTime.DaysInMonth(int.Parse(year.options[year.value].text), month.value + 1); i++)
            temp.Add(i.ToString());

        day.ClearOptions();
        day.AddOptions(temp);
    }

    private void HandleDate()
    {
        year.value = dob.Year - (DateTime.Now.Year - 50);
        month.value = dob.Month - 1;
        day.value = dob.Day - 1;
    }

    private void SetDateOfBirth()
    {
        dob = new DateTime(int.Parse(year.options[year.value].text), int.Parse(month.options[month.value].text), int.Parse(day.options[day.value].text));
    }
}