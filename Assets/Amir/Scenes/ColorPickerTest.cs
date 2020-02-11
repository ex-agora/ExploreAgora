using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerTest : MonoBehaviour
{
    #region Fields
    Color c;
    Color[] cc;
    WaitForEndOfFrame frameEnd;
    [SerializeField] Rect img;
    [SerializeField] Color q;
    [SerializeField] Color q1;
    [SerializeField] Color targetColor;
    Texture2D tex;
    [SerializeField] Text uiText;
    #endregion Fields

    #region Methods
    public void ShotColor()
    {


        frameEnd = new WaitForEndOfFrame();
        StartCoroutine(GetScreenshot(false));
    }

    void CheckColor()
    {
        float h, s, b;
        Color.RGBToHSV(c, out h, out s, out b);
        Debug.Log($"{h}::{s}::{b}");
    }

    IEnumerator GetScreenshot(bool argb32)
    {
        yield return frameEnd;
        Rect viewRect = Camera.main.pixelRect;
        Texture2D tex = new Texture2D((int)viewRect.width, (int)viewRect.height, (argb32 ? TextureFormat.ARGB32 : TextureFormat.RGB24), false);
        tex.ReadPixels(viewRect, 0, 0, false);
        tex.Apply(false);
        c = tex.GetPixel(tex.width / 2, tex.height / 2);
        c = Color.black;
        cc = tex.GetPixels((int)img.x, (int)img.y, (int)img.width, (int)img.height);
        for (int i = 0; i < cc.Length; i++)
        {
            c += cc[i];
        }
        c /= cc.Length;
        uiText.text = ColorUtility.ToHtmlStringRGB(c);
        uiText.color = c;
        CheckColor();
        Destroy(tex);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion Methods
}
