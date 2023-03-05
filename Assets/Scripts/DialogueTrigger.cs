using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    [FormerlySerializedAs("action")] [SerializeField]
    private UnityEvent actionIfEndDialogue;

    [Header("Other Settings")] [SerializeField]
    private bool isCatScene;

    [Header("Text Fields")] [SerializeField]
    private Text nameField;

    [SerializeField] private Text dialogueField;

    private Animator _animator;
    private bool _isOpen;

    public Dialogue Dialogue
    {
        get => dialogue;
        set => dialogue = value;
    }

    public Text NameField
    {
        get => nameField;
        set => nameField = value;
    }

    public Text DialogueField
    {
        get => dialogueField;
        set => dialogueField = value;
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
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        gameObject.SetActive(true);
        if (!_isOpen && !DialogueManager.Instance.IsDialog)
        {
            gameObject.SetActive(true);
            DialogueManager.Instance.StartDialogue(this);
            _isOpen = true;
        }
    }

    public void ActionAfterEndDialogue()
    {
        actionIfEndDialogue?.Invoke();
        gameObject.SetActive(false);
    }
}

[Serializable]
public class Dialogue
{
    [SerializeField] private string[] names;

    [TextArea(3, 10)] [SerializeField] private string[] sentences;

    [SerializeField] private int[] sentencesCountWithName;

    [Header("Colors")] [SerializeField] private string[] letterColors;

    public string[] LetterColors
    {
        get => letterColors;
        set => letterColors = value;
    }

    public int[] SentencesCountWithName
    {
        get => sentencesCountWithName;
        set => sentencesCountWithName = value;
    }

    public string[] Names
    {
        get => names;
        set => names = value;
    }

    public string[] Sentences
    {
        get => sentences;
        set => sentences = value;
    }
}