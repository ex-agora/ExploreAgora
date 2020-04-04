using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AchievmentProperties))]
public class AchievementSystem : MonoBehaviour, IObserver
{
    // conditional counter which determine if the achievement is Done or not  
    public int counter;
    // instance of AchievmentManager which responsible of save , load achievement required and calling Refresh ui method from within achievement ProPerties 
    public AchievmentManager achievmentManager;


    // according to observer pattern this method called when something(counter , ... ) that observable responsible to watch changed 
    public void OnNotificationReceived(int observableDeliveredValue, ObservableHandler observable)
    {
        //check recieved observable is extend ObservableHandler 
        // some other achievment may require another observables 
        if (!(observable is TestObservable))
            return;
        TestObservable t = (TestObservable)observable;
        print("Entered " + observableDeliveredValue + " " + t.name);

        //if recieved value meet requirement then achievement is done so no longer use of this achievment so we must UnRegister it from observable list
        if (observableDeliveredValue == counter)
        {
            Debug.Log("Achievment unlocked");
            achievmentManager.RefreshUI(GetComponent<AchievmentProperties>().AchievementSO.Id, true);
            observable.UnRegisterObservers(this);
        }
    }

    public void OnNotificationReceived(bool x, ObservableHandler observable)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        //initializing achievment Manager 
        achievmentManager.AddAchievments(GetComponent<AchievmentProperties>());
    }

    // Start is called before the first frame update
    void Start()
    {
        // search for all needed observables to register in 
        var Observables = FindObjectsOfType<TestObservable>();
        foreach (var Observable in Observables)
        {
            Observable.RegisterObservers(this);
        }

    }
}
