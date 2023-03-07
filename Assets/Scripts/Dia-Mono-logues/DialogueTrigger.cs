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
    private Text dialogue1Field;

    [FormerlySerializedAs("dialogueField")] [SerializeField]
    private Text dialogue2Field;

    [Header("Images")] [SerializeField] private Image image1;

    public Image Image1
    {
        get => image1;
        set => image1 = value;
    }

    public Image Image2
    {
        get => image2;
        set => image2 = value;
    }

    [SerializeField] private Image image2;

    private Animator _animator;
    private bool _isOpen;

    public Dialogue Dialogue
    {
        get => dialogue;
        set => dialogue = value;
    }

    public Text Dialogue1Field
    {
        get => dialogue1Field;
        set => dialogue1Field = value;
    }

    public Text Dialogue2Field
    {
        get => dialogue2Field;
        set => dialogue2Field = value;
    }

    public bool IsOpen
    {
        get => _isOpen;
        set => _isOpen = value;
    }

    private void Start()
    {
        if (isCatScene)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        gameObject.SetActive(true);
        if (!_isOpen && DialogueManager.Instance.LOGKey != DialogueManager.LogKey.Dialogue)
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
    [TextArea(3, 10)] [SerializeField] private string[] sentences;

    [SerializeField] private int[] sentencesCount;

    [Header("Colors")] [TextArea(3, 10)] [SerializeField]
    private string[] letterColors;

    public string[] LetterColors
    {
        get => letterColors;
        set => letterColors = value;
    }

    public int[] SentencesCount
    {
        get => sentencesCount;
        set => sentencesCount = value;
    }

    public string[] Sentences
    {
        get => sentences;
        set => sentences = value;
    }
}