using System;
using UnityEngine;

namespace SIS
{
    /// <summary>
    /// In order to interact with objects in the scene this class casts a ray into the scene and if it finds
    /// a VRInteractiveItem it exposes it for other classes to use. This script should be generally be placed on the camera.
    /// </summary>
    public class VREyeRaycaster : MonoBehaviour
    {
        public event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

        public Canvas m_Canvas;                       // Reference to the canvas containing the UI.
        public Transform m_Camera;                    // Reference to the camera transform casting the ray from.
        public LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
        public VRReticle m_Reticle;                   // The reticle, if applicable.
        public VRInput m_VrInput;                     // Used to call input based events on the current VRInteractiveItem.
        public VRSelectionRadial m_SelectionRadial;   // The radial selection allowing for user confirmation.
        public bool m_ShowDebugRay;                   // Optionally show the debug ray.
        public float m_DebugRayLength = 5f;           // Debug ray length.
        public float m_DebugRayDuration = 1f;         // How long the Debug ray will remain visible.
        public float m_RayLength = 500f;              // How far into the scene the ray is cast.
        
        private VRInteractiveItem m_CurrentInteractible;                //The current interactive item
        private VRInteractiveItem m_LastInteractible;                   //The last interactive item


        /// <summary>
        /// Utility for other classes to get the current interactive item.
        /// </summary>
        public VRInteractiveItem CurrentInteractible
        {
            get { return m_CurrentInteractible; }
        }


        // This ensures that the UI (such as the reticle and selection bar) are set up correctly.
        private void Awake()
        {
            // Make sure the canvas is on.
            m_Canvas.enabled = true;

            // Set its sorting order to the front.
            m_Canvas.sortingOrder = Int16.MaxValue;

            // Force the canvas to redraw so that it is correct before the first render.
            Canvas.ForceUpdateCanvases();
        }


        private void OnEnable()
        {
            m_VrInput.OnClick += HandleClick;
            m_VrInput.OnDoubleClick += HandleDoubleClick;
            m_VrInput.OnUp += HandleUp;
            m_VrInput.OnDown += HandleDown;
            m_VrInput.OnSwipe += HandleSwipe;
        }


        private void OnDisable ()
        {
            m_VrInput.OnClick -= HandleClick;
            m_VrInput.OnDoubleClick -= HandleDoubleClick;
            m_VrInput.OnUp -= HandleUp;
            m_VrInput.OnDown -= HandleDown;
            m_VrInput.OnSwipe -= HandleSwipe;
        }


        private void Update()
        {
            EyeRaycast();
        }

      
        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (m_ShowDebugRay)
            {
                Debug.DrawRay(m_Camera.position, m_Camera.forward * m_DebugRayLength, Color.blue, m_DebugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(m_Camera.position, m_Camera.forward);
            RaycastHit hit;
            
            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {
                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                m_CurrentInteractible = interactible;

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                {
                    interactible.Over();

                    //only show the selection radial if there actually is an event associated to it
                    if(interactible.OnSelectionComplete.GetPersistentEventCount() > 0)
                        m_SelectionRadial.Show();
				}

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactiveLastInteractible();

                m_LastInteractible = interactible;

                // Something was hit, set at the hit position.
                if (m_Reticle)
                    m_Reticle.SetPosition(hit);

                if (OnRaycasthit != null)
                    OnRaycasthit(hit);
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                if (m_Reticle)
                    m_Reticle.SetPosition();
            }
        }


        private void DeactiveLastInteractible()
        {
            if (m_LastInteractible == null)
                return;

            m_LastInteractible.Out();
            m_LastInteractible = null;
            m_SelectionRadial.Hide();
        }


        private void HandleUp()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Up();
        }


        private void HandleDown()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Down();
        }


        private void HandleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.Click();
        }


        private void HandleDoubleClick()
        {
            if (m_CurrentInteractible != null)
                m_CurrentInteractible.DoubleClick();
        }


        private void HandleSwipe(VRInput.SwipeDirection swipe)
        {
            if (m_CurrentInteractible == null || swipe == VRInput.SwipeDirection.NONE)
                return;

            switch (swipe)
            {
                case VRInput.SwipeDirection.LEFT:
                    m_CurrentInteractible.Swipe(new Vector2(-1, 0));
                    break;
                case VRInput.SwipeDirection.RIGHT:
                    m_CurrentInteractible.Swipe(new Vector2(1, 0));
                    break;
                case VRInput.SwipeDirection.UP:
                    m_CurrentInteractible.Swipe(new Vector2(0, 1));
                    break;
                case VRInput.SwipeDirection.DOWN:
                    m_CurrentInteractible.Swipe(new Vector2(0, -1));
                    break;
            }
        }
    }
}