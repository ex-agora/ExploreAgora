using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleTap : MonoBehaviour, IPointerClickHandler
{
    #region Private Variables
    private bool doubleTapped = false;
    private float tapSpeed = 0.5f;
    private float lastTapTime = 0;
    private float tapCounter = 0;
    #endregion

    #region Methods
    /// <summary>
    /// Calculate tapping numbers and time between taps.
    /// </summary>
    [SerializeField] GameEvent @event;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!doubleTapped)
        {
            if ((Time.time - lastTapTime) < tapSpeed)
            {
                tapCounter++;
                if (tapCounter == 1)
                {
                    doubleTapped = true;
                }
            }
            lastTapTime = Time.time;
        }
        if (doubleTapped)
        {
            tapCounter = 0;
            lastTapTime = 0;
            @event.Raise();
            doubleTapped = false;
        }
    }
    #endregion
}
