using UnityEngine;

public class DecisionAttackRange : FSMDecision
{
    [Header("Config")]
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerMask;

    private AgentBrain enemy;

    public override bool Decide()
    {
        return PlayerInAttackRange();
    }

    private void Awake() {
        enemy = GetComponent<AgentBrain>();

        if (enemy == null)
        {
            Debug.LogError($"DecisionDetectPlayer: No AgentBrain found on {gameObject.name}");
            enabled = false;
        }
    }

    private bool PlayerInAttackRange() {
        if (enemy.Player == null) return false;

        Collider2D playerCollider = Physics2D.OverlapCircle(enemy.transform.position, attackRange, playerMask);
        
        if (playerCollider != null) {
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected() {
        if (attackRange <= 0) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
