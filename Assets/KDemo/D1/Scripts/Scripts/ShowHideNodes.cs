using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideNodes : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<string> vs;
    [SerializeField] Image img;
    [SerializeField] Text txt;
    [SerializeField] List<EventRaiser> raisers;
    int index;
    void Start()
    {
        index = 0;
    }

    public void Click() {
        index = (index + 1) % 2;
        img.sprite = sprites[index];
        txt.text = vs[index];
        raisers[index].Fire();
    }


}
