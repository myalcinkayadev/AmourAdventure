using UnityEngine;

public enum InteractionType {
    Quest,
    Shop,
}

[CreateAssetMenu(menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public Sprite Icon;

    [Header("Interaction")]
    public bool HasInteraction;
    public InteractionType InteractionType;

    [Header("Dialogue")]
    public string Greeting;
    [TextArea] public string[] Dialogue;
}
