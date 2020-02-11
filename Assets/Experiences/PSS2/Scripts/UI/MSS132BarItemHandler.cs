using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSS132BarItemHandler : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Sprite itemActiveSprite;
    [SerializeField] Sprite itemDiactiveSprite;
    [SerializeField] float animationDuration;
    [SerializeField] int animationCount;
    float animationDurationPerOne;
    private void Start ()
    {
        animationDurationPerOne = animationDuration / animationCount;
    }
    void ActivateImage ()
    {
        itemImage.sprite = itemActiveSprite;
    }
    public void DiactivateImage ()
    {
        itemImage.sprite = itemDiactiveSprite;
    }
    public void ActivateImageAnimation ()
    {
        for ( int i = 0 ; i < animationCount ; i++ )
        {
            if ( i % 2 == 0 )
            {
                Invoke (nameof (ActivateImage) , i * animationDurationPerOne);
            }
            else
            {
                Invoke (nameof (DiactivateImage) , i * animationDurationPerOne);
            }
        }
        Invoke (nameof (ActivateImage) , animationDuration);
    }
}
