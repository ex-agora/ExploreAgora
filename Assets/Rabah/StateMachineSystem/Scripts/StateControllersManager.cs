using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// singleton class used to get the specified IStateController by the GetController<T>()function
    /// T is used to specify type
    /// </summary>
    public class StateControllersManager : MonoBehaviour

    {
        static StateControllersManager instance;
        public static StateControllersManager Instance { get => instance; set => instance = value; }
        //Singleton
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        private void Start()
        {
            controllers = controllersObj.Select(i => i.GetComponent<IStateController>()).ToList();
        }
        //Returns the IStateController according to the Action
        [TypeConstraint(typeof(IStateController))] [SerializeField] List<GameObject> controllersObj;
        List<IStateController> controllers;
        public T GetController<T>() where T : class, IStateController
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i] is T)
                {
                    return (T)controllers[i];
                }
            }
            return null;
        }
    }
}