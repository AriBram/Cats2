using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MonologueTrigger : MonoBehaviour
{
    [SerializeField] private Monologue monologue;

    [FormerlySerializedAs("actions")]
    [SerializeField] private UnityEvent actionIfStartMonologue;
    [SerializeField] private UnityEvent actionIfEndMonologue;

    [Header("Other Settings")] 
    [SerializeField] private bool isCatScene;
    [FormerlySerializedAs("isTitle")] [SerializeField] private bool isStatic;
    [SerializeField] private bool isOneTime;

    [FormerlySerializedAs("dialogueField")]
    [Header("Text Fields")]
    [SerializeField] private Text monologueField;
    
    private bool _isOpen;
    private bool _isCanTrigger = true;

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

    public bool IsCanTrigger
    {
        get => _isCanTrigger;
        set => _isCanTrigger = value;
    }

    public bool IsOpen
    {
        get => _isOpen;
        set => _isOpen = value;
    }

    private void Start()
    {
        if (isCatScene || isStatic)
        {
            TriggerMonologue();
        }
    }

    public void TriggerMonologue()
    {
        if (_isCanTrigger)
        {
            actionIfStartMonologue?.Invoke();
            gameObject.SetActive(true);
            if (!_isOpen && DialogueManager.Instance.LOGKey != DialogueManager.LogKey.Monologue)
            {
                gameObject.SetActive(true);
                DialogueManager.Instance.StartMonologue(this);
                _isOpen = true;
            }
        }
        if (isOneTime)
        {
            _isCanTrigger = false;
        }
        
    }

    public void ActionAfterEndMonologue()
    {
        if(isStatic)
        {
            monologueField.text = Monologue.Sentences[0];
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        print(DialogueManager.Instance.LOGKey);
        actionIfEndMonologue?.Invoke();
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