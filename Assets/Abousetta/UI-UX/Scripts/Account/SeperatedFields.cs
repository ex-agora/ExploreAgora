using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeperatedFields : MonoBehaviour
{
    [SerializeField] private List<InputField> inputFields;

    private int index = 0;
    private AxisEventData direction;


    private void Start()
    {
        direction = new AxisEventData(EventSystem.current);
        direction.moveDir = MoveDirection.None;
    }

    public void NextField()
    {

        if (inputFields[index].text == string.Empty)
        {
            direction.moveDir = MoveDirection.Left;
            ExecuteEvents.Execute(inputFields[index--].gameObject, direction, ExecuteEvents.moveHandler);
            if (index < 0)
                index = 0;
        }
        else
        {
            direction.moveDir = MoveDirection.Right;
            ExecuteEvents.Execute(inputFields[index++].gameObject, direction, ExecuteEvents.moveHandler);
            if (index >= inputFields.Count)
                index = inputFields.Count - 1;
        }
    }
}