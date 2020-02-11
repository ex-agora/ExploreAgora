using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    #region Fields
    public static Zoom instance;
    public float ang;
    public float dis, oldDis;
    public Transform g1, g2;
    public bool minus;
    Vector2 firstPos;
    Vector2 secondPos;
    #endregion Fields

    #region Methods
    private void Awake()
    {
        instance = this;
        g2.hasChanged = false;
    }
    void Update()
    {

        var fingerCount = 0;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                fingerCount++;
            }
            if (fingerCount >= 2)
            {
                firstPos = Input.touches[0].position;
                secondPos = Input.touches[Input.touches.Length - 1].position;
                var ProductPos1 = (secondPos - firstPos).normalized;
                ang = Mathf.Atan2(ProductPos1.y, ProductPos1.x) * Mathf.Rad2Deg;
                dis = Vector2.Distance(firstPos, secondPos);
                if (oldDis > dis)
                {
                    minus = true;
                }
                else
                {
                    minus = false;
                }
                oldDis = dis;
            }
            else if (fingerCount < 2)
            {
                ang = 0;
                dis = 0;
                oldDis = 0;
            }

            //     if (g2.hasChanged || g1.hasChanged)
            //     {
            //         firstPos = g1.position;
            //         secondPos = g2.position;
            //         var ProductPos1 = (secondPos - firstPos).normalized;
            //         ang = Mathf.Atan2(ProductPos1.y, ProductPos1.x) * Mathf.Rad2Deg;
            //         dis = Vector2.Distance(firstPos, secondPos);
            //         Debug.LogWarning(dis);
            //         if (oldDis > dis)
            //         {
            //             minus = true;
            //         }
            //         else
            //         {
            //             minus = false;
            //         }
            //         oldDis = dis;
            //     }
            //     else
            //     {
            //         ang = 0;
            //         dis = 0;
            //     }
            //     g2.hasChanged = false;
            //     g1.hasChanged = false;
            //     print(g2.hasChanged);
            // }
        }
    }
    #endregion Methods
}