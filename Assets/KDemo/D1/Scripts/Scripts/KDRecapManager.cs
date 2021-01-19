using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace KDemo.D1.Scripts.Scripts
{
    public class KDRecapManager : MonoBehaviour
    {
       
        [SerializeField] List<KDRecapHandler> recapsTexts;
        [SerializeField] private Text temp;
        private void Start()
        {
            foreach (var recap in recapsTexts)
            {
                recap.CloseRecap();
            }
        }
        public void ShowRecap()
        {
            foreach (var recap in recapsTexts)
            {
                recap.OpenRecap(false,0);
            }
        }
        public void CloseRecap()
        {
            foreach (var recap in recapsTexts)
            {
                recap.CloseRecap();
            }
        }

        public void StartTemp()
        {
            temp.text = string.Empty;
            StartCoroutine(ShowNumber());
        }

        public void StopTemp()
        {
            temp.text = string.Empty;
            StopAllCoroutines();
        }

        IEnumerator ShowNumber()
        {
            yield return new WaitForEndOfFrame();
            for (var i = 0; i <= 100; i++)
            {
                temp.text = i.ToString();
                yield return  new WaitForSeconds(0.1f);
            }
        }

        public void HandleRecap(int index,bool isLineByLine,float timeLapse )
        {
            for (var i = 0; i < recapsTexts.Count; i++)
            {
                if(index == i)
                    recapsTexts[i].OpenRecap(isLineByLine,timeLapse);
                else
                    recapsTexts[i].CloseRecap();
            }
        }
        
    }
}