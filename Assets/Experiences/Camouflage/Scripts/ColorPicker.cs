using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    //[SerializeField] Text uiText;
    Color c;
    Color []cc;
    Color targetColor;
    
    Texture2D tex;
    WaitForEndOfFrame frameEnd;
    [SerializeField] Image img;
    public Rect sourceRect;

    public void ShotColor(ColorChecker checker) {     
        frameEnd = new WaitForEndOfFrame();
        StartCoroutine(GetScreenshot(false, checker));
    }
    IEnumerator GetScreenshot(bool argb32, ColorChecker checker)
    {
        yield return null;
        checker.Group.alpha =0;
        yield return frameEnd;
        Rect viewRect = Camera.main.pixelRect;
        Texture2D tex = new Texture2D((int)viewRect.width, (int)viewRect.height, (argb32 ? TextureFormat.ARGB32 : TextureFormat.RGB24), false);
        sourceRect = img.rectTransform.rect;
        tex.ReadPixels(viewRect, 0, 0, false);
        tex.Apply(false);
        Debug.Log($"{tex.width }:::{tex.height}");
        var r =  img.rectTransform.position;
        //c = tex.GetPixel((int)r.x, (int)r.y);
        c = Color.black;
        cc = tex.GetPixels((int)r.x- (int)img.rectTransform.rect.width/2, (int)r.y- (int)img.rectTransform.rect.height/2, (int)img.rectTransform.rect.width, (int)img.rectTransform.rect.height);
        for (int i = 0; i < cc.Length; i++)
        {
            c += cc[i];
        }
        c /= cc.Length;
        c = c == Color.clear ? Color.black : c;
        //uiText.text = ColorUtility.ToHtmlStringRGB(c);
        //uiText.color = c;
        //imgTest.color = c;
        checker.Group.alpha = 1;
        yield return null;
        CheckColor(checker);
        Destroy(tex);
    }
    void CheckColor(ColorChecker colorChecker) {
        targetColor = colorChecker.TargetColor;
        float r = c.r - targetColor.r;
        float g = c.g - targetColor.g;
        float b = c.b - targetColor.b;
        float a = 1;
        float srgb = Mathf.Sqrt((r * r) + (b * b) + (g * g));
        float sa = Mathf.Sqrt((a) + (a) + (a));
        float sf = 1-(srgb / sa);
        colorChecker.CurrentColor = c;
        colorChecker.CheckResult(sf);
        //uiText.text = sf.ToString();
        Debug.Log(sf);
    }
}
