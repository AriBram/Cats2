using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float delayInSeconds;
    [SerializeField] private int needCatsCount;
    [SerializeField] private int currentCatsCount;
    private Animator _animator;
    
    private static readonly int ThereIs = Animator.StringToHash("there-is");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Appearance()
    {
        StartCoroutine(AppearanceWithDelay());
    }
    
    public void Disappearance()
    {
        if (needCatsCount == currentCatsCount)
        {
            _animator.SetBool(ThereIs, false);
        }
    }

    public void AddCat()
    {
        currentCatsCount++;
    }
    
    public void DelCat()
    {
        currentCatsCount--;
    }

    IEnumerator AppearanceWithDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        _animator.SetBool(ThereIs, true);
    }
    
    IEnumerator DisappearanceWithDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        _animator.SetBool(ThereIs, true);
    }
}
