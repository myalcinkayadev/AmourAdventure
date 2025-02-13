using UnityEngine;

public class MemoAction : MonoBehaviour
{
    [SerializeField] private FlowerSpawner flowerSpawner;

    private void Start()
    {
        DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
    }

    private void OnDialogueEnd(string name)
    {
        if (gameObject.name == name)
        {
            flowerSpawner.StartSpawning(gameObject.transform.position);
        }
    }
}
