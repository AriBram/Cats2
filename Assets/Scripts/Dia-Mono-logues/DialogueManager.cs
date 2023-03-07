using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float delayPauseInSeconds = 2;
    [SerializeField] private float delayLettersApp = 0.08f;

    private Queue<string> _sentences;
    private Queue<string> _letterColors;

    public static DialogueManager Instance;

    private DialogueTrigger _dialogueTrigger;
    private MonologueTrigger _monologueTrigger;
    private LogKey _logKey = LogKey.Nothing;
    private bool _allowedShowNext;
    private bool _is1Field = true;
    private int _sentencesIndex;
    private bool _isAllText;
    private string _currentSentence;
    private string _currentLetterColors;
    private string _endDialogueKey = "Other";

    public enum LogKey
    {
        Nothing,
        Dialogue,
        Monologue
    }

    private static readonly int IsEndDialogueAnim = Animator.StringToHash("is-end");

    public LogKey LOGKey
    {
        get => _logKey;
        set => _logKey = value;
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
        if (_logKey != LogKey.Nothing)
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

        /*if (_logKey == LogKey.Dialogue)
        {
            if (_sentences.Count == 0 || _letterColors.Count == 0)
            {
                EndDialogue();
            }
            else if (_logKey == LogKey.Monologue)
            {
                if (_sentences.Count == 0 || _letterColors.Count == 0)
                {
                    EndMonologue();
                }
            }
        }*/
    }

    private void ShowAllText()
    {
        _isAllText = true;

        /*if (_logKey == LogKey.Dialogue)
        {
            if (_is1Field)
            {
                _dialogueTrigger.Dialogue1Field.text = _currentSentence;
            }
            else
            {
                _dialogueTrigger.Dialogue2Field.text = _currentSentence;
            }
        } else if (_logKey == LogKey.Monologue)
        {
            _monologueTrigger.MonologueField.text = _currentSentence;
        }*/

        var currentSentenceCharArr = _currentSentence.ToCharArray();
        var currentLetterColorsCharArr = _currentLetterColors.ToCharArray();

        if (_logKey == LogKey.Dialogue)
        {
            _dialogueTrigger.Dialogue1Field.text = "";
            _dialogueTrigger.Dialogue2Field.text = "";
        }
        else if (_logKey == LogKey.Monologue)
        {
            _monologueTrigger.MonologueField.text = "";
        }

        for (int i = 0; i < currentSentenceCharArr.Length; i++)
        {
            if (_logKey == LogKey.Dialogue)
            {
                if (_is1Field)
                {
                    _dialogueTrigger.Dialogue1Field.text +=
                        "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" +
                        currentSentenceCharArr[i] + "</color>";
                }
                else
                {
                    _dialogueTrigger.Dialogue2Field.text +=
                        "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" +
                        currentSentenceCharArr[i] + "</color>";
                }
            }
            else if (_logKey == LogKey.Monologue)
            {
                _monologueTrigger.MonologueField.text += "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) +
                                                         ">" + currentSentenceCharArr[i] + "</color>";
            }
        }


        _allowedShowNext = true;
    }

    public void StartDialogue(DialogueTrigger dialogueTrigger)
    {
        _isAllText = false;
        _logKey = LogKey.Dialogue;

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

        ShowNextSentence();
    }

    public void StartMonologue(MonologueTrigger monologueTrigger)
    {
        _logKey = LogKey.Monologue;

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

        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        _isAllText = false;
        _allowedShowNext = false;

        if (_logKey == LogKey.Dialogue)
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
                StartCoroutine(ShowSentenceByLetter());
            }
        }
        else if (_logKey == LogKey.Monologue)
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
                StartCoroutine(ShowSentenceByLetter());
            }
        }
    }

    IEnumerator ShowSentenceByLetter()
    {
        if (_logKey == LogKey.Dialogue)
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
            if (_isAllText)
            {
                yield break;
            }

            if (!_isAllText && _logKey == LogKey.Dialogue)
            {
                if (_is1Field)
                {
                    _dialogueTrigger.Dialogue1Field.text +=
                        "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" +
                        currentSentenceCharArr[i] + "</color>";
                }
                else
                {
                    _dialogueTrigger.Dialogue2Field.text +=
                        "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) + ">" +
                        currentSentenceCharArr[i] + "</color>";
                }

                yield return new WaitForSeconds(delayLettersApp);
            }
            else if (_logKey == LogKey.Monologue)
            {
                _monologueTrigger.MonologueField.text += "<color=" + SetLetterColor(currentLetterColorsCharArr[i]) +
                                                         ">" + currentSentenceCharArr[i] + "</color>";
                yield return new WaitForSeconds(delayLettersApp);
            }
            else
            {
                yield return null;
            }
        }

        yield return null;
        StartCoroutine(NextSentence());
        _allowedShowNext = true;
    }

    IEnumerator NextSentence()
    {
        yield return new WaitForSeconds(delayPauseInSeconds);
        if (_allowedShowNext)
        {
            ShowNextSentence();
        }
    }

    private void EndDialogue()
    {
        _dialogueTrigger.IsOpen = false;
        _logKey = LogKey.Nothing;
        _dialogueTrigger.ActionAfterEndDialogue();
    }

    private void EndMonologue()
    {
        _monologueTrigger.MonologueField.text = "";
        _monologueTrigger.IsOpen = false;
        _logKey = LogKey.Nothing;
        _monologueTrigger.ActionAfterEndMonologue();
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
                playerComponent.SetMonologueState(_logKey == LogKey.Monologue);
            }
        }

        foreach (var cat in catsReflection)
        {
            var playerComponent = cat.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.SetMonologueState(_logKey == LogKey.Monologue);
            }
        }
    }
}