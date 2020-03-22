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

    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite purpleSprite;
    [SerializeField] private Sprite goldSprite;

    [SerializeField] private int blueRange;
    [SerializeField] private int purpleRange;
    [SerializeField] private int goldRange;

    [SerializeField] private int current;

    public int Current { get => current; set { current = value; UpdateCurrentPanel(); } }

    private void UpdateCurrentPanel()
    {
        if (isOneBadge)
        {
            if (current > 0)
                blueBadge.sprite = blueSprite;
            return;
        }
        if (current < purpleRange && current >= blueRange)
        {
            blueBadge.sprite = blueSprite;
            UpdateScoreText(purpleRange);
        }
        else if (current < goldRange && current >= purpleRange)
        {
            blueBadge.sprite = blueSprite;
            purpleBadge.sprite = purpleSprite;
            UpdateScoreText(goldRange);
        }
        else if (current >= goldRange)
        {
            blueBadge.sprite = blueSprite;
            purpleBadge.sprite = purpleSprite;
            goldBadge.sprite = goldSprite;

            UpdateScoreText(goldRange);
        }
        else { UpdateScoreText(blueRange); }
    }

    private void UpdateScoreText(int _LimitScore)
    {
        scoreText.text = $"{current}/{_LimitScore}";
    }
}