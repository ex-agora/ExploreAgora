using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreSettingsHandler
{
    ScoreSettingsType ScoreSettings { get; set; }

    int CalculateScore();
}
