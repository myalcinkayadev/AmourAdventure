using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public PlayerStats Stats => stats;
    public PlayerMana PlayerMana { get; private set; }

    private PlayerAnimations animations;

    public void Awake() {
        PlayerMana = GetComponent<PlayerMana>();
        animations = GetComponent<PlayerAnimations>();

        if (PlayerMana == null) {
            Debug.LogError($"{nameof(PlayerMana)} component is missing on {gameObject.name}.");
        }
        if (animations == null) {
            Debug.LogError($"{nameof(PlayerAnimations)} component is missing on {gameObject.name}.");
        }
    }

    public void ResetPlayer() {
        stats.ResetStats();
        PlayerMana.ResetMana();
        animations.ResetAnimation();
    }
}
