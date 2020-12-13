using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetExperienceData : GeneralPanelTest
{
    public Text experiences;
    public void ShowData (string  data)
    {
        experiences.text = data;
    }
}
