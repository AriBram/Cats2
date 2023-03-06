using UnityEngine;

public class OpenLinkComponent : MonoBehaviour
{
    [SerializeField] private string link;

    public void OpenLink()
    {
        Application.OpenURL(link);
    }
}
