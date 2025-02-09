using System;
using UnityEngine;

public class AgentHealth : MonoBehaviour, IDamageable
{
    public static event Action OnEnemyDeadEvent;

    [Header("Config")]
    [SerializeField] private float health;

    private readonly int deadHash = Animator.StringToHash("Dead");
    public float CurrentHealth { get; private set; }

    private Animator animator;
    private AgentBrain agentBrain;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agentBrain = GetComponent<AgentBrain>();
    }

    private void Start()
    {
        CurrentHealth = health;
    }

    public void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0f) return;
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);

        if (CurrentHealth <= 0f) Die();
        else DamageUIManager.Instance.ShowDamageText(amount, transform);
    }

    private void Die()
    {
        if (agentBrain.enabled)
        {
            animator.SetTrigger(deadHash);
            agentBrain.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            OnEnemyDeadEvent?.Invoke();
        }
    }
}