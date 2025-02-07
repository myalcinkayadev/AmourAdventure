using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Config")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI npcNameTMP;
    [SerializeField] private TextMeshProUGUI npcDialogueTMP;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f; // Adjust speed of typing effect

    public NPCInteraction NPCSelected { get; set; }

    private PlayerAction actions;
    private bool dialogueStarted;
    private bool isTyping;
    private readonly Queue<string> dialogueQueue = new Queue<string>();
    private Coroutine typingCoroutine;
    private string currentSentence; 

    protected override void Awake() {
        base.Awake();
        actions = new PlayerAction();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueStarted = false;
        isTyping = false;
        dialogueQueue.Clear();
    }

    private void Start() {
        actions.Dialogue.Interact.performed += ctx => StartDialogue();
        actions.Dialogue.Continue.performed += ctx => ContinueDialogue();
    }

    private void StartDialogue()
    {
        if (NPCSelected == null || dialogueStarted)
            return;

        dialogueStarted = true;
        LoadDialogueFromNPC();
        OpenDialoguePanel();
        DisplayNextSentence();
    }

    private void LoadDialogueFromNPC() {
        dialogueQueue.Clear();

        if (NPCSelected.DialogueToShow.Dialogue == null || NPCSelected.DialogueToShow.Dialogue.Length == 0)
            return;

        foreach (string sentence in NPCSelected.DialogueToShow.Dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }
    }

    private void OpenDialoguePanel()
    {
        dialoguePanel.SetActive(true);
        npcIcon.sprite = NPCSelected.DialogueToShow.Icon;
        npcNameTMP.text = NPCSelected.DialogueToShow.Name;
    }

    private void ContinueDialogue()
    {
        if (!dialogueStarted)
            return;

        if (isTyping)
        {
            CompleteCurrentSentence();
        }
        else if (dialogueQueue.Count > 0)
        {
            DisplayNextSentence();
        }
        else
        {
            EndDialogue();
        }
    }

    private void CompleteCurrentSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        npcDialogueTMP.text = currentSentence;
        isTyping = false;
    }


    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = dialogueQueue.Dequeue();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        npcDialogueTMP.text = "";

        foreach (char letter in sentence)
        {
            npcDialogueTMP.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void OnEnable() {
        actions.Enable();
    }

    private void OnDisable() {
        actions.Disable();
    }
}
