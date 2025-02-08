using UnityEngine;

public class DecisionDetectPlayer : FSMDecision
{

    [Header("Config")]
    [SerializeField] private float range = 4f;
    [SerializeField] private LayerMask playerMask;

    private AgentBrain agent;

    public override bool Decide() => DetectPlayer();

    private void Awake() {
        agent = GetComponent<AgentBrain>();

        if (agent == null)
        {
            Debug.LogError($"DecisionDetectPlayer: No agentBrain found on {gameObject.name}");
            enabled = false;
        }
    }

    private bool DetectPlayer() {
        Collider2D playerCollider = Physics2D.OverlapCircle(agent.transform.position, range, playerMask);
        
        if (playerCollider != null) {
            agent.SetPlayer(playerCollider.transform);
            return true;
        }

        agent.SetPlayer(null);
        return false;
    }

    private void OnDrawGizmosSelected() {
        if (range <= 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
