using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeperatedFields : MonoBehaviour
{
    [SerializeField] private List<InputField> inputFields;
    [HideInInspector] public string code;

    private int index = 0;
    private AxisEventData direction;
    int previous;
    public bool up;
    private void Start()
    {
        direction = new AxisEventData(EventSystem.current);
        direction.moveDir = MoveDirection.None;


        //    for (int i = 1; i < inputFields.Count; i++)
        //        inputFields[i].interactable = false;
        inputFields[0].Select();
        inputFields[0].ActivateInputField();
    }

    public void NextField()
    {
        if (inputFields[index].text == string.Empty)
        {
            direction.moveDir = MoveDirection.Left;
            previous = index;
            //inputFields[index < 0 ? 0 : index - 1].interactable = true;
            up = ExecuteEvents.Execute(inputFields[index--].gameObject, direction, ExecuteEvents.moveHandler);
            if (index < 0)
                index = 0;
        }
        else
        {
            direction.moveDir = MoveDirection.Right;
            previous = index;
            //inputFields[index >= inputFields.Count ? inputFields.Count - 1 : index + 1].interactable = true;
            up = ExecuteEvents.Execute(inputFields[index++].gameObject, direction, ExecuteEvents.moveHandler);
            if (index >= inputFields.Count)
                index = inputFields.Count - 1;
        }

        //inputFields[previous].interactable = false;
        inputFields[index].Select();
        inputFields[index].ActivateInputField();
    }

    public int SubmitCode()
    {
        code = string.Empty;

        for (int i = 0; i < inputFields.Count; i++)
            code += inputFields[i].text;

        return int.Parse(code);

    }
   
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            NextField();
    }
    */
    public void ClearSeperatedFields() {
        for (int i = 0; i < inputFields.Count; i++)
            inputFields[i].text = string.Empty;
        inputFields[0].Select();
        inputFields[0].ActivateInputField();
        direction = new AxisEventData(EventSystem.current);
        direction.moveDir = MoveDirection.None;
    }
    /*void LateUpdate()
    {
        if (!TouchScreenKeyboard.visible)
            TouchScreenKeyboard.Open("");
    }*/
}