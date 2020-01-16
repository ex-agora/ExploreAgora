using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Manage the state system and can start,pause,stop and transit to the next state the satte
    /// current state is the state that is working now, 
    /// remainState is used to stay in the current state
    /// </summary>
    public class StateMachineManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] State autoPreviousState;

        [SerializeField] StateControllersManager controllersManager;

        [SerializeField] State currentState;

        float elpTime = 0.0f;

        bool isPause;

        bool isWorking;

        [SerializeField] State previousState;

        //use it if will not go to next state
        [SerializeField] State remainState;
        #endregion Fields

        #region Properties
        public float ElpTime { get => elpTime; set => elpTime = value; }
        public bool IsWorking { get => isWorking; set => isWorking = value; }
        #endregion Properties

        #region Methods
        //Go to next state and reset elapse time
        public bool GoTo(State nextState)
        {
            if (nextState != remainState)
            {
                currentState.OnExitState<IStateController>(controllersManager);
                if (currentState.StateType == StateType.PrimaryState)
                    previousState = currentState;
                currentState = nextState;
                currentState.OnEnterState<IStateController>(controllersManager);
                ElpTime = 0.0f;
                return true;
            }
            return false;
        }

        public void GoToPrevious()
        {
            if (currentState == autoPreviousState)
            {
                currentState = previousState;
                currentState.OnEnterState<IStateController>(controllersManager);
                ElpTime = 0.0f;
            }
        }

        //Pause the current state if it is working
        public void PasueSM()
        {
            if (isWorking)
            {
                isPause = !isPause;
            }
        }

        //Start the current state if it is not working
        public void StartSM()
        {
            if (!IsWorking)
            {
                IsWorking = true;
                currentState.OnEnterState<IStateController>(controllersManager);
            }
        }
        //Stop the current state if it is working
        public void StopSM()
        {
            if (isWorking)
            {
                IsWorking = false;
                isPause = false;
                currentState.OnExitState<IStateController>(controllersManager);
            }
        }

        private void FixedUpdate()
        {
            if (!IsWorking || isPause)
                return;
            currentState.OnFixedStayState<IStateController>(controllersManager);
        }

        private void LateUpdate()
        {
            if (!IsWorking || isPause)
                return;
            currentState.OnLateStayState<IStateController>(controllersManager);
        }

        private void Start()
        {
            IsWorking = false;
            isPause = false;
        }
        private void Update()
        {
            if (!IsWorking || isPause)
                return;
            currentState.OnStayState<IStateController>(controllersManager);
            currentState.CheckTransiton<IStateController>(this, controllersManager);
            ElpTime += Time.deltaTime;
        }
        #endregion Methods
    }
}