using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheildUIHander : MonoBehaviour
{
    [SerializeField] List<Sprite> sheildFragSprites;
    [SerializeField] List<Image> sheildFragImgs;
    int count = 0;
    public void UpdateSheildFrag() {
        if (sheildFragSprites.Count == count)
            return;
        sheildFragImgs[count].sprite = sheildFragSprites[count];
        count = Mathf.Clamp(count + 1, 0, sheildFragSprites.Count);
    }
}
