using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DragToWorld : MonoBehaviour
{
    //starting position of the UI Element to be dragged
	Vector2 pos;
    private RaycastHit hit;
    Ray ray;
    // check if object dragged into the right place
    bool rightPlace;
    // position to final place that we want to drag to  
    Vector3 fellowPos;
    // type of dragged object either an ui element or 3dObject
    [SerializeField] PlacingType placingType;
    // world space image that will appear if dragging of UI element completed successfully
    public GameObject image;
    // world space Model that will appear if dragging of UI element completed successfully
    public GameObject objectToBePlaced;


    private void Start()
    {
        //set starting pos of UI Element
        pos = transform.position;
    }

    //wihin dragging
    public void OnDrag ()
	{
        // UI element image position changes according to current finger position on screen (mousePosition)
        transform.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if current finger position hits the dedicated target 
        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider.name == "Cube" && hit.collider.gameObject.GetComponent<triggerDetector>().placingType == placingType)
                rightPlace = true;
            else
                rightPlace = false;
        }
    }
    
	public void BeginDrag ()
	{
        
	}

    // when release the screen 
	public void Deselect ()
	{
        //if this is the right place activate world space components else turn back ui element to its starting pos
		if (rightPlace)
		{
            if (placingType == PlacingType.UI)
            {
                image.SetActive(true);
            }
            else
            {
                Instantiate(objectToBePlaced, objectToBePlaced.transform.position, Quaternion.identity);
            }
            this.gameObject.SetActive(false);
        }
		else
		{
            StartCoroutine(MoveToPosition( transform.position, pos, 0.5f));
		}
	}

    // return ui element back to its starting pos smoothly
    IEnumerator MoveToPosition(Vector3 current, Vector3 original, float duration)
    {
        
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(current, original, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}