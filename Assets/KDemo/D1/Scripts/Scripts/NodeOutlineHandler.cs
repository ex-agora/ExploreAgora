using System;
using UnityEngine;

namespace KDemo.D1.Scripts.Scripts
{
    public class NodeOutlineHandler : MonoBehaviour
    {
        [SerializeField] private bool isActive;

        private void OnTriggerEnter(Collider other)
        {
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