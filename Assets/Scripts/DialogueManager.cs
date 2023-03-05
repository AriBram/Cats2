using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float delayPauseInSeconds = 2;
    private Queue<string> _sentences;
    private Queue<string> _letterColors;

    public static DialogueManager Instance;

    private DialogueTrigger _dialogueTrigger;
    private MonologueTrigger _monologueTrigger;
    private bool _isDialog;
    private bool _isMonologue;
    private int _nameIndex;
    private bool _isAllText;
    private string _currentSentence;
    private string _currentLetterColors;
    private string _endDialogueKey = "Other";

    enum LogKey
    {
        Dialogue,
        Monologue
    }

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
        Instance = this;
        _sentences = new Queue<string>();
        _letterColors = new Queue<string>();
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

    /*private void ShowAllText()
    {
        _isAllText = true;
        //StopCoroutine(ShowSentenceByLetter());
        _dialogueTrigger.DialogueField.text = _currentSentence;
        _allowedShowNext = true;
    }*/

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

        ShowNextSentence(LogKey.Dialogue);
    }

    private void ShowNextSentence(LogKey logKey)
    {
        if (logKey == LogKey.Dialogue)
        {
            _isAllText = false;
            //_allowedShowNext = false;

            if (_nameIndex != _dialogueTrigger.Dialogue.Names.Length)
            {
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
                StartCoroutine(ShowSentenceByLetter(LogKey.Dialogue));
            }
        }
        else
        {
            if (_sentences.Count == 0 || _letterColors.Count == 0)
            {
                EndMonologue();
            }
            else
            {
                //_monologueTrigger.Monologue.Sentences.Length

                _currentSentence = _sentences.Dequeue();
                _currentLetterColors = _letterColors.Dequeue();
                StartCoroutine(ShowSentenceByLetter(LogKey.Monologue));
            }
        }
    }

    IEnumerator ShowSentenceByLetter(LogKey logKey)
    {
        if (logKey == LogKey.Dialogue)
        {
            _dialogueTrigger.DialogueField.text = "";
        }
        else
        {
            _monologueTrigger.MonologueField.text = "";
        }


        var currentSentenceCharArr = _currentSentence.ToCharArray();
        var currentLetterColorsCharArr = _currentLetterColors.ToCharArray();

        for (int i = 0; i < currentSentenceCharArr.Length; i++)
        {
            if (!_isAllText && logKey == LogKey.Dialogue)
            {
                _dialogueTrigger.DialogueField.text += "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" +
                                                       currentSentenceCharArr[i] + "</color>";
                yield return new WaitForSeconds(0.08f);
            }
            else if (logKey == LogKey.Monologue)
            {
                _monologueTrigger.MonologueField.text += "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) +
                                                         ">" + currentSentenceCharArr[i] + "</color>";
                yield return new WaitForSeconds(0.08f);
            }
            else
            {
                yield return null;
            }
        }

        yield return null;
        StartCoroutine(NextSentence(logKey));
        //_allowedShowNext = true;
    }

    IEnumerator NextSentence(LogKey logKey)
    {
        yield return new WaitForSeconds(delayPauseInSeconds);
        ShowNextSentence(logKey);
    }

    private void EndDialogue()
    {
        _dialogueTrigger.Animator.SetBool(IsEndDialogueAnim, true);
        _dialogueTrigger.IsOpen = false;
        _isDialog = false;
    }

    public void StartMonologue(MonologueTrigger monologueTrigger)
    {
        _isMonologue = true;

        _monologueTrigger = monologueTrigger;
        _nameIndex = 0;

        _sentences.Clear();
        _letterColors.Clear();

        foreach (var sentence in _monologueTrigger.Monologue.Sentences)
        {
            _sentences.Enqueue(sentence);
        }

        foreach (var sentence in _monologueTrigger.Monologue.LetterColors)
        {
            _letterColors.Enqueue(sentence);
        }

        ShowNextSentence(LogKey.Monologue);
    }

    private void EndMonologue()
    {
        _monologueTrigger.MonologueField.text = "";
        _monologueTrigger.IsOpen = false;
        _monologueTrigger.ActionAfterEndMonologue();
        _isMonologue = false;
    }
}