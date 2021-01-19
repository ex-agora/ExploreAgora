using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KDemo.D1.Scripts.Scripts
{
    public enum Kd1State
    {
        None,S1,S2,S3,End
    }

    public class KDPrefabManager : MonoBehaviour
    {
        private Kd1State state = Kd1State.None;
        [SerializeField] private KDRecapManager manager;
        public static KDPrefabManager Instance { get; set; }
        public KDRecapManager KDRecapManager { get => kDRecapManager; set => kDRecapManager = value; }

        public PathsHandler Handler
        {
            get => pathsHandler;
            set => pathsHandler = value;
        }

        [SerializeField] private PathsHandler pathsHandler;
        [SerializeField] KDRecapManager kDRecapManager;
        [SerializeField] private ParticleSystem firePS;
        [SerializeField] private ParticleSystem steamPS;
        [SerializeField] private AudioSource fireAudio;
        [SerializeField] private AudioSource waterDropAudio;
        [SerializeField] private List<NodeOutlineHandler> nodeOutlineHandlers;
        private float updateRate = 1f;
        private float elpTime;
        private float daley = 4f;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void StartSim()
        {
            pathsHandler.StartSimulation();
            firePS.Play(true);
            steamPS.Play(true);
            fireAudio.Play(1284);
            if (IsInvoking(nameof(CustomUpdate))) return;
            elpTime = daley;
            manager.StartTemp();
            InvokeRepeating(nameof(CustomUpdate), daley, updateRate);
        }
        public void StopSim()
        {
            HandleOutline(-1);
            pathsHandler.StopSimulation();
            firePS.Stop(true);
            steamPS.Play(false);
            fireAudio.Stop();
            if (!IsInvoking(nameof(CustomUpdate))) return;
            manager.StopTemp();
            CancelInvoke(nameof(CustomUpdate));
            manager.HandleRecap(-1, false, 00.00f);
        }

        private void CustomUpdate()
        {
            elpTime += updateRate;
            switch (state)
            {
                case Kd1State.None:
                    HandleOutline(-1);
                    if (elpTime >= (daley + 1)) state = Kd1State.S1;
                    break;
                case Kd1State.S1:
                    HandleOutline(0);
                    manager.HandleRecap(0,true,2f);
                    if (elpTime >= (daley + 7)) state = Kd1State.S2;
                    break;
                case Kd1State.S2:
                    HandleOutline(1);
                    manager.HandleRecap(1,true,2f);
                    if (elpTime >= (daley + 11)) state = Kd1State.S3;
                    break;
                case Kd1State.S3:
                    HandleOutline(2);
                    manager.HandleRecap(2,true,2f);
                    if (elpTime >= (daley + 18)) {
                        state = Kd1State.None;
                        elpTime = daley;
                    }
                    break;
                case Kd1State.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }  
        }

        private void HandleOutline(int index)
        {
            for (var i = 0; i < nodeOutlineHandlers.Count; i++)
            {
                nodeOutlineHandlers[i].IsActive = index == i;
            }
        }

        public void ToggleFireSound()
        {
            fireAudio.mute = !fireAudio.mute;
            waterDropAudio.mute = !waterDropAudio.mute;
            AudioManager.Instance.AudioController(fireAudio.mute ? 3 : 0);
        }

    }
}