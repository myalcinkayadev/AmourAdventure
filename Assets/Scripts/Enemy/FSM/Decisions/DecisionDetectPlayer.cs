using UnityEngine;

public class DecisionDetectPlayer : FSMDecision
{

    [Header("Config")]
    [SerializeField] private float range = 4f;
    [SerializeField] private LayerMask playerMask;

    private EnemyBrain enemy;

    public override bool Decide() => DetectPlayer();

    private void Awake() {
        enemy = GetComponent<EnemyBrain>();

        if (enemy == null)
        {
            Debug.LogError($"DecisionDetectPlayer: No EnemyBrain found on {gameObject.name}");
            enabled = false;
        }
    }

    private bool DetectPlayer() {
        Collider2D playerCollider = Physics2D.OverlapCircle(enemy.transform.position, range, playerMask);
        
        if (playerCollider != null) {
            enemy.SetPlayer(playerCollider.transform);
            return true;
        }

        enemy.SetPlayer(null);
        return false;
    }

    private void OnDrawGizmosSelected() {
        if (range <= 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
