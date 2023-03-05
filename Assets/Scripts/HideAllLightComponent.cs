using UnityEngine;

public class HideAllLightComponent : MonoBehaviour
{
    public void HideAllLight()
    {
        GameObject[] light2Ds = GameObject.FindGameObjectsWithTag("Light");
        foreach (var lightObj in light2Ds)
        {
            lightObj.SetActive(false);
        }
    }
}
