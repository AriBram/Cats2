using UnityEngine;

public class ChangeColorPlayer : MonoBehaviour
{
    [SerializeField] private Skin skin;
    [SerializeField] private AnimatorOverrideController orangeAnim;

    private Animator _animator;
    private Animator _defaultAnimator;

    enum Skin{
        Default,
        Orange
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _defaultAnimator = _animator;

        switch (skin)
        {
            case Skin.Orange:
                SetOrangeSkin();
                break;
            default:
                SetDefaultSkin();
                break;
        }
    }
    
    public void SetDefaultSkin()
    {
        _animator.runtimeAnimatorController = _defaultAnimator.runtimeAnimatorController;
    }

    public void SetOrangeSkin()
    {
        _animator.runtimeAnimatorController = orangeAnim as RuntimeAnimatorController;
    }

}
