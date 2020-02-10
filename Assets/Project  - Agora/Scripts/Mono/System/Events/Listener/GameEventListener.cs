using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    #region Fields
    [SerializeField] GameEvent @event;
    [SerializeField] UnityEvent eventSystem;
    #endregion Fields

    #region Methods
    public void Fire()
    {
        eventSystem.Invoke();
    }

    private void OnDisable()
    {
        @event.UnSubscribe(this);
    }

    private void OnEnable()
    {
        @event.Subscribe(this);
    }
    #endregion Methods
}
