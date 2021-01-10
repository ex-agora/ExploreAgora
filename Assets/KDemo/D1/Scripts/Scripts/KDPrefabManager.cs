using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KDemo.D1.Scripts.Scripts
{
    public class KDPrefabManager : MonoBehaviour
    {
        public static KDPrefabManager Instance { get; set; }
        public KDRecapManager KDRecapManager { get => kDRecapManager; set => kDRecapManager = value; }

        [SerializeField] KDRecapManager kDRecapManager;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
    }
}