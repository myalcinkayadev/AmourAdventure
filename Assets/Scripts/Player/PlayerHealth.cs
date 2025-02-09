using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    private PlayerAnimations playerAnimations;

    public void TakeDamage(float amount)
    {
        if (stats.Health <= 0f) return;

        stats.Health = Mathf.Max(0f, stats.Health - amount);
        DamageUIManager.Instance.ShowDamageText(amount, transform);

        if (stats.Health <= 0f) PlayerDead();
    }

    private void Awake() {
        playerAnimations = GetComponent<PlayerAnimations>();

        if (playerAnimations == null)
        {
            Debug.LogError($"PlayerHealth: No PlayerAnimations component found on {gameObject.name}.");
        }
    }

    private void Start()
    {
        if (stats.Health <= 0f) PlayerDead();
    }

    private void PlayerDead() {
        playerAnimations.SetDeadAnimation();
    }
}
