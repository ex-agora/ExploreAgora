using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace KDemo.D1.Scripts.Scripts
{
    public class KDRecapManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> recaps;
        [SerializeField] List<TMP_Text> recapsTexts;
        private void Start()
        {
            foreach (var recap in recaps)
            {
                recap.SetActive(false);
            }
        }
        public void ShowRecap()
        {
            foreach (var recap in recaps)
            {
                recap.SetActive(true);
            }
        }
        public void CloseRecap()
        {
            foreach (var recap in recaps)
            {
                recap.SetActive(false);
            }
        }
        public void UpdateRecapTexts(List<string> texts)
        {
            for (int i = 0; i < recapsTexts.Count; i++)
            {
                recapsTexts[i].text = texts[i];
            }
        }
    }
}