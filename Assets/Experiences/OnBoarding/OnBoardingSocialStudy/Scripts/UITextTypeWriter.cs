using System.Collections;
using UnityEngine;
using TMPro;

public class UITextTypeWriter : MonoBehaviour
{
    #region Fields
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private float duration;
    float updateRate;
    private string story;
    int index;
    float elpTime;
    #endregion Fields

    #region Methods

    private void Awake()
    {
        story = txt.text;
        txt.text = ""; 
    }
    public void PlayTextAnim() {
        index = 0;
        elpTime = 0;
        updateRate = duration / story.Length;
        InvokeRepeating(nameof(CustoumUpdate), 0, updateRate);
    }
    private void CustoumUpdate()
    {
        if (index >= story.Length) {
            txt.text = story;
            CancelInvoke(nameof(CustoumUpdate));
            return;
        }
        txt.text += story[index];
        index++;
        elpTime += updateRate;
        if (elpTime >= duration) {
            CancelInvoke(nameof(CustoumUpdate));
        }
    }

    #endregion Methods
}