using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingPossibility : MonoBehaviour
{
    Vector3 myOriginalPos;
    GameObject myReleventObj;

    public Vector3 MyOriginalPos { get => myOriginalPos; set => myOriginalPos = value; }

    bool trueAnswer;
    [SerializeField] GameEvent TrueAnswer;
    private void Awake()
    {
        myOriginalPos = this.transform.localPosition;
    }


    public void UpdateAnswers()
    {
        switch (SandwichComponentsHandler.Instance.SandwichStages)
        {
            case ESandwichStages.Bread:
                SandwichComponentsHandler.Instance.PlayerAnswer[0] = this.GetComponent<ComponentsData>().SandwichComponents.ToString();
                break;
            case ESandwichStages.Cheese:
                SandwichComponentsHandler.Instance.PlayerAnswer[1] = this.GetComponent<ComponentsData>().SandwichComponents.ToString();
                break;
            case ESandwichStages.Burger:
                SandwichComponentsHandler.Instance.PlayerAnswer[2] = this.GetComponent<ComponentsData>().SandwichComponents.ToString();
                break;
            case ESandwichStages.Extras:
                if (SandwichComponentsHandler.Instance.PlayerAnswer[3] == "")
                    SandwichComponentsHandler.Instance.PlayerAnswer[3] = this.GetComponent<ComponentsData>().SandwichComponents.ToString();
                break;
        }
        myReleventObj = this.GetComponent<ComponentsData>().MyReleventComponent;
        if (SandwichComponentsHandler.Instance.LastComponent.Count != 4)
            SandwichComponentsHandler.Instance.LastComponent.Add(myReleventObj);
        myReleventObj.SetActive(true);
        myReleventObj.GetComponent<FadeInOut>()?.SetFadeAmount(1);
        SandwichComponentsHandler.Instance.ChangeSandwichStagesInHandler(1);
        SandwichComponentsHandler.Instance.EnableDisableCheckButton();

        Debug.Log("Update");
    }


    public void ReturnToOrignalPos()
    {
        this.transform.localPosition = myOriginalPos;
    }

    public void OnDeselect()
    {
        ReturnToOrignalPos();

        if (trueAnswer)
        {
            TrueAnswer?.Raise();
           
            var fadeOutList = gameObject.GetComponentsInChildren<FadeInOut>();
            for (int i = 0; i < fadeOutList.Length; i++)
            {

                fadeOutList[i].StopAllCoroutines();
                fadeOutList[i].SetFadeAmount(0);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);

        if (other.name == "correct Place")
            trueAnswer = true;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "correct Place")
            trueAnswer = false;
    }

}

