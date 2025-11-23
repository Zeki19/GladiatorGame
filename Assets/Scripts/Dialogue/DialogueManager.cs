using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
    private DialogueManager _instance;

    #region Constants
    [Header("Wiring")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueBoxUIText;
    [SerializeField] private Image speakerBoxUIImage;
    [SerializeField] private TextMeshProUGUI speakerBoxUIName;
    [Space]
    [SerializeField] private AudioSource audioSource;
    [Space]
    [SerializeField] private Animator animator;
    private CameraShake _cameraShake;
    #endregion

    #region Variables
    [Header("Dialogue config")]
    [SerializeField] private float typingDelay;
    [SerializeField] private Sound typingSound;

    #endregion

    [SerializeField] private List<DialogueSO> dialogues;

    private readonly Queue<DialogueLine> _linesQueue = new Queue<DialogueLine>();
    private Action _onTypeEnd;
    public Action OnConversationEnd;
    public Action OnLineChange;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);

        ServiceLocator.Instance.RegisterService(this);
        
        if(typingSound.clip) audioSource.volume = typingSound.volume;
    }
    public void StartConversation(DialogueSO dialogue)
    {
        dialogueCanvas.SetActive(true);
        animator.SetBool("IsOpen", true);
        audioSource.enabled = true;

        QueueDialogue(dialogue);
    }

    public void StartConversation(EnumDialogues dialogue)
    {
        DialogueSO dialogueSo = FindDialogueSo(dialogue);

        dialogueCanvas.SetActive(true);
        animator.SetBool("IsOpen", true);
        audioSource.enabled = true;

        QueueDialogue(dialogueSo);
    }

    private void QueueDialogue(DialogueSO dialogue)
    {
        _linesQueue.Clear();

        foreach (var line in dialogue.lines)
        {
            _linesQueue.Enqueue(line);
        }

        DisplayNextSentence();
    }

    private DialogueSO FindDialogueSo(EnumDialogues dialogue)
    {
        foreach (var so in dialogues)
        {
            if (so.name == dialogue.ToString())
            {
                return so;
            }
        }

        Debug.Log("No dialogue with that name");
        return null;
    }
    public void DisplayNextSentence()
    {
        if (_linesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        OnLineChange?.Invoke();
        OnLineChange = null;

        var currentLine = _linesQueue.Dequeue();

        PrepareUI(currentLine);

        // Types the line
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));
    }
    private IEnumerator CloseDialogueRoutine()
    {
        animator.SetBool("IsOpen", false);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float duration = stateInfo.length;

        yield return new WaitForSeconds(duration);

        OnConversationEnd?.Invoke();
        dialogueCanvas.SetActive(false);
        audioSource.enabled = false;
    }
    public void EndDialogue()
    {
        StartCoroutine(CloseDialogueRoutine());
    }
    private IEnumerator TypeSentence(string sentence)
    {
        dialogueBoxUIText.text = "";
        foreach (char letter in sentence)
        {
            dialogueBoxUIText.text += letter;
            if (typingSound.clip)
            {
                audioSource.PlayOneShot(typingSound.clip);
            }
            yield return new WaitForSeconds(typingDelay);
        }
        _onTypeEnd?.Invoke();
        _onTypeEnd = null;
    }
    private void PrepareUI(DialogueLine line)
    {
        _onTypeEnd = null;

        if (_cameraShake == null)
        {
            _cameraShake = ServiceLocator.Instance.GetService<CameraShake>();
        }

        // Speaker info
        speakerBoxUIName.text = line.speakerName;
        speakerBoxUIImage.sprite = line.speakerImage;

        // Audio handling
        if (line.soundToPlay != null)
        {
            if (line.playAtTheStart)
                audioSource.PlayOneShot(line.soundToPlay);
            else
                _onTypeEnd += () => audioSource.PlayOneShot(line.soundToPlay);
        }

        //Shaker
        if (line.cameraShake)
        {
            if (line.shakeAtTheStart)
                _cameraShake.Shake();
            else
                _onTypeEnd += () => _cameraShake.Shake();
        }
    }

    public void SkipDialogue()
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("Skipping");
        audioSource.enabled = false;
        dialogueCanvas.SetActive(false);
    }
}
