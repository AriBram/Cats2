using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private ReflectionState state = ReflectionState.NotReflection;

    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isGrounded = true;
    private Vector2 _direction;
    
    enum ReflectionState{
        Reflection,
        NotReflection
    }
    
    private static readonly int IsMove = Animator.StringToHash("is-move");
    private static readonly int IsGrounded = Animator.StringToHash("is-grounded");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    private void Move(Vector2 direction)
    {
        if (direction.x != 0)
        {
            _rb.velocity = new Vector2(direction.x * speed + Time.deltaTime, _rb.velocity.y);
            if (state != ReflectionState.Reflection)
            {
                transform.localScale = new Vector3(direction.x, 1, 1);
            }
            _animator.SetBool(IsMove, true);
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            _animator.SetBool(IsMove, false);
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void SetGrounded(bool status)
    {
        _isGrounded = status;
        _animator.SetBool(IsGrounded, _isGrounded);
    }

    public void PrintStr(string str)
    {
        print(str);
    }
}
