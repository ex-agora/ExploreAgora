﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayExperienceData : GeneralPanelTest
{
    public InputField status;
    public InputField exName;
    public InputField score;

    public ExperiencePlayData getPlayExperienceData ()
    {
        ExperiencePlayData s = new ExperiencePlayData ();
        s.status = Int32.Parse(status.text);
        s.experienceName = exName.text;
        s.score = Int32.Parse (score.text);
        return s;
    }

}
