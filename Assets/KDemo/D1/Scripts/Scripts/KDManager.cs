using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KDemo.D1.Scripts.Scripts
{
    public class KDManager : MonoBehaviour
    {
        [SerializeField] PathsHandler pathsHandler;
        [SerializeField] Button doneBtn;
        [SerializeField] Button checkBtn;
        [SerializeField] private Text instrucationTxt;
        [SerializeField] private Text instrucationDownTxt;
        [SerializeField] private Image playBtn;
        [SerializeField] private Image muteBtn;
        [SerializeField] private Sprite[] playSp;
        [SerializeField] private Sprite[] muteSp;
        [SerializeField] private Text instruction;
        [SerializeField] private Text resText;
        [SerializeField] private Animator flashAnim;
        [SerializeField] private List<WordPlacingHandler> wordPlaces;
        [SerializeField] private Animator panelT;
        private int indexMute = 0;
        private int placedAns = 0;
        private bool isStarted;
       public static KDManager Instance { get; set; }
        public Text InstrucationDownTxt { get => instrucationDownTxt; set => instrucationDownTxt = value; }

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
        public void OpenPanel() { panelT.SetBool("IsOpen", true); }
        public void ToggelPanel() { panelT.SetBool("IsOpen", !panelT.GetBool("IsOpen")); }
        public void ReloadLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        public void Exit() { Application.Quit(); }
        public void AllAnswerPlaced()
        {
            placedAns++;
            checkBtn.gameObject.SetActive(placedAns == wordPlaces.Count);
        }

        public void StartExperience()
        {
            //instruction.text = string.Empty;
            if (isStarted)
            {
                playBtn.sprite = playSp[0];
                KDPrefabManager.Instance.StopSim();
                instrucationTxt.text = "Play to see the molecules in action";
            }
            else
            {
                playBtn.sprite = playSp[1];
                KDPrefabManager.Instance.StartSim();
                instrucationTxt.text = "Pause the molecules";
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
            KDPrefabManager.Instance.HideModel();
        }
        public void FinishExperience()
        {
            EnableDoneBtn(true);
        }

        private void EnableDoneBtn(bool isEnabled)
        {
            doneBtn.gameObject.SetActive(isEnabled);
        }

        public void GetScore()
        {
            var counter = wordPlaces.Sum(word => word.CheckAnswer() ? 1 : 0);
            resText.text = $"{counter} / {wordPlaces.Count}";
        }
    }
}