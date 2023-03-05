using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadSceneComponent : MonoBehaviour
{
    [SerializeField] private string nameScene;
    [SerializeField] private UnityEvent action;

    public void LoadScene()
    {
        action?.Invoke();
        SceneManager.LoadScene(nameScene);
    }
}
