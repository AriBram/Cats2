using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;

    [FormerlySerializedAs("jumpForce")] [SerializeField]
    private float jumpHeight;

    [SerializeField] private State state = State.Original;
    [SerializeField] private float delayInSeconds = 2;

    [Header("Boolean settings")] [FormerlySerializedAs("_isGrounded")] [SerializeField]
    private bool isGrounded = true;

    [FormerlySerializedAs("_isLadder")] [SerializeField]
    private bool isLadder;

    [FormerlySerializedAs("_isPlatform")] [SerializeField]
    private bool isPlatform;

    [SerializeField] private bool isKey;
    [SerializeField] private bool isMonologue;

    [Header("Interaction settings")] [SerializeField]
    private float interactionRadius;

    [SerializeField] private LayerMask interactionLayer;

    [FormerlySerializedAs("monologueCanvas")] [Header("Child objects")] [SerializeField]
    private GameObject monologueImage;

    [SerializeField] private List<GameObject> keys;

    private Rigidbody2D _rb;
    private Animator _animator;

    private Vector2 _direction;
    private float _velocityY;
    private float _velocityX;

    public bool IsMonologue
    {
        get => isMonologue;
        set => isMonologue = value;
    }

    public bool IsKey
    {
        get => isKey;
        set => isKey = value;
    }

    private readonly Collider2D[] _interactionResult = new Collider2D[2];

    enum State
    {
        Original,
        Reflection,
        Copy,
        CopyReflection
    }

    enum InputAct
    {
        Move,
        Jump,
        Interact
    }

    private static readonly int IsMove = Animator.StringToHash("is-move");
    private static readonly int IsGrounded = Animator.StringToHash("is-grounded");
    private static readonly int IsInteract = Animator.StringToHash("is-interact");
    private static readonly int IsClimb = Animator.StringToHash("is-climb");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isGrounded = true;
        _animator.SetBool(IsGrounded, true);
    }

    private void FixedUpdate()
    {
        var velocity = _rb.velocity;
        _velocityY = velocity.y;
        _velocityX = velocity.x;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isMonologue)
        {
            if (state is State.Copy or State.CopyReflection)
            {
                StartCoroutine(ActionWithDelay(InputAct.Move, context));
            }
            else
            {
                Move(context.ReadValue<Vector2>());
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isMonologue)
        {
            if (context.started)
            {
                if (state is State.Copy or State.CopyReflection)
                {
                    StartCoroutine(ActionWithDelay(InputAct.Jump, context));
                }
                else
                {
                    Jump();
                }
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!isMonologue)
        {
            if (context.performed)
            {
                if (state is State.Copy or State.CopyReflection)
                {
                    StartCoroutine(ActionWithDelay(InputAct.Interact, context));
                }
                else
                {
                    Interact();
                }
            }
        }
    }

    IEnumerator ActionWithDelay(InputAct inputAct, InputAction.CallbackContext context)
    {
        var delayDirection = Vector2.zero;
        if (inputAct == InputAct.Move)
        {
            delayDirection = context.ReadValue<Vector2>();
        }

        yield return new WaitForSeconds(delayInSeconds);

        switch (inputAct)
        {
            case InputAct.Move:
                Move(delayDirection);
                break;
            case InputAct.Jump:
                Jump();
                break;
            case InputAct.Interact:
                Interact();
                break;
        }
    }

    private void Move(Vector2 direction)
    {
        if (direction.y < 0 && isGrounded && !isPlatform)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = new Vector2(_velocityX, 0);
        }
        else if (direction.y != 0 && isLadder)
        {
            _animator.SetBool(IsClimb, true);
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.velocity = new Vector2(_velocityX, direction.y * speed * 40 * Time.fixedDeltaTime);
        }
        else
        {
            _animator.SetBool(IsClimb, false);
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (direction.y == 0)
        {
            _rb.velocity = new Vector2(_velocityX, direction.y);
        }

        if (direction.x != 0)
        {
            _rb.velocity = new Vector2(direction.x * speed + Time.fixedDeltaTime, _velocityY);
            if (state != State.Reflection && state != State.CopyReflection)
            {
                transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
                if (monologueImage != null)
                {
                    monologueImage.transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
                }
            }

            _animator.SetBool(IsMove, true);
        }
        else
        {
            _rb.velocity = new Vector2(direction.x, _rb.velocity.y);
            _animator.SetBool(IsMove, false);
        }
    }

    private void Jump()
    {
        if (isGrounded || isPlatform)
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * _rb.gravityScale));
            _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Interact()
    {
        _animator.SetTrigger(IsInteract);

        if (state is State.Reflection or State.CopyReflection) return;

        var size = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, _interactionResult,
            interactionLayer);

        for (int i = 0; i < size; i++)
        {
            var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public void SetGrounded(bool status)
    {
        isGrounded = status;
        isPlatform = false;

        if (isGrounded && !isLadder)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        _animator.SetBool(IsGrounded, isGrounded);
    }

    public void SetPlatformState(bool status)
    {
        isPlatform = status;
        isGrounded = false;

        _animator.SetBool(IsGrounded, isPlatform);
    }

    public void SetLadderState(bool status)
    {
        isLadder = status;
        if (!isLadder && (!isGrounded || !isPlatform))
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            //_rb.velocity = new Vector2(_rb.velocity.x, 0);
        }
    }

    public void SetMonologueState(bool status)
    {
        isMonologue = status;
        if (monologueImage != null)
        {
            monologueImage.SetActive(isMonologue);
        }
        
        if (isMonologue)
        {
            _rb.velocity = Vector2.zero;
            _animator.SetBool(IsMove, false);
        }
    }

    public void PickUpKey()
    {
        if (!isKey)
        {
            foreach (var key in keys)
            {
                key.SetActive(true);
            }

            isKey = true;
        }
    }

    public void UseKey()
    {
        foreach (var key in keys)
        {
            key.SetActive(false);
        }

        isKey = false;
    }

    public void PrintStr(string str)
    {
        print(str);
    }
}