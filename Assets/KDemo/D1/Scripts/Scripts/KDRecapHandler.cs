using System;
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
        [SerializeField] private TMP_Text text;
        private bool isOpen = false;
        bool isOut;

        public bool IsOut { get => isOut; set => isOut = value; }
        public void OpenRecap(bool isLineByLine, float timeLapse,float daley = 0.0f,string str = "")
        {
            if(isOpen)
                return;
            isOpen = true;
            gameObject.SetActive(true);
            if (isLineByLine)
                ShowTextLineByLine(timeLapse, daley, true);
            else
                ShowText(str);
        }

        public void OpenRecapInstruction(bool isLineByLine, float timeLapse, float daley = 0.0f, string str = "",Text textIns = null)
        {
            if (isOpen)
                return;
            isOpen = true;
            isOut = false;
            gameObject.SetActive(true);
            if (isLineByLine)
                ShowTextLineByLine(timeLapse, daley,true,textIns);
            else
                ShowText(str);
        }
        public void CloseRecap()
        {
            if(!isOpen)
                return;
            isOpen = false;
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
        public void ShowTextLineByLine(float timeLapse, float daley = 0.0f, bool isIns = false, Text textIns = null)
        {
            textLines = textLineRecap.Split('#');
            text.text = string.Empty;
            if (isIns)
                StartCoroutine(BuildTextIns(timeLapse, daley, textIns));
            else
                StartCoroutine(BuildText(timeLapse, daley));
        }
        
        public void ShowText(string str = "")
        {
            text.text = string.Empty;
            text.text = str == string.Empty ? finalText : str;
            isOut = true;

        }
        private void Start()
        {
            isOut = true;
        }
        private IEnumerator BuildText(float timeLapse, float daley = 0.0f)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(daley);
            foreach (var t in textLines)
            {
                text.text = string.Concat(text.text, t);
                text.text = string.Concat(text.text,"\n");
                yield return new WaitForSeconds(timeLapse);
                
            }
        }

        private IEnumerator BuildTextIns(float timeLapse, float daley = 0.0f, Text textIns = null)
        {
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(daley);
            var counter = 0;
            textIns.text = string.Empty;
            foreach (var t in textLines)
            {
                textIns.text = string.Concat(textIns.text, t);
                textIns.text = string.Concat(textIns.text, "\n");
                counter++;
                if (counter % 3 == 0) {
                    yield return new WaitForSeconds(timeLapse);
                    textIns.text = string.Empty;
                }

            }
            isOut = true;
        }
    }
}