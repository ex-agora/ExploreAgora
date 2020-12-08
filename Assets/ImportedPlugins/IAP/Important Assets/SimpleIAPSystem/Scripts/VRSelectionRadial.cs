using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SIS
{
    /// <summary>
    /// This class is used to control a radial bar that fills up as the user holds down the Fire1 button.
    /// When it has finished filling it triggers an event.  It also has a coroutine which returns once the bar is filled.
    /// </summary>
    public class VRSelectionRadial : MonoBehaviour
    {
        public float m_SelectionDuration = 2f;               // How long it takes for the bar to fill.
        public bool m_HideOnStart = true;                    // Whether or not the bar should be visible at the start.
        public Image m_Selection;                            // Reference to the image who's fill amount is adjusted to display the bar.
        public VRInput m_VRInput;                            // Reference to the VRInput so that input events can be subscribed to.
        public VREyeRaycaster m_VREyeRaycaster;

		private Coroutine m_SelectionFillRoutine;            // Used to start and stop the filling coroutine based on input.


        private void OnEnable()
        {
            m_VRInput.OnDown += HandleDown;
            m_VRInput.OnUp += HandleUp;
        }


        private void OnDisable()
        {
            m_VRInput.OnDown -= HandleDown;
            m_VRInput.OnUp -= HandleUp;
        }


        private void Start()
        {
            // Setup the radial to have no fill at the start and hide if necessary.
            m_Selection.fillAmount = 0f;

            if(m_HideOnStart)
                Hide();
        }


        /// <summary>
        /// Show the selection radial when interaction is needed.
        /// </summary>
        public void Show()
        {
            if (m_Selection.IsActive())
                return;
            
            m_Selection.gameObject.SetActive(true);
        }


        /// <summary>
        /// Hide the selection radial when leaving an UI element that can be interacted with.
        /// </summary>
        public void Hide()
        {
			if (!m_Selection.IsActive())
				return;

            HandleUp();
            m_Selection.gameObject.SetActive(false);

            // This effectively resets the radial for when it's shown again.
            //m_Selection.fillAmount = 0f;            
        }


        private IEnumerator FillSelectionRadial()
        {
            // Create a timer and reset the fill amount.
            float timer = 0f;
            m_Selection.fillAmount = 0f;
            
            // This loop is executed once per frame until the timer exceeds the duration.
            while (timer < m_SelectionDuration)
            {
                // The image's fill amount requires a value from 0 to 1 so we normalise the time.
                m_Selection.fillAmount = timer / m_SelectionDuration;

                // Increase the timer by the time between frames and wait for the next frame.
                timer += Time.deltaTime;
                yield return null;
            }

            // When the loop is finished set the fill amount to be full.
            m_Selection.fillAmount = 1f;

            // If there is anything subscribed to OnSelectionComplete call it.
            if(m_VREyeRaycaster.CurrentInteractible != null)
            {
                m_VREyeRaycaster.CurrentInteractible.OnSelectionComplete.Invoke();
                HandleUp();
            }
        }


        private void HandleDown()
        {
            // If the radial is active start filling it.
            if (m_Selection.IsActive())
            {
                m_SelectionFillRoutine = StartCoroutine(FillSelectionRadial());
            }
        }


        private void HandleUp()
        {
            // If the radial is active stop filling it and reset it's amount.
            if (m_Selection.IsActive())
            {
                if(m_SelectionFillRoutine != null)
                    StopCoroutine(m_SelectionFillRoutine);

                m_Selection.fillAmount = 0f;
            }
        }
    }
}