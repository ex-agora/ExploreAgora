using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreSettingsHandler
{
    #region Properties
    ScoreSettingsType ScoreSettings { get; set; }
    #endregion Properties

    #region Methods
    int CalculateScore();
    #endregion Methods
}
