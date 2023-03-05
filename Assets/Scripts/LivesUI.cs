using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private Text livesField;
    private string _livesString;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            PlayerPrefs.SetString("LivesString", "0");
            
        }
        _livesString = PlayerPrefs.HasKey("LivesString") ? PlayerPrefs.GetString("LivesString") : "0";
        livesField.text = _livesString;
    }

    public void AddLive()
    {
        _livesString = GetNextLivesString(_livesString);
        livesField.text = _livesString;
    }

    public void SaveLivesString()
    {
        PlayerPrefs.SetString("LivesString", _livesString);
    }

    private string GetNextLivesString(string str)
    {
        switch (str)
        {
            case "0":
                return "1";
            case "1":
                return "2";
            case "2":
                return "3";
            case "3":
                return "3+1";
            case "3+1":
                return "3+2";
            case "3+2":
                return "3*2";
            case "3*2":
                return "3*2+1";
            case "3*2+1":
                return "3*2+2";
            case "3*2+2":
                return "3^2";
            default:
                return "0";
        }
    }
}