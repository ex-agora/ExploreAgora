using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KDemo.D1.Scripts.Scripts
{
    public class KDRecapHandler : MonoBehaviour
    {
        private string[] textLines;
        [SerializeField] private string textLineRecap;
        [SerializeField] private string finalText;
        [SerializeField] private Text text;
        private bool isOpen = false;
        public void OpenRecap(bool isLineByLine, float timeLapse)
        {
            if(isOpen)
                return;
            isOpen = true;
            gameObject.SetActive(true);
            if (isLineByLine)
                ShowTextLineByLine(timeLapse);
            else
                ShowText();
        }
        public void CloseRecap()
        {
            if(!isOpen)
                return;
            isOpen = false;
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
        public void ShowTextLineByLine(float timeLapse)
        {
            textLines = textLineRecap.Split('#');
            text.text = string.Empty;
            StartCoroutine(BuildText(timeLapse));
        }

        public void ShowText()
        {
            text.text = string.Empty;
            text.text = finalText;
        }

        private IEnumerator BuildText(float timeLapse)
        {
            yield return new WaitForEndOfFrame();
            foreach (var t in textLines)
            {
                text.text = string.Concat(text.text, t);
                text.text = string.Concat(text.text,"\n");
                yield return new WaitForSeconds(timeLapse);
                
            }
        }
    }
}