using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragToWorld : MonoBehaviour
{
    [SerializeField] string targetLabelStr;
    [SerializeField] bool isPosTarget;
    [SerializeField] RectTransform posTarget;
    private RaycastHit hit;
    Ray ray;
    bool canDarg;
    // check if object dragged into the right place
    bool rightPlace;
    // position to final place that we want to drag to  
    Vector3 fellowPos;
    // type of dragged object either an ui element or 3dObject
    [SerializeField] PlacingType placingType;
    [HideInInspector] [SerializeField] bool isPlacingUI;
    [HideInInspector] [SerializeField] bool isPlacingObject;
    // world space image that will appear if dragging of UI element completed successfully
    [SerializeField] GameObject image;
    // world space Model that will appear if dragging of UI element completed successfully
    [SerializeField] GameObject objectToBePlaced;
    RectTransform rectTransform;
    LabelWorldHandler label;
    Vector3 initPos;
    private void Start()
    {
        //set starting pos of UI Element
        rectTransform = transform.GetComponent<RectTransform>();
        canDarg = true;
        initPos = rectTransform.position;
        PlantPartsGameManager.Instance.MaxQuizPart++;
    }

    public void BeginDrag() {
        AudioManager.Instance.Play("UIAction", "UI");
    }
    //wihin dragging
    public void OnDrag ()
	{
        // UI element image position changes according to current finger position on screen (mousePosition)
        if (canDarg)
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    //void ResetTransform() {
    //    rectTransform.offsetMax = Vector2.zero;
    //    rectTransform.offsetMin= Vector2.zero;
    //}
	public void CheckDrag ()
	{
       
        ray = PlantPartsGameManager.Instance.ArCamera.ScreenPointToRay(transform.position);
        //if current finger position hits the dedicated target 
        if (Physics.Raycast(ray, out hit))
        {
            label = hit.collider.gameObject.GetComponentInParent<LabelWorldHandler>();
            if (label == null) return;
            if (label.LabelTextStr == targetLabelStr)
            {
                rightPlace = true;
                PlantPartsGameManager.Instance.CorrectAnswer++;
            }
            else
            {
                rightPlace = false;
                PlantPartsGameManager.Instance.WrongTrialCount++;
            }
        }
    }

    // when release the screen 
    public void Deselect()
    {
        canDarg = false;
        CheckDrag();
        //if this is the right place activate world space components else turn back ui element to its starting pos
        if (rightPlace)
        {
            if (placingType == PlacingType.UI)
            {
                AudioManager.Instance.Play("placeObject", "Activity");
                label.RightAnswer();
                label.enabled = false;
            }
            else
            {
                Instantiate(objectToBePlaced, objectToBePlaced.transform.position, Quaternion.identity);
            }
            this.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(MoveToPosition(0.4f));
            //ResetTransform();
        }
    }

    // return ui element back to its starting pos smoothly
    IEnumerator MoveToPosition(float duration)
    {
        
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            //rectTransform.offsetMax = Vector2.Lerp(rectTransform.offsetMax, Vector2.zero , (elapsedTime / duration));
            //rectTransform.offsetMin = Vector2.Lerp(rectTransform.offsetMin, Vector2.zero, (elapsedTime / duration));
            rectTransform.position = Vector3.Lerp(rectTransform.position, isPosTarget? posTarget.position: initPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canDarg = true;
    }
    private void OnValidate()
    {
        if (placingType == PlacingType.Object)
        {
            isPlacingUI = false;
            isPlacingObject = true;
        }
        else if (placingType == PlacingType.UI)
        {
            isPlacingUI = true;
            isPlacingObject = false;
        }
    }
}