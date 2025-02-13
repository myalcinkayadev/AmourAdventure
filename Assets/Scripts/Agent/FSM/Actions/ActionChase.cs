using UnityEngine;

public class ActionChase : FSMAction
{
    [Header("Config")]
    [SerializeField] private float chaseSpeed;

    private const float ChaseRange = 1.69f;

    private AgentBrain agentBrain;

    private AgentAnimation chaseAnimation;

    public override void Act()
    {
        ChasePlayer();
    }

    private void Awake() {
        agentBrain = GetComponent<AgentBrain>();
        chaseAnimation = GetComponent<AgentAnimation>();

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
            Vector3 movement = directionToPlayer.normalized * (chaseSpeed * Time.deltaTime);
            transform.Translate(movement);
            
            if (chaseAnimation != null) chaseAnimation.SetMoveAnimation(movement);
        }
    }
}
