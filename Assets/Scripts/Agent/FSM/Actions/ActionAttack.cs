using System.Threading;
using UnityEngine;

public class ActionAttack : FSMAction
{
    [Header("Config")]
    [SerializeField] private float damage;
    [SerializeField] private float timeBtwAttacks;

    private AgentBrain enemyBrain;
    private float attackTimer;

    public override void Act()
    {
        AttackPlayer();
    }

    private void Awake() {
        enemyBrain = GetComponent<AgentBrain>();

        if (enemyBrain == null)
        {
            Debug.LogError($"ActionAttack: No AgentBrain found on {gameObject.name}");
            enabled = false;
        }
    }

    private void AttackPlayer() {
        if (enemyBrain.Player == null) return;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        IDamageable player = enemyBrain.Player.GetComponent<IDamageable>();
        if (player == null) {
            Debug.LogError($"ActionAttack: {enemyBrain.Player.name} does not implement IDamageable!");
            return; 
        }

        player.TakeDamage(damage);
        attackTimer = timeBtwAttacks;
    }
}
