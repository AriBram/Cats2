using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnterTriggerComponent : MonoBehaviour
{
    [FormerlySerializedAs("_tag")] [SerializeField] private new string tag;
    [FormerlySerializedAs("_action")] [SerializeField] private GameObjectChange action;
    [SerializeField] private bool isTriggerEnterOnly;
    
    private void OnTriggerEnter2D(Collider2D  other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            DoAction(other);
        }
    }
    
    private void OnTriggerStay2D(Collider2D  other)
    {
        if (other.gameObject.CompareTag(tag) && !isTriggerEnterOnly)
        {
            DoAction(other);
        }
    }

    public void DoAction(Collider2D  other)
    {
        action?.Invoke(other.gameObject);
    }
    
    [Serializable]
    public class GameObjectChange : UnityEvent<GameObject>
    {
        
    }
}