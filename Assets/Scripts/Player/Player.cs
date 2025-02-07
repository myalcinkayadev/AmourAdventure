using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public PlayerStats Stats => stats;

    private PlayerAnimations animations;

    public void Awake() {
        animations = GetComponent<PlayerAnimations>();
    }

    public void ResetPlayer() {
        stats.ResetStats();
        animations.ResetAnimation();
    }
}
