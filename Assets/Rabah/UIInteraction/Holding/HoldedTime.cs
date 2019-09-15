using UnityEngine;
using UnityEngine.EventSystems;


public class HoldedTime : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    public bool IsHolded { get => isHolded; set => isHolded = value; }

    float elapsedTime;
    float startTime;
    bool isHolded;
    [SerializeField] float updateRate = 0.5f;
    public void OnPointerDown(PointerEventData eventData)
    {
        
        // print("OnPointerDown " + ElapsedTime);
        if (!IsInvoking(nameof(UpdatePerUnit)))
        {
            startTime = 0;
            ElapsedTime = 0;
            IsHolded = true;
            InvokeRepeating(nameof(UpdatePerUnit), 0, updateRate);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // print("OnPointerUp " + ElapsedTime);
        startTime = 0;
        ElapsedTime = 0;
        IsHolded = false;
        if (IsInvoking(nameof(UpdatePerUnit)))
        {
            CancelInvoke(nameof(UpdatePerUnit));
        }
    }
    private void OnDisable()
    {
        if (IsInvoking(nameof(UpdatePerUnit)))
        {
            CancelInvoke(nameof(UpdatePerUnit));
        }
    }
    private void UpdatePerUnit()
    {
            ElapsedTime += updateRate;
            // time = Mathf.Round(time * 100) / 100;
       
        //Debug.Log("ElapsedTime " + ElapsedTime );
    }
}
