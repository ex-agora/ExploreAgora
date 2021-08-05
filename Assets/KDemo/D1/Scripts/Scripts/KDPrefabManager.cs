using System;
using System.Collections;
using System.Collections.Generic;
using KDemo.D2.Scripts;
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
        [SerializeField] private GameObject waterDrop;
        [SerializeField] private AudioSource waterBoilingAudio;
        [SerializeField] private List<NodeOutlineHandler> nodeOutlineHandlers;
        [SerializeField] private Animator iceCube;
        [SerializeField] private FadeInOut water;
        private float updateRate = 0.5f;
        private float elpTime;
        private float daley = 1f;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        private void Start()
        {
            water.SetFadeAmount(0);
        }
        public void HideModel() { gameObject.SetActive(false); }
        public void StartSim()
        {
            pathsHandler.StartSimulation();
            firePS.Play(true);
            firePS.gameObject.SetActive(true);
            fireAudio.Play(1284);
            if (IsInvoking(nameof(CustomUpdate))) return;
            elpTime = 0;
            //manager.StartTemp();
            InvokeRepeating(nameof(CustomUpdate), daley, updateRate);
        }
        public void StopSim()
        {
            HandleOutline(-1);
            pathsHandler.StopSimulation();
            firePS.Stop(true);
            firePS.gameObject.SetActive(false);
            steamPS.Stop(false);
            waterBoilingAudio.Stop();
            waterDropAudio.Stop();
            waterDrop.gameObject.SetActive(false);
            fireAudio.Stop();
            state =  Kd1State.None;
            manager.StopTemp();
            if (!IsInvoking(nameof(CustomUpdate))) return;
            iceCube.SetBool("IsMelting", false);
            water.fadeInOut(false);
            CancelInvoke(nameof(CustomUpdate));
            StartCoroutine(manager.HandleRecap(-1, false, 3f));
        }

        private void CustomUpdate()
        {
            elpTime += updateRate;
            switch (state)
            {
                case Kd1State.None:
                    HandleOutline(-1);
                    if (elpTime >= (1))
                    {
                        
                        state = Kd1State.S1;
                        manager.StartTemp(-5,0,0.8f);
                        iceCube.SetBool("IsMelting", true);

                    }
                    break;
                case Kd1State.S1:
                    HandleOutline(0);
                    StartCoroutine(manager.HandleRecap(0, true, 1.5f));
                    if (elpTime >= (4.5))
                    {
                        state = Kd1State.S2;
                        manager.StartTemp(1,80,0.15f);
                        water.fadeInOut(true);
                    }
                    break;
                case Kd1State.S2:
                    HandleOutline(1);
                    StartCoroutine(manager.HandleRecap(1, true, 2f, 1.5f));
                    if (elpTime >= (16.5))
                    {
                        SetBoiling();
                        manager.StartTemp(81,100,0.15f);
                        state = Kd1State.S3;
                        
                    }
                    break;
                case Kd1State.S3:
                    HandleOutline(2);
                    StartCoroutine(manager.HandleRecap(2, true, 2f, 4f));
                    if (elpTime >= (23)) {
                        waterDrop.gameObject.SetActive(true);
                        waterDropAudio.Play();
                        if (!IsInvoking(nameof(CustomUpdate))) return;
                        CancelInvoke(nameof(CustomUpdate));
                    }
                    break;
                case Kd1State.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }  
        }

        private void SetBoiling()
        {
            waterBoilingAudio.volume = 0;
            waterBoilingAudio.Play();
            StartCoroutine(FadeAudioSource.StartFade(waterBoilingAudio, 10f, 1f));
            
            StartCoroutine(FadeInPS(steamPS,20f));
            steamPS.Play(true);
        }
        IEnumerator FadeInPS(ParticleSystem particle, float duration) {
            yield return new WaitForEndOfFrame();
            float currentTime = 0;
            var colorOverLifetime = steamPS.colorOverLifetime;
            List<ParticleSystem> mains = new List<ParticleSystem>();
           // colorOverLifetime.enabled = false;
            var pss = particle.GetComponentsInChildren<ParticleSystem>();
            Debug.Log($"pss:{pss.Length}");
            for (int i = 0; i < pss.Length; i++)
            {

                pss[i].startColor = new Color(pss[i].startColor.r, pss[i].startColor.g, pss[i].startColor.b, 0);
                mains.Add(pss[i]);


            }
                
            
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                foreach (var item in mains){

                    var a = Mathf.Lerp(0, 1, currentTime / duration);
                    item.startColor = new Color(item.startColor.r, item.startColor.g, item.startColor.b, a);
                  
                }
                yield return null;
            }
            //colorOverLifetime.enabled = true;
            yield break;

        }
        

        private void HandleOutline(int index)
        {
            //for (var i = 0; i < nodeOutlineHandlers.Count; i++)
            //{
            //    nodeOutlineHandlers[i].IsActive = index == i;
            //}
        }

        public void ToggleFireSound()
        {
            var mute = fireAudio.mute;
            mute = !mute;
            fireAudio.mute = mute;
            waterDropAudio.mute = !waterDropAudio.mute;
            waterBoilingAudio.mute = !waterBoilingAudio.mute;
            AudioManager.Instance.AudioController(mute ? 3 : 0);
        }

    }
}