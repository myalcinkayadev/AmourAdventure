using UnityEngine;

public class ActionChase : FSMAction
{
    [Header("Config")]
    [SerializeField] private float chaseSpeed;

    private const float ChaseRange = 1.69f;

    private AgentBrain agentBrain;

    public override void Act()
    {
        ChasePlayer();
    }

    private void Awake() {
        agentBrain = GetComponent<AgentBrain>();

        if (agentBrain == null)
        {
            Debug.LogError($"ActionChase: No AgentBrain found on {gameObject.name}");
            enabled = false;
        }
    }
    
    private void ChasePlayer() {
        if (agentBrain.Player == null) return;
        
        Vector3 directionToPlayer = agentBrain.Player.position - transform.position;
        if (directionToPlayer.magnitude >= ChaseRange) {
            transform.Translate(directionToPlayer.normalized * (chaseSpeed * Time.deltaTime));
        }
    }
}
