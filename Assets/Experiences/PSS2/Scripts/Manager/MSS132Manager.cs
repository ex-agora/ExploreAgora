using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132Manager : MonoBehaviour
{
    #region singletone
    static MSS132Manager instance;
    public static MSS132Manager Instance { get => instance; set => instance = value; }
    #endregion

    #region private variables
    bool isAnimationWorking;
    MSS132ElementHandler currentBulb;
    MSS132ElementHandler currentBottle;
    MSS132ElementHandler currentGasTube;
    MSS132PlantStates plantState;
    bool isSummaryShown;
    #endregion
    #region serialzed field
    [SerializeField] MSS132PlantAnimations plantAnim;
    [SerializeField] Elements elements;
    #endregion
    #region properties
    public bool IsAnimationWorking { get => isAnimationWorking; set => isAnimationWorking = value; }
    public MSS132PlantStates PlantState { get => plantState; set => plantState = value; }
    #endregion
    private void Awake ()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start ()
    {
        //Initial Plant State
        PlantState = MSS132PlantStates.None;
    }

    // Update is called once per frame
    void Update ()
    {

    }
    #region Logic Functions
    #region Update Current Elements
    public void UpdateCurrentBulb (MSS132ElementHandler currentElement)
    {
        currentBulb = currentElement;
    }
    public void UpdateCurrentWashBottle (MSS132ElementHandler currentElement)
    {
        currentBottle = currentElement;
    }
    public void UpdateCurrentGasTube (MSS132ElementHandler currentElement)
    {
        currentGasTube = currentElement;
    }
    #endregion
    #region Set Plant Data
    // Set endKeyValue of Plant transition from state to other state
    void SetPlantData (float endKeyValue)
    {
        plantAnim.EndKayValue = endKeyValue;
    }
    // Set endKeyValue of Plant transition to same state (need 2 values {Start & End})
    void SetPlantData (float startKayValue , float endKayValue)
    {
        plantAnim.StartKayValue = startKayValue;
        plantAnim.EndKayValue = endKayValue;
    }
    #endregion
    #region Plant Transitions
    public void PlantAliveToAlive ()
    {
        SetPlantData (plantAnim.AliveValue , plantAnim.AliveAnimationValue);
        plantAnim.StartPlantTransitionSameState ();
    }
    public void PlantDyingToDying ()
    {
        SetPlantData (plantAnim.DyingValue , plantAnim.DyingAnimationValue);
        plantAnim.StartPlantTransitionSameState ();
    }
    public void PlantThriving ()
    {
        SetPlantData (plantAnim.ThrivinvgValue);
        plantAnim.StartPlantTransition ();
    }
    public void PlantAlive ()
    {
        SetPlantData (plantAnim.AliveValue);
        plantAnim.StartPlantTransition ();
    }
    public void PlantDying ()
    {
        SetPlantData (plantAnim.DyingValue);
        plantAnim.StartPlantTransition ();
    }
    #endregion
    #region CheckElements
    //CheckElements to start transition
    //using after drag any element to its correct position in element's event listener in correct position
    public void CheckElements ()
    {
        if ( currentBottle == null || currentBulb == null || currentGasTube == null )
        {
            PlantState = MSS132PlantStates.None;
            return;
        }
        else if ( currentBottle == elements.WaterBottle && currentBulb == elements.WhiteBulb && currentGasTube == elements.CarbonDioxideOxygenGasTube )
        {
            PlantThriving ();
            PlantState = MSS132PlantStates.Thriving;
            MSS132GameManager.Instance.BarHandler.ActiveThriving ();
            if ( !isSummaryShown )
            {
                MSS132GameManager.Instance.ShowFinalSummery();
                isSummaryShown = true;
            }
        }
        else if ( currentBottle == elements.WaterBottle )
        {
            if ( currentBulb == elements.WhiteBulb && currentGasTube == elements.OxygenGasTube )
            {
                if ( PlantState == MSS132PlantStates.Alive )
                {
                    PlantAliveToAlive ();
                }
                else
                {
                    PlantAlive ();
                }
                PlantState = MSS132PlantStates.Alive;
                MSS132GameManager.Instance.BarHandler.ActiveAlive ();
            }
            else if ( currentBulb == elements.RedBulb && currentGasTube == elements.OxygenGasTube )
            {
                if ( PlantState == MSS132PlantStates.Alive )
                {
                    PlantAliveToAlive ();
                }
                else
                {
                    PlantAlive ();
                }
                PlantState = MSS132PlantStates.Alive;
                MSS132GameManager.Instance.BarHandler.ActiveAlive ();
            }
            else if ( currentBulb == elements.RedBulb && currentGasTube == elements.CarbonDioxideOxygenGasTube )
            {
                if ( PlantState == MSS132PlantStates.Alive )
                {
                    PlantAliveToAlive ();
                }
                else
                {
                    PlantAlive ();
                }
                PlantState = MSS132PlantStates.Alive;
                MSS132GameManager.Instance.BarHandler.ActiveAlive ();
            }
            else
            {
                if ( PlantState == MSS132PlantStates.Dying )
                {
                    PlantDyingToDying ();
                }
                else
                {
                    PlantDying ();
                }
                PlantState = MSS132PlantStates.Dying;
                MSS132GameManager.Instance.BarHandler.ActiveDying ();
            }
        }
        else
        {
            if ( PlantState == MSS132PlantStates.Dying )
            {
                PlantDyingToDying ();
            }
            else
            {
                PlantDying ();
            }
            PlantState = MSS132PlantStates.Dying;
            MSS132GameManager.Instance.BarHandler.ActiveDying ();
        }
    }
    #endregion
    #endregion
}
[System.Serializable]
public class Elements
{
    #region SerializeFields
    [Header ("Bulbs")]
    [Space (5)]
    [SerializeField] MSS132ElementHandler redBulb;
    [SerializeField] MSS132ElementHandler greenBulb;
    [SerializeField] MSS132ElementHandler whiteBulb;
    [Header ("Gas Tubes")]
    [Space (5)]
    [SerializeField] MSS132ElementHandler oxygenGasTube;
    [SerializeField] MSS132ElementHandler carbonDioxideOxygenGasTube;
    [SerializeField] MSS132ElementHandler nitrogenGasTube;
    [Header ("Bottles")]
    [Space (5)]
    [SerializeField] MSS132ElementHandler sodaBottle;
    [SerializeField] MSS132ElementHandler waterBottle;
    [SerializeField] MSS132ElementHandler saltWaterBottle;
    #endregion

    #region Properties
    public MSS132ElementHandler RedBulb { get => redBulb; set => redBulb = value; }
    public MSS132ElementHandler WhiteBulb { get => whiteBulb; set => whiteBulb = value; }
    public MSS132ElementHandler GreenBulb { get => greenBulb; set => greenBulb = value; }
    public MSS132ElementHandler OxygenGasTube { get => oxygenGasTube; set => oxygenGasTube = value; }
    public MSS132ElementHandler CarbonDioxideOxygenGasTube { get => carbonDioxideOxygenGasTube; set => carbonDioxideOxygenGasTube = value; }
    public MSS132ElementHandler NitrogenGasTube { get => nitrogenGasTube; set => nitrogenGasTube = value; }
    public MSS132ElementHandler SodaBottle { get => sodaBottle; set => sodaBottle = value; }
    public MSS132ElementHandler WaterBottle { get => waterBottle; set => waterBottle = value; }
    public MSS132ElementHandler SaltWaterBottle { get => saltWaterBottle; set => saltWaterBottle = value; }
    #endregion
}