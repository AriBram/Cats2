using UnityEngine;
using UnityEngine.Events;

public class InteractableComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private new bool enabled = true;

    public bool Enabled
    {
        get => enabled;
        set => enabled = value;
    }

    public void Interact()
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