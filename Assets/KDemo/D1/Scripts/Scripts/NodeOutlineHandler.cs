using System;
using UnityEngine;
using UnityEngine.UI;

namespace KDemo.D1.Scripts.Scripts
{
    public class NodeOutlineHandler : MonoBehaviour
    {
        [SerializeField] private bool isActive;
        [SerializeField] private bool isLastState;
        private void OnTriggerEnter(Collider other)
        {
            if (isLastState) KDManager.Instance.FinishExperience();
            if (!isActive) 
                return;
            var outline = other.GetComponent<OutlineHandler>();
            if (outline)
            {
                outline.ShowOutline();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
            var outline = other.GetComponent<OutlineHandler>();
            if (outline)
            {
                outline.HideOutline();
            }
        }
    }
}