using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> _sentences;
    private Queue<string> _letterColors;

    public static DialogueManager Instance;

    private bool _allowedShowNext;
    private DialogueTrigger _dialogueTrigger;
    private bool _isDialog;
    private int _nameIndex;
    private bool _isAllText;
    private string _currentSentence;
    private string _currentLetterColors;
    private string _playerName = "You";
    private string _endDialogueKey = "Other";

    private static readonly int IsEndDialogueAnim = Animator.StringToHash("is-end");

    public string EndDialogueKey
    {
        get => _endDialogueKey;
        set => _endDialogueKey = value;
    }

    public bool IsDialog
    {
        get => _isDialog;
        set => _isDialog = value;
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Name"))
        {
            _playerName = PlayerPrefs.GetString("Name");
        }
        Instance = this;
        _sentences = new Queue<string>();
    }

    /*protected override void Enter()
    {
        if (_isDialog)
        {
            if (_allowedShowNext)
            {
                ShowNextSentence();
            }
            else if (!_isAllText)
            {
                ShowAllText();
            } 
        }
    }*/

    private void ShowAllText()
    {
        _isAllText = true;
        //StopCoroutine(ShowSentenceByLetter());
        _dialogueTrigger.DialogueField.text = _currentSentence;
        _allowedShowNext = true;
    }

    public void StartDialogue(DialogueTrigger dialogueTrigger)
    {
        _isAllText = false;
        _isDialog = true;

        _dialogueTrigger = dialogueTrigger;
        _nameIndex = 0;

        _sentences.Clear();
        _letterColors.Clear();

        foreach (var sentence in _dialogueTrigger.Dialogue.Sentences)
        {
            _sentences.Enqueue(sentence);
        }
        
        foreach (var sentence in _dialogueTrigger.Dialogue.LetterColors)
        {
            _letterColors.Enqueue(sentence);
        }

        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        _isAllText = false;
        _allowedShowNext = false;

        if (_nameIndex != _dialogueTrigger.Dialogue.Names.Length)
        {
            if (_dialogueTrigger.Dialogue.Names[_nameIndex] == "You")
            {
                _dialogueTrigger.Dialogue.Names[_nameIndex] = _playerName;
            }
            _dialogueTrigger.NameField.text = _dialogueTrigger.Dialogue.Names[_nameIndex];
        }
        
        if (_sentences.Count == 0 || _letterColors.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            _dialogueTrigger.Dialogue.SentencesCountWithName[_nameIndex]--;
            if (_dialogueTrigger.Dialogue.SentencesCountWithName[_nameIndex] == 0)
            {
                _nameIndex++;
            }

            _currentSentence = _sentences.Dequeue();
            _currentLetterColors = _letterColors.Dequeue();
            StartCoroutine(ShowSentenceByLetter());
        }
    }

    IEnumerator ShowSentenceByLetter()
    {
        _dialogueTrigger.DialogueField.text = "";
        var currentSentenceCharArr = _currentSentence.ToCharArray();
        var currentLetterColorsCharArr = _currentLetterColors.ToCharArray();
        
        for (int i = 0; i < currentSentenceCharArr.Length; i++)
        {
            
                if (!_isAllText)
                {
                    _dialogueTrigger.DialogueField.text += "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" + currentSentenceCharArr[i] + "</color>";
                    yield return new WaitForSeconds(0.08f);
                }
                else
                {
                    yield return null;
                }
            
        }
        
        yield return null;
        _allowedShowNext = true;
    }

    private string SetLetterColor(char code)
    {
        switch (code)
        {
            case 'd':
                return "#4D234A";
            case 'o':
                return "#D4715D";
            case 'y':
                return "#F3B486";
            case 'g':
                return "#F3B486";
            case 'b':
                return "#EFEBEA";
            default:
                return "#FFFFFF";
        }
    }

    private void EndDialogue()
    {
        _dialogueTrigger.Animator.SetBool(IsEndDialogueAnim, true);
        _dialogueTrigger.IsOpen = false;
        _isDialog = false;
    }
}