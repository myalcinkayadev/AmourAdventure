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

    public NPCInteraction NPCSelected { get; set; }

    private bool dialogueStarted;
    private PlayerAction actions;
    private Queue<string> dialogueQueue = new Queue<string>();

    protected override void Awake() {
        base.Awake();
        actions = new PlayerAction();
    }

    private void Start() {
        actions.Dialogue.Interact.performed += ctx => ShowDialogue();
        actions.Dialogue.Continue.performed += ctx => ContinueDialogue();
    }

    private void ShowDialogue() {
        if (NPCSelected == null || dialogueStarted) return;

        dialogueStarted = true;
        LoadDialogueFromNPC();

        dialoguePanel.SetActive(true);
        npcIcon.sprite = NPCSelected.DialogueToShow.Icon;
        npcNameTMP.text = NPCSelected.DialogueToShow.Name;
        npcDialogueTMP.text = NPCSelected.DialogueToShow.Greeting;
    }

    private void LoadDialogueFromNPC() {
        if (NPCSelected.DialogueToShow.Dialogue.Length <= 0) return;

        foreach (string sentence in NPCSelected.DialogueToShow.Dialogue) {
            dialogueQueue.Enqueue(sentence);
        }
    }

    private void ContinueDialogue() {
        if (NPCSelected == null || dialogueQueue.Count <= 0)
        {
            CloseDialoguePanel();
            return;
        }

        npcDialogueTMP.text = dialogueQueue.Dequeue();
    }

    public void CloseDialoguePanel() {
        dialoguePanel.SetActive(false);
        dialogueStarted = false;
        dialogueQueue.Clear();
    }

    private void OnEnable() {
        actions.Enable();
    }

    private void OnDisable() {
        actions.Disable();
    }
}
