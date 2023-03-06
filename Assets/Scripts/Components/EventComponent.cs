using UnityEngine;
using UnityEngine.Events;

public class EventComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private new bool enabled = true;

    public void PerformAction()
    {
        if (enabled)
        {
            action?.Invoke();   
        }
    }

    public void SetEnabled(bool status)
    {
        enabled = status;
    }
}
