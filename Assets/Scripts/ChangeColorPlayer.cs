using UnityEngine;

public class ChangeColorPlayer : MonoBehaviour
{
    [SerializeField] private Skin skin;
    [SerializeField] private AnimatorOverrideController orangeAnim;
    [SerializeField] private AnimatorOverrideController yellowAnim;

    private Animator _animator;
    private Animator _defaultAnimator;

    enum Skin{
        Default,
        Orange,
        Yellow
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _defaultAnimator = _animator;
    }

    private void Start()
    {
        switch (skin)
        {
            case Skin.Orange:
                SetOrangeSkin();
                break;
            case Skin.Yellow:
                SetYellowSkin();
                break;
            default:
                SetDefaultSkin();
                break;
        }
    }

    private void SetDefaultSkin()
    {
        _animator.runtimeAnimatorController = _defaultAnimator.runtimeAnimatorController;
    }

    private void SetOrangeSkin()
    {
        _animator.runtimeAnimatorController = orangeAnim as RuntimeAnimatorController;
    }
    
    private void SetYellowSkin()
    {
        _animator.runtimeAnimatorController = yellowAnim as RuntimeAnimatorController;
    }

}
