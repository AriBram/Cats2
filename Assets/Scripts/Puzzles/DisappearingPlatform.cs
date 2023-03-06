using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float delayInSecondsToApp = 1.2f;
    [SerializeField] private float delayInSecondsToDis = 1.5f;
    [SerializeField] private int needCatsCount;
    [SerializeField] private int currentCatsCount;
    [SerializeField] private bool canAppearance = true;
    [SerializeField] private Animator animator;

    private static readonly int ThereIs = Animator.StringToHash("there-is");

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        } 
    }

    public void Appearance()
    {
        if (canAppearance)
        {
            StartCoroutine(AppearanceWithDelay());
        }
    }
    
    public void Disappearance()
    {
        if (needCatsCount == currentCatsCount)
        {
            animator.SetBool(ThereIs, false);
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
        yield return new WaitForSeconds(delayInSecondsToApp);
        animator.SetBool(ThereIs, true);
    }
    
    IEnumerator DisappearanceWithDelay()
    {
        yield return new WaitForSeconds(delayInSecondsToDis);
        animator.SetBool(ThereIs, false);
    }
}
