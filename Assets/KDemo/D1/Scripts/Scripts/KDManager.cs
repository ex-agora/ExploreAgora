using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KDemo.D1.Scripts.Scripts
{
    public class KDManager : MonoBehaviour
    {
        [SerializeField] PathsHandler pathsHandler;
        [SerializeField] Button doneBtn;
        public PathsHandler PathsHandler { get => pathsHandler; set => pathsHandler = value; }
        public static KDManager Instance { get; set; }
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            EnableDoneBtn(false);
        }
        public void StartExperience()
        {
            PathsHandler.StartSimulation();
        }
        public void ToggleMute()
        {
            AudioManager.Instance.ToggleMute("Background");
        }
        public void Done()
        {
            KDPrefabManager.Instance.KDRecapManager.ShowRecap();
            PathsHandler.StopSimulation();
        }
        public void FinishExperience()
        {
            EnableDoneBtn(true);
        }
        public void EnableDoneBtn(bool isEnabled)
        {
            doneBtn.interactable = isEnabled;
        }
    }
}