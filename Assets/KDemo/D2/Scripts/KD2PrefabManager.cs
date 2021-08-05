using System;
using KDemo.D1.Scripts.Scripts;
using UnityEngine;

namespace KDemo.D2.Scripts
{
    public class KD2PrefabManager : MonoBehaviour
    {
        private static KD2PrefabManager instance;
        private float updateRate = 1f;
        private int timeElp;
        [SerializeField] private GameEvent @questionEvent = null;
        [Header("Init")]
        [SerializeField] private GameObject initObj;
        [SerializeField] private PathsHandler initPathsHandler;
        [SerializeField] private KDRecapHandler initRecapHandler;
        [Header("Surface")]
        [SerializeField] private GameObject surfaceObj;
        [SerializeField] private PathsHandler[] surfacePathsHandler;
        [SerializeField] private KDRecapHandler surfaceRecapHandler;
        [SerializeField] private CounterNodeHandler[] counterSurface;
        [Header("Humidity")]
        [SerializeField] private GameObject humidityObj;
        [SerializeField] private PathsHandler[] humidityPathsHandler;
        [SerializeField] private KDRecapHandler humidityRecapHandler;
        [SerializeField] private CounterNodeHandler[] counterHumidity;



        public static KD2PrefabManager Instance
        {
            get => instance;
            set => instance = value;
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            initRecapHandler.CloseRecap();
            surfaceRecapHandler.CloseRecap();
            humidityRecapHandler.CloseRecap();
        }

        private void InitSimStart()
        {
            initPathsHandler.StartSimulation();
            if (IsInvoking(nameof(InitCustomUpdate))) return;
            timeElp = 0;
            InvokeRepeating(nameof(InitCustomUpdate), 0, updateRate);
        }
        
        private void SurfaceSimStart()
        {
            for (int i = 0; i < surfacePathsHandler.Length; i++)
            {

                surfacePathsHandler[i].StartSimulation();
            }
            foreach (var counterNodeHandler in counterSurface)
            {
                counterNodeHandler.Start();
            }
            if (IsInvoking(nameof(SurfaceCustomUpdate))) return;
            timeElp = 0;
            InvokeRepeating(nameof(SurfaceCustomUpdate), 0, updateRate);
            KD2Manager.Instance.StartTimer(15f);
        }
        
        private void HumiditySimStart()
        {
            for (int i = 0; i < humidityPathsHandler.Length; i++)
            {

                humidityPathsHandler[i].StartSimulation();
            }
            foreach (var counterNodeHandler in counterHumidity)
            {
                counterNodeHandler.Start();
            }
            if (IsInvoking(nameof(HumidityCustomUpdate))) return;
            timeElp = 0;
            InvokeRepeating(nameof(HumidityCustomUpdate), 0, updateRate);
            KD2Manager.Instance.StartTimer(20f);

        }
        
        private void InitSimStop()
        {
            initPathsHandler.StopSimulation();
            initRecapHandler.CloseRecap();
            if (!IsInvoking(nameof(InitCustomUpdate))) return;
            CancelInvoke(nameof(InitCustomUpdate));
        }
        
        private void SurfaceSimStop()
        {
            for (int i = 0; i < surfacePathsHandler.Length; i++)
            {

            surfacePathsHandler[i].StopSimulation();
            }
            foreach (var counterNodeHandler in counterSurface)
            {
                counterNodeHandler.Stop();
            }
            surfaceRecapHandler.CloseRecap();
            if (!IsInvoking(nameof(SurfaceCustomUpdate))) return;
            CancelInvoke(nameof(SurfaceCustomUpdate)); 
            KD2Manager.Instance.StopTimer();
        }
        
        private void HumiditySimStop()
        {
            for (int i = 0; i < humidityPathsHandler.Length; i++)
            {

            humidityPathsHandler[i].StopSimulation();
            }
            foreach (var counterNodeHandler in counterHumidity)
            {
                counterNodeHandler.Stop();
            }
            humidityRecapHandler.CloseRecap();
            if (!IsInvoking(nameof(HumidityCustomUpdate))) return;
            CancelInvoke(nameof(HumidityCustomUpdate));
            KD2Manager.Instance.StopTimer();
        }

        private void InitCustomUpdate()
        {
            timeElp ++;
            if (timeElp == 1)
            {
                KD2Manager.Instance.OpenPanel();

                KD2Manager.Instance.InstrucationDownTxt.text = "Even at rest, the water" +
                                                  " " +
                                                  "molecules are moving";   
            }
            else if (timeElp == 5)
            {
                Debug.Log(1);
                Debug.Log($"timeElp: {timeElp}");
                Debug.Log($"timeElp Floor: {Mathf.Ceil((timeElp - 5f))}");
                //initPathsHandler.SwitchNode(1);
                initRecapHandler.CloseRecap();
                KD2Manager.Instance.OpenPanel();

                KD2Manager.Instance.InstrucationDownTxt.text = "As they move, they collide with" +
                                                     " each other and gain energy";   
            }
            else if (timeElp == 10)
            {
                initPathsHandler.SwitchNode(1);
                initRecapHandler.CloseRecap();
                KD2Manager.Instance.OpenPanel();

                KD2Manager.Instance.InstrucationDownTxt.text = "When a molecule gains enough" +
                                                     " energy, it escapes the surface into the air taking some" +
                                                     " of the liquid’s energy with it";
            }
            else if (timeElp == 15)
            {
                initPathsHandler.SwitchNode(1);
            }
            else if (timeElp == 20)
            {
                initPathsHandler.SwitchNode(1);
                DoneEnable();
                if (IsInvoking(nameof(InitCustomUpdate))) return;
                CancelInvoke(nameof(InitCustomUpdate));
                
            }
        }

        private void SurfaceCustomUpdate()
        {
            timeElp++;
            if (timeElp == 1)
            {
                KD2Manager.Instance.OpenPanel();

                KD2Manager.Instance.InstrucationDownTxt.text =  "Both Bowls have the same " +
                                                           "amount of water, but one bowl has a bigger surface area than " +
                                                           "the other";
            }
            else if (timeElp == 5)
            {
                surfacePathsHandler[0].SwitchNode(1);
                surfacePathsHandler[1].SwitchNode(2);
                surfaceRecapHandler.CloseRecap();
                KD2Manager.Instance.OpenPanel();

                KD2Manager.Instance.InstrucationDownTxt.text= "Evaporation happens in both bowls" +
                                                           " at the same time. Check the counter to see which bowl had" +
                                                           " more evaporation";
            }
            else if (timeElp == 10)
            {
                surfacePathsHandler[0].SwitchNode(1);
                surfacePathsHandler[1].SwitchNode(2);
            }
            else if (timeElp == 15)
            {
                surfacePathsHandler[0].SwitchNode(1);
                surfacePathsHandler[1].SwitchNode(2);
                DoneEnable();
                if (IsInvoking(nameof(SurfaceCustomUpdate))) return;
                CancelInvoke(nameof(SurfaceCustomUpdate));
                
            }
        }
        
        private void HumidityCustomUpdate()
        {
            timeElp++;
            if (timeElp == 1)
            {
                KD2Manager.Instance.InstrucationDownTxt.text = "Both Bowls have the same amount" +
                                                     " of water, but one bowl is locked in a glass box";   
            }
            else if (timeElp == 5)
            {
                humidityPathsHandler[0].SwitchNode(1);
                humidityPathsHandler[1].SwitchNode(1);
                humidityRecapHandler.CloseRecap();
                KD2Manager.Instance.OpenPanel();

                KD2Manager.Instance.InstrucationDownTxt.text = "Evaporation happens in both bowls" +
                                                         " at the same time. The open bowl molecules can escape away" +
                                                         " into the air. The molecules in the enclosed bowl escape" +
                                                         " but stay trapped in the box ";   
            }
            else if (timeElp == 10)
            {
                humidityPathsHandler[0].SwitchNode(3);
                humidityPathsHandler[1].SwitchNode(1);
                humidityRecapHandler.CloseRecap();
                KD2Manager.Instance.OpenPanel();

                KD2Manager.Instance.InstrucationDownTxt.text = "As more molecules stay trapped" +
                                                         " in the box, the air becomes occupied (saturated) with air" +
                                                         " molecules, and less molecules can fit it ";
            }
            else if (timeElp == 15)
            {
                humidityPathsHandler[0].SwitchNode(1);
               // humidityPathsHandler[1].SwitchNode(1);
            }
            else if (timeElp == 20)
            {
                humidityPathsHandler[0].SwitchNode(2);
                humidityPathsHandler[1].SwitchNode(1);
                DoneEnable();
                if (IsInvoking(nameof(InitCustomUpdate))) return;
                CancelInvoke(nameof(InitCustomUpdate));
                
            }
        }

        public void ShowQuestion(KD2StateExp state)
        {
            switch (state)
            {
                case KD2StateExp.None:
                    break;
                case KD2StateExp.Init:
                    break;
                case KD2StateExp.Surface:
                    surfaceRecapHandler.CloseRecap();
                    KD2Manager.Instance.OpenPanel();
                    KD2Manager.Instance.SetInstructionText("Tap on the bowl that had more evaporation");
                    questionEvent.Raise();
                    break;
                case KD2StateExp.Humidity:
                    humidityRecapHandler.CloseRecap();
                    KD2Manager.Instance.OpenPanel();

                    KD2Manager.Instance.SetInstructionText("Tap on the bowl that had more evaporation");
                    questionEvent.Raise();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void AnswerQuestion(bool isRightAnswer)
        {
            surfaceRecapHandler.CloseRecap();
            humidityRecapHandler.CloseRecap();
            KD2Manager.Instance.ValidAnswerExp(isRightAnswer);
        }

        public void StartSim(KD2StateExp state)
        {
            switch (state)
            {
                case KD2StateExp.None:
                    break;
                case KD2StateExp.Init:
                   InitSimStart(); break;
                case KD2StateExp.Surface:
                   SurfaceSimStart(); break;
                case KD2StateExp.Humidity:
                  HumiditySimStart();  break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        public void StopSim(KD2StateExp state)
        {
            switch (state)
            {
                case KD2StateExp.None:
                    break;
                case KD2StateExp.Init:
                 InitSimStop();   break;
                case KD2StateExp.Surface:
                   SurfaceSimStop(); break;
                case KD2StateExp.Humidity:
                 HumiditySimStop();   break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void DoneEnable()
        {
            KD2Manager.Instance.FinishSimulation();
        }

        public void ChnageState(KD2StateExp state)
        {
            switch (state)
            {
                case KD2StateExp.None:
                    break;
                case KD2StateExp.Init:
                    initObj.SetActive(true);
                    surfaceObj.SetActive(false);
                    humidityObj.SetActive(false);
                    break;
                case KD2StateExp.Surface:
                    initObj.SetActive(false);
                    surfaceObj.SetActive(true);
                    humidityObj.SetActive(false);
                    break;
                case KD2StateExp.Humidity:
                    initObj.SetActive(false);
                    surfaceObj.SetActive(false);
                    humidityObj.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}