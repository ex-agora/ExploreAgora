using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace KDemo.D2.Scripts
{
    [System.Serializable]
    public enum KD2StateExp{
        None = 0,Init = 1,Surface = 2,Humidity = 3
    }

    public class KD2Manager : MonoBehaviour
    {
        private static KD2Manager instance;
        private bool isSimStarted = false;
        private int countFinishedExp = 0;
        private int correctAns;
        KD2StateExp currentState = KD2StateExp.None;
        [SerializeField] private Button doneBtn;
        [SerializeField] private Sprite[] playSp;
        [SerializeField] private GameObject instractionObj;
        [SerializeField] private Text instrucationTxt;
        [SerializeField] private Text instrucationDownTxt;
        [SerializeField] private Button playBtn;
        [SerializeField] private Text instruction;
        [SerializeField] private GameObject routePanel;
        [SerializeField] private GameObject testPanel;
        [SerializeField] private Text resTxt;
        [Header("Init")] 
        [SerializeField] private GameObject recap1;
        
        [Header("Common")] 
        [SerializeField] private Text answer;
        [SerializeField] private GameObject recapExp;
        [SerializeField] private TimerUIHandler timer;
        [SerializeField] private GameEvent @correctAnsEvt;
        [SerializeField] private GameEvent @wrongAnsEvt;
        [SerializeField] private Animator panelT;
        
        [Header("Surface")] [SerializeField] 
        private GameObject surfaceRecap;
        
        [Header("Humidity")] [SerializeField]
        private GameObject humidityRecap;
        
        public static KD2Manager Instance
        {
            get => instance;
            set => instance = value;
        }
        public Text InstrucationDownTxt { get => instrucationDownTxt; set => instrucationDownTxt = value; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        private void Start()
        {
            correctAns = 0;
        }
        public void CorrectAns() { correctAns++; }
        public void UpdateResTxt() { resTxt.text = $"{correctAns} / 2 "; }
        public void OpenPanel() { panelT.SetBool("IsOpen", true); }
        public void ToggelPanel() { panelT.SetBool("IsOpen", !panelT.GetBool("IsOpen")); }
        public void ReloadLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        public void Exit() { Application.Quit(); }
        public void SimulationHandling()
        {
            //instruction.text = string.Empty;

            if (isSimStarted)
            {
                playBtn.image.sprite = playSp[0];
                KD2PrefabManager.Instance.StopSim(currentState);
                instrucationTxt.text = "Play to see the molecules in action";
            }
            else
            {
                playBtn.image.sprite = playSp[1];
                KD2PrefabManager.Instance.StartSim(currentState);
                instrucationTxt.text = "Stop the molecules";
            }

            isSimStarted = !isSimStarted;
        }

        private void PlayBtnEnable(bool status) {
            playBtn.enabled = status;
        }

        public void ResetPInstructionPanel() {
            PlayBtnEnable(true);
            instractionObj.SetActive(true);
            instrucationDownTxt.text = string.Empty;
            instrucationTxt.text = "Play to see the molecules in action";
            OpenPanel();
        }
        public void SetInstructionText(string str) {
            PlayBtnEnable(false);
            instrucationTxt.text = str;
            OpenPanel();
        }
        public void StartTimer(float time)
        {
            timer.Duration = time;
            timer.ViewBar();
            timer.StartTimer();
        }
        public void StopTimer()
        {
            timer.HideBar();
        }

        public void FinishExp()
        {
            countFinishedExp++;
            instractionObj.SetActive(false);
            switch (countFinishedExp)
            {
                case 1:
                    routePanel.SetActive(true);
                    break;
                case 2:
                    testPanel.SetActive(true);
                    break;
            }
        }

        public void FinishSimulation()
        {
            doneBtn.gameObject.SetActive(true);
        }
        public void Done()
        {
           KD2PrefabManager.Instance.StopSim(currentState);
        }

        private void ResetState()
        {
            isSimStarted = false;
            playBtn.image.sprite = playSp[0];
        }

        public void ChangeState(int state)
        {
            ResetState();
            currentState = (KD2StateExp) state;
            KD2PrefabManager.Instance.ChnageState(currentState);
        }

        public void ValidAnswerExp(bool isRightAnswer)

        {
            answer.text = isRightAnswer ? "WELL DONE" : "Oops. Let me show you the right answer!";
            var tempEvt = isRightAnswer ? @correctAnsEvt : @wrongAnsEvt;
            tempEvt.Raise();
            recapExp.SetActive(true);
            switch (currentState)
            {
                case KD2StateExp.None:
                    break;
                case KD2StateExp.Init:
                    break;
                case KD2StateExp.Surface:
                   surfaceRecap.SetActive(true);
                   humidityRecap.SetActive(false); break;
                case KD2StateExp.Humidity:
                    surfaceRecap.SetActive(false);
                    humidityRecap.SetActive(true); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void HandleRoute()
        {
            switch (currentState)
            {
                case KD2StateExp.None:
                    break;
                case KD2StateExp.Init:
                    instractionObj.SetActive(false);
                    recap1.SetActive(true);   break;
                case KD2StateExp.Surface:
                case KD2StateExp.Humidity:
                    KD2PrefabManager.Instance.ShowQuestion(currentState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}