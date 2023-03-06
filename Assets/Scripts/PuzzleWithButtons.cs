using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleWithButtons : MonoBehaviour
{
    [SerializeField] private int needCatsCount;
    [SerializeField] private int currentCats;
    [SerializeField] private UnityEvent actionIfIs;
    [SerializeField] private UnityEvent actionIfNot;

    private readonly List<GameObject> _currentCats = new List<GameObject>();

    public void CheckCondition()
    {
        if (needCatsCount == currentCats)
        {
            actionIfIs?.Invoke();
        }
        else
        {
            actionIfNot?.Invoke();
        }
    }
    
    public void AddCat(GameObject gameObj)
    {
        if (!_currentCats.Contains(gameObj))
        {
            _currentCats.Add(gameObj);
            currentCats++;
        }
    }
    
    public void DelCat(GameObject gameObj)
    {
        if (_currentCats.Contains(gameObj))
        {
            _currentCats.Remove(gameObj);
            currentCats--;
        }
    }

}
