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

        bool[] isDone;
        int BufferIndex;
        private void Start()
        {
            
            BufferIndex = 0;
            isDone = new bool[recapsTexts.Count];
            isDone[BufferIndex] = true;
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

        public void StartTemp(int from, int to, float daley = 0.1f)
        {
           // temp.text = string.Empty;
            StartCoroutine(ShowNumber(from, to, daley));
        }

        public void StopTemp()
        {
            temp.text = string.Empty;
            StopAllCoroutines();
        }

        private IEnumerator ShowNumber(int from, int to, float daley)
        {
            yield return new WaitForEndOfFrame();
            for (var i = from; i <= to; i++)
            {
                temp.text = i.ToString();
                yield return  new WaitForSeconds(daley);
            }
        }

        public IEnumerator HandleRecap(int index,bool isLineByLine,float timeLapse, float daley = 0.0f)
        {
            if(index == -1)
                yield break;
            KDManager.Instance.OpenPanel();
            bool isUp = true;
            for (int j = 0; j < recapsTexts.Count; j++)
            {
                isUp &= recapsTexts[j].IsOut;
            }
            yield return isUp && isDone[index];
            
            for (var i = 0; i < recapsTexts.Count; i++)
            {
                if(index == i)
                    recapsTexts[i].OpenRecapInstruction(isLineByLine,timeLapse,daley,"", KDManager.Instance.InstrucationDownTxt);
                else
                    recapsTexts[i].CloseRecap();
            }
            for (int j = 0; j < recapsTexts.Count; j++)
            {
                isUp &= recapsTexts[j].IsOut;
            }
            yield return isUp && isDone[index];
            index++;
        }
        
    }
}