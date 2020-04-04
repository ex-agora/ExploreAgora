using SIS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdultVerification : MonoBehaviour
{
    [SerializeField] InputField ansInput;
    [SerializeField] ToolBarHandler popup;
    [SerializeField] Text eqText;
    IAPItem item;
    int ans;
    public void ShowPopup(IAPItem _item) {
        item = _item;
        eqText.text = CreateEq();
        popup.OpenToolBar();
    }
    string CreateEq() {
        string eq = string.Empty;
        int x = Random.Range(1, 9);
        int y = Random.Range(1, 9);
        int z = Random.Range(1, 9);
        eq = $"({x} * {y}) + {z} = ";
        ans = (x * y) + z;
        return eq;
    }
    public void CheckAns() {
        if (ans.ToString() == ansInput.text)
        {
            popup.CloseToolBar();
            item.Purchase();
        }
        else {
            
            eqText.text = CreateEq();
        }
        ansInput.text = string.Empty;
    }
}
