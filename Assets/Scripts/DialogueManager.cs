using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool _allowedShowNext;
    private bool _is1Field = true;
    private int _sentencesIndex;
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Skip();
        }
    }

    private void Skip()
    {
        if (_isDialog || _isMonologue)
        {
            if (_allowedShowNext)
            {
                ShowNextSentence(_isMonologue ? LogKey.Monologue : LogKey.Dialogue);
            }
        }
    }

    /*private void ShowAllText(LogKey logKey)
    {
        _isAllText = true;
        if (logKey == LogKey.Dialogue)
        {
            _dialogueTrigger.DialogueField.text = _currentSentence;
        }
        else
        {
            _monologueTrigger.MonologueField.text = _currentSentence;
        }
        
        _allowedShowNext = true;
    }*/

    public void StartDialogue(DialogueTrigger dialogueTrigger)
    {
        _isAllText = false;
        _isDialog = true;

        _dialogueTrigger = dialogueTrigger;
        _sentencesIndex = 0;

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
    
    public void StartMonologue(MonologueTrigger monologueTrigger)
    {
        _isMonologue = true;

        SetMonologueStateToCats();

        _monologueTrigger = monologueTrigger;
        _sentencesIndex = 0;

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

    private void ShowNextSentence(LogKey logKey)
    {
        _isAllText = false;
        _allowedShowNext = false;
        
        if (logKey == LogKey.Dialogue)
        {
            if (_sentences.Count == 0 || _letterColors.Count == 0)
            {
                EndDialogue();
            }
            else
            {
                if (_dialogueTrigger.Dialogue.SentencesCount[_sentencesIndex] == 0)
                {
                    _sentencesIndex++;
                    _is1Field = !_is1Field;
                }
                _dialogueTrigger.Dialogue.SentencesCount[_sentencesIndex]--;

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
            if (_is1Field)
            {
                _dialogueTrigger.Dialogue1Field.text = "";
                _dialogueTrigger.Image1.gameObject.SetActive(true);
                _dialogueTrigger.Image2.gameObject.SetActive(false);
            }
            else
            {
                _dialogueTrigger.Dialogue2Field.text = "";
                _dialogueTrigger.Image2.gameObject.SetActive(true);
                _dialogueTrigger.Image1.gameObject.SetActive(false);
            }
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
                if (_is1Field)
                {
                    _dialogueTrigger.Dialogue1Field.text += "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" +
                                                           currentSentenceCharArr[i] + "</color>";
                }
                else
                {
                    _dialogueTrigger.Dialogue2Field.text += "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" +
                                                           currentSentenceCharArr[i] + "</color>";
                }
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
        _allowedShowNext = true;
    }

    IEnumerator NextSentence(LogKey logKey)
    {
        yield return new WaitForSeconds(delayPauseInSeconds);
        if (_allowedShowNext)
        {
            ShowNextSentence(logKey);
        }
    }

    private void EndDialogue()
    {
        _dialogueTrigger.IsOpen = false;
        _isDialog = false;
        _dialogueTrigger.ActionAfterEndDialogue();
    }

    private void EndMonologue()
    {
        _monologueTrigger.MonologueField.text = "";
        _monologueTrigger.IsOpen = false;
        _monologueTrigger.ActionAfterEndMonologue();
        
        _isMonologue = false;
        SetMonologueStateToCats();
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

    private void SetMonologueStateToCats()
    {
        GameObject[] cats = GameObject.FindGameObjectsWithTag("Cat");
        GameObject[] catsReflection = GameObject.FindGameObjectsWithTag("CatReflection");
        
        foreach (var cat in cats)
        {
            var playerComponent = cat.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.SetMonologueState(_isMonologue);
            }
        }
        foreach (var cat in catsReflection)
        {
            var playerComponent = cat.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.SetMonologueState(_isMonologue);
            }
        }
    }

}