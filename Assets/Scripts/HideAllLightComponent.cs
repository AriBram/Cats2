using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HideAllLightComponent : MonoBehaviour
{
    public void HideAllLight()
    {
        var light2Ds = FindLight2Ds();
        
        foreach (var lightObj in light2Ds)
        {
            lightObj.enabled = false;
        }
    }
    
    public void ShowAllLight()
    {
        var light2Ds = FindLight2Ds();
        
        foreach (var lightObj in light2Ds)
        {
            lightObj.enabled = true;
        }
    }

    private List<Light2D> FindLight2Ds()
    {
        GameObject[] objLights = GameObject.FindGameObjectsWithTag("Light");
        List<Light2D> light2Ds = new List<Light2D>();
        
        foreach (var objLight in objLights)
        {
            Light2D[] children = objLight.GetComponentsInChildren<Light2D>();
            if (children != null)
            {
                foreach (var child in children)
                {
                    light2Ds.Add(child);
                }
            }
        }

        return light2Ds;
    }
}
