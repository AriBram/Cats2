using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private new string tag;
    [SerializeField] private GameObjectChange action;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            action?.Invoke(other.gameObject);
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
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
