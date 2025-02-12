using UnityEngine;
using UnityEngine.InputSystem;

public class StoneInteraction : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject interactionBox;
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private float teleportCooldown = 1f;

    private float currentTeleportCooldown;
    private PlayerAction actions;
    private Transform playerTransform;

    private void Awake()
    {
        actions = new PlayerAction();
    }

    private void Update()
    {
        if (currentTeleportCooldown > 0f)
            currentTeleportCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionBox != null)
                interactionBox.SetActive(true);

            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionBox != null)
                interactionBox.SetActive(false);

            playerTransform = null;
        }
    }

    private void OnEnable()
    {
        actions.Enable();
        actions.Interaction.Interact.performed += TeleportPlayer;
    }

    private void OnDisable()
    {
        actions.Interaction.Interact.performed -= TeleportPlayer;
        actions.Disable();
    }

    private void TeleportPlayer(InputAction.CallbackContext ctx)
    {
        if (teleportDestination == null)
        {
            Debug.LogError("StoneInteraction: Teleport destination is not assigned!");
            return;
        }
        if (playerTransform == null)
        {
            Debug.LogWarning("StoneInteraction: No player in trigger area; cannot teleport.");
            return;
        }
        if (currentTeleportCooldown > 0f)
            return;

        playerTransform.position = teleportDestination.position;
        Debug.Log("Player teleported to: " + teleportDestination.position);
        currentTeleportCooldown = teleportCooldown;
    }
}
