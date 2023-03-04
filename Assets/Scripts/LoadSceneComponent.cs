using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneComponent : MonoBehaviour
{
    [SerializeField] private string nameScene;

    public void LoadScene()
    {
        SceneManager.LoadScene(nameScene);
    }
}
