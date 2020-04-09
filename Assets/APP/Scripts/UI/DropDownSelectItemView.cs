using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownSelectItemView : MonoBehaviour
{
    [SerializeField] Dropdown selectedDD;
     RectTransform myRect;
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        HandleSelecetd();
    }
    void HandleSelecetd() {
        RectTransform _selcetedOption = transform.GetChild(selectedDD.value + 1).GetComponent<RectTransform>();
        float newYPos = myRect.rect.height - (_selcetedOption.anchoredPosition.y + _selcetedOption.rect.height);
        myRect.localPosition = new Vector3(myRect.localPosition.x, newYPos, myRect.localPosition.z);
    }
}
