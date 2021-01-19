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
        [SerializeField] private Image playBtn;
        [SerializeField] private Image muteBtn;
        [SerializeField] private Sprite[] playSp;
        [SerializeField] private Sprite[] muteSp;
        [SerializeField] private Text instruction;
        [SerializeField] private Animator flashAnim;
        private int indexMute = 0;
        private bool isStarted;
       public static KDManager Instance { get; set; }
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        // Start is called before the first frame update
        private void Start()
        {
            isStarted = false;
            EnableDoneBtn(false);
        }
        public void StartExperience()
        {
            instruction.text = string.Empty;
            if (isStarted)
            {
                playBtn.sprite = playSp[0];
                KDPrefabManager.Instance.StopSim();
            }
            else
            {
                playBtn.sprite = playSp[1];
                KDPrefabManager.Instance.StartSim();
            }

            isStarted = !isStarted;
        }
        public void ToggleMute()
        {
            muteBtn.sprite = muteSp[++indexMute % 2];
            KDPrefabManager.Instance.ToggleFireSound();
        }
        public void Done()
        {
            KDPrefabManager.Instance.KDRecapManager.ShowRecap();
            KDPrefabManager.Instance.StopSim();
        }
        public void FinishExperience()
        {
            EnableDoneBtn(true);
        }

        private void EnableDoneBtn(bool isEnabled)
        {
            doneBtn.interactable = isEnabled;
        }
    }
}