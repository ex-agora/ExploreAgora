using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SIS
{
    /// <summary>
    /// This class should be added to any gameobject in the scene that should react to input based on the user's gaze.
    /// It contains events that can be subscribed to by classes that need to know about input specifics to this gameobject.
    /// </summary>
    public class VRInteractiveItem : MonoBehaviour
    {
        public UnityEvent OnOver;             // Called when the gaze moves over this object
        public UnityEvent OnOut;              // Called when the gaze leaves this object
        public UnityEvent OnClick;            // Called when click input is detected whilst the gaze is over this object.
        public UnityEvent OnDoubleClick;      // Called when double click input is detected whilst the gaze is over this object.
        public UnityEvent OnSelectionComplete;// Called when the selection radial is over thus confirming the selection.
        public UnityEvent OnUp;               // Called when Fire1 is released whilst the gaze is over this object.
        public UnityEvent OnDown;             // Called when Fire1 is pressed whilst the gaze is over this object.
        public Vector2Event OnSwipe;          // Called per swipe event with the direction as a Vector2 representation.
        protected bool m_IsOver;


        // Is the gaze currently over this object?
        public bool IsOver
        {
            get { return m_IsOver; }              
        }


        // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        // They in turn call the appropriate events should they have subscribers.
        public void Over()
        {
            m_IsOver = true;

            OnOver.Invoke();
        }


        public void Out()
        {
            m_IsOver = false;

            OnOut.Invoke();
        }


        public void Click()
        {
            OnClick.Invoke();
        }


        public void DoubleClick()
        {
            OnDoubleClick.Invoke();
        }


        public void Up()
        {
            OnUp.Invoke();
        }


        public void Down()
        {
            OnDown.Invoke();
        }


        public void Swipe(Vector2 dir)
        {
            OnSwipe.Invoke(dir);
        }


        public void InvokeSelectable()
        {
            Selectable selectable = GetComponent<Selectable>();
            if (selectable is Button)
                (selectable as Button).onClick.Invoke();
            else if (selectable is Toggle)
                (selectable as Toggle).onValueChanged.Invoke(true);            
        }


		[System.Serializable]
		public class Vector2Event : UnityEvent<Vector2>
		{
		}
    }
}