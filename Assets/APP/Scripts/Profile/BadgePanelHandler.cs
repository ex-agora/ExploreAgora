using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgePanelHandler : MonoBehaviour
{
    [SerializeField] private bool isOneBadge;

    [SerializeField] private Image blueBadge;
    [SerializeField] private Image purpleBadge;
    [SerializeField] private Image goldBadge;

    [SerializeField] private Text scoreText;

    [SerializeField] AchievementHolder achievement;
    public int Current { get => achievement.current; set { achievement.current = value; UpdateCurrentPanel(); } }
    private void OnEnable()
    {
        UpdateCurrentPanel();
    }
    private void UpdateCurrentPanel()
    {
        if (isOneBadge)
        {
            if (achievement.current > 0)
                blueBadge.sprite = achievement.blueSprite;
            return;
        }
        if (achievement.current < achievement.purpleRange && achievement.current >= achievement.blueRange)
        {
            blueBadge.sprite = achievement.blueSprite;
            UpdateScoreText(achievement.purpleRange);
        }
        else if (achievement.current < achievement.goldRange && achievement.current >= achievement.purpleRange)
        {
            blueBadge.sprite = achievement.blueSprite;
            purpleBadge.sprite = achievement.purpleSprite;
            UpdateScoreText(achievement.goldRange);
        }
        else if (achievement.current >= achievement.goldRange)
        {
            blueBadge.sprite = achievement.blueSprite;
            purpleBadge.sprite = achievement.purpleSprite;
            goldBadge.sprite = achievement.goldSprite;

            UpdateScoreText(achievement.goldRange);
        }
        else { UpdateScoreText(achievement.blueRange); }
    }

    private void UpdateScoreText(int _LimitScore)
    {
        scoreText.text = $"{achievement.current}/{_LimitScore}";
    }
}