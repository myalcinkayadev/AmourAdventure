using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class DialogueResponse
{
    public string[] dialogue;
}

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Config")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI npcNameTMP;
    [SerializeField] private TextMeshProUGUI npcDialogueTMP;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f; 

    private CinemachineCamera cinemachineCamera;
    private readonly float zoomedSize = 3f;
    private float defaultSize = 5f; 
    private readonly float zoomSpeed = 0.5f;

    public NPCInteraction NPCSelected { get; set; }

    private PlayerAction actions;
    private readonly Queue<string> dialogueQueue = new Queue<string>();
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentSentence;

    public event Action<string> OnDialogueEnd;

    protected override void Awake()
    {
        base.Awake();
        actions = new PlayerAction();

        if (cinemachineCamera == null) {
            // Trying to find automatically
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        }

        if (cinemachineCamera != null) {
            defaultSize = cinemachineCamera.Lens.OrthographicSize;
        }
    }

    public void EndDialogue()
    {
        if (dialoguePanel.activeSelf && NPCSelected != null)
        {
            OnDialogueEnd?.Invoke(NPCSelected.name);
        }

        StartCoroutine(ZoomCamera(defaultSize));
        dialoguePanel.SetActive(false);
        dialogueQueue.Clear();
        npcDialogueTMP.text = "";
        npcIcon.sprite = null;
        npcNameTMP.text = "";
        isTyping = false;

        NPCSelected = null;
    }

    private void Start()
    {
        if (actions == null) actions = new PlayerAction();

        actions.Interaction.Interact.performed += ctx => StartDialogue();
        actions.Interaction.Continue.performed += ctx => ContinueDialogue();
    }

    private void StartDialogue()
    {
        if (NPCSelected == null || dialoguePanel.activeSelf) return;

        dialoguePanel.SetActive(true);
        npcIcon.sprite = NPCSelected.DialogueToShow.Icon;
        npcNameTMP.text = NPCSelected.DialogueToShow.Name;
        StartCoroutine(ZoomCamera(zoomedSize));

        dialogueQueue.Clear();

        if (!NPCSelected.DialogueToShow.IsLLM)
        {
            LoadDialogueFromNPC();
        }
        else
        {
            LoadDialogueFromLLM(NPCSelected.DialogueToShow.ApiUrl);
        }
    }

    private void LoadDialogueFromNPC()
    {
        foreach (string sentence in NPCSelected.DialogueToShow.Dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    private void LoadDialogueFromLLM(string url)
    {
        IEnumerator LLMCoroutine()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                DialogueResponse response = null;
                if (request.result == UnityWebRequest.Result.Success)
                {
                    response = JsonUtility.FromJson<DialogueResponse>(request.downloadHandler.text);
                }

                if (request.result != UnityWebRequest.Result.Success || response?.dialogue == null)
                {
                    Debug.LogError("API Error or invalid response: " + request.error);
                    dialogueQueue.Enqueue("<Thinking>");
                }
                else
                {
                    foreach (string sentence in response.dialogue)
                    {
                        dialogueQueue.Enqueue(sentence);
                    }
                }
            }
            DisplayNextSentence();
        }

        StartCoroutine(LLMCoroutine());
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

    private IEnumerator ZoomCamera(float targetSize)
    {
        if (cinemachineCamera == null) yield break;

        float startSize = cinemachineCamera.Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            elapsedTime += Time.deltaTime;
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / zoomSpeed);
            yield return null;
        }

        cinemachineCamera.Lens.OrthographicSize = targetSize;
    }

    private void OnEnable()
    {
        if (actions == null) actions = new PlayerAction(); 
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}