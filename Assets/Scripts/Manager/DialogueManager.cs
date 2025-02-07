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
    private readonly Queue<string> dialogueQueue = new Queue<string>();
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentSentence;

    protected override void Awake() {
        base.Awake();
        actions = new PlayerAction();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueQueue.Clear();
        isTyping = false;
    }

    private void Start()
    {
        actions.Dialogue.Interact.performed += ctx => StartDialogue();
        actions.Dialogue.Continue.performed += ctx => ContinueDialogue();
    }

    private void StartDialogue()
    {
        if (NPCSelected == null || dialoguePanel.activeSelf)
            return;

        dialoguePanel.SetActive(true);
        npcIcon.sprite = NPCSelected.DialogueToShow.Icon;
        npcNameTMP.text = NPCSelected.DialogueToShow.Name;

        dialogueQueue.Clear();
        foreach (string sentence in NPCSelected.DialogueToShow.Dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void ContinueDialogue()
    {
        if (isTyping) SkipTyping();
        else DisplayNextSentence();
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
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
        RestartTypingCoroutine(currentSentence);
    }

    private void RestartTypingCoroutine(string sentence)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
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
