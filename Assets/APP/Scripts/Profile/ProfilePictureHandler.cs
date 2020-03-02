using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePictureHandler : MonoBehaviour
{
    [SerializeField] private Image[] frames;
    [SerializeField] private Sprite[] profilePictures;

    private int frameIndex = 0;
    public void TurnFrameOn(int _frame)
    {
        for (int i = 0; i < frames.Length; i++)
        {
            frames[i].enabled = false;
        }
        frames[_frame].enabled = true;
        frameIndex = _frame;

    }

    public void SetActiveFrameIndex(int _index = -1)
    {
        frameIndex = _index == -1 ? 0 : _index;
        TurnFrameOn(frameIndex);
    }

    public int ChangeProfilePicture()
    {
        return frameIndex;
    }

    public Sprite GetProfileSprite(int _index = -1)
    {
        frameIndex = _index == -1 ? 0 : _index;
        return profilePictures[frameIndex];
    }
}


