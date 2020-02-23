using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwichStoriesHandler : MonoBehaviour
{
    [SerializeField] UnityEvent tocuhRightScreen;
    [SerializeField] UnityEvent tocuhLeftScreen;
    [SerializeField] UnityEvent onResumeTouch;
    [SerializeField] UnityEvent onFinishTouch;
    Touch touch;
    // Update is called once per frame
    void Update ()
    {
        if ( Input.touchCount > 0 )
        {
            touch = Input.GetTouch (0);
            if ( touch.phase == TouchPhase.Began )
            {
                if ( touch.position.x < Screen.width / 2 )
                {
                    tocuhLeftScreen.Invoke ();
                }
                else if ( touch.position.x > Screen.width / 2 )
                {
                    tocuhRightScreen.Invoke ();
                }
            }
            else if ( touch.phase == TouchPhase.Stationary )
            {
                onResumeTouch.Invoke ();
            }
            else if ( touch.phase == TouchPhase.Ended )
            {
                onFinishTouch.Invoke ();
            }
        }
    }
}
