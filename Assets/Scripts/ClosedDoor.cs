using UnityEngine;
using UnityEngine.Events;

public class ClosedDoor : MonoBehaviour
{
    [SerializeField] private int needKeysCount;
    [SerializeField] private int currentKeysCount;
    [SerializeField] private InteractableComponent interactableComponent;

    public void CheckKeysCount()
    {
        if (needKeysCount == currentKeysCount)
        {
            interactableComponent.Enabled = true;
        }
        else
        {
            interactableComponent.Enabled = false;
        }
    }

    public void AddKey()
    {
        currentKeysCount++;
    }

    public void DelKey()
    {
        currentKeysCount--;
    }
}
