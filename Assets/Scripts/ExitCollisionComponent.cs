using System;
using UnityEngine;
using UnityEngine.Events;

public class ExitCollisionComponent : MonoBehaviour
{
    [SerializeField] private new string tag;
    [SerializeField] private GameObjectChange action;

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            action?.Invoke(other.gameObject);
        }
    }

    [Serializable]
    public class GameObjectChange : UnityEvent<GameObject>
    {
        
    }
}
