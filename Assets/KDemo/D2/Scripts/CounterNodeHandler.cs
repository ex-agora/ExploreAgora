using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KDemo.D2.Scripts
{
    public class CounterNodeHandler : MonoBehaviour
    {
        private int counter;
        private bool isStarted;
        [SerializeField] private TMP_Text counterTxt;

        public int Counter
        {
            get => counter;
            private set { counter = value; UpdateUI();}
        }

        public void Start()
        {
            gameObject.SetActive(true);
            isStarted = true;
            Counter = 0;
        }

        public void Stop()
        {
            gameObject.SetActive(false);
            isStarted = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if(!isStarted) return;
            var outline = other.GetComponent<OutlineHandler>();
            if (!outline) return;
            outline.ShowOutline();
            outline.HideOutline();
            Counter++;
        }

        private void UpdateUI()
        {
            counterTxt.text = counter.ToString();
        }
    }
}