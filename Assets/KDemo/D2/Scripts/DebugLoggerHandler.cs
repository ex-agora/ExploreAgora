using System;
using UnityEngine;

namespace KDemo.D2.Scripts
{
    public class DebugLoggerHandler : MonoBehaviour
    {
        private static DebugLoggerHandler _instance;
        [SerializeField] private bool isActive;
        public static DebugLoggerHandler Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Log(string msg)
        {
            if(!isActive) return;
            Debug.Log(msg);
        }
        
    }
}