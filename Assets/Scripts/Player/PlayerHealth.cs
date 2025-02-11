using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    private PlayerAnimations playerAnimations;

    public event Action<float, float> OnHealthChanged;

    private void Start()
    {
        OnHealthChanged?.Invoke(stats.Health, stats.MaxHealth);
        if (stats.Health <= 0f) {
            PlayerDead();
        }
    }

    public void TakeDamage(float amount)
    {
        if (stats.Health <= 0f) return;

        stats.Health = Mathf.Max(0f, stats.Health - amount);
        DamageUIManager.Instance.ShowDamageText(amount, transform);
        OnHealthChanged?.Invoke(stats.Health, stats.MaxHealth);

        if (stats.Health <= 0f) {
            PlayerDead();
        }
    }

    private void Awake() {
        playerAnimations = GetComponent<PlayerAnimations>();

        if (playerAnimations == null)
        {
            Debug.LogError($"PlayerHealth: No PlayerAnimations component found on {gameObject.name}.");
        }

        if (stats != null) stats.OnStatsReset += OnStatsReset;
    }

    private void OnDestroy()
    {
        if (stats != null) stats.OnStatsReset -= OnStatsReset;
    }

    private void OnStatsReset()
    {
        OnHealthChanged?.Invoke(stats.Health, stats.MaxHealth);
    }

    private void PlayerDead() {
        playerAnimations.SetDeadAnimation();
    }
}
