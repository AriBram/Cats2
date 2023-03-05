using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MonologueTrigger : MonoBehaviour
{
    [SerializeField] private Monologue monologue;

    [FormerlySerializedAs("actionIfEndDialogue")]
    [SerializeField] private UnityEvent actionIfStartMonologue;
    [SerializeField] private UnityEvent actionIfEndMonologue;

    [Header("Other Settings")] 
    [SerializeField] private bool isCatScene;

    [FormerlySerializedAs("dialogueField")]
    [Header("Text Fields")]
    [SerializeField] private Text monologueField;

    private Animator _animator;
    private bool _isOpen;

    public Monologue Monologue
    {
        get => monologue;
        set => monologue = value;
    }

    public Text MonologueField
    {
        get => monologueField;
        set => monologueField = value;
    }

    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    public bool IsOpen
    {
        get => _isOpen;
        set => _isOpen = value;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (isCatScene)
        {
            TriggerMonologue();
        }
    }

    public void TriggerMonologue()
    {
        actionIfStartMonologue?.Invoke();
        gameObject.SetActive(true);
        if (!_isOpen && !DialogueManager.Instance.IsDialog)
        {
            gameObject.SetActive(true);
            DialogueManager.Instance.StartMonologue(this);
            _isOpen = true;
        }
    }

    public void ActionAfterEndMonologue()
    {
        actionIfEndMonologue?.Invoke();
        gameObject.SetActive(false);
    }
}

[Serializable]
public class Monologue
{
    [TextArea(3, 10)] [SerializeField] private string[] sentences;

    [Header("Colors")] [TextArea(3, 10)] [SerializeField] private string[] letterColors;

    public string[] LetterColors
    {
        get => letterColors;
        set => letterColors = value;
    }

    public string[] Sentences
    {
        get => sentences;
        set => sentences = value;
    }
}