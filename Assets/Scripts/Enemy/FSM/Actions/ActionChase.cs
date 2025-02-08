using UnityEngine;

public class ActionChase : FSMAction
{
    [Header("Config")]
    [SerializeField] private float chaseSpeed;

    private const float ChaseRange = 1.69f;

    private EnemyBrain enemyBrain;

    public override void Act()
    {
        ChasePlayer();
    }

    private void Awake() {
        enemyBrain = GetComponent<EnemyBrain>();

        if (enemyBrain == null)
        {
            Debug.LogError($"ActionChase: No EnemyBrain found on {gameObject.name}");
            enabled = false;
        }
    }
    
    private void ChasePlayer() {
        if (enemyBrain.Player == null) return;
        
        Vector3 directionToPlayer = enemyBrain.Player.position - transform.position;
        if (directionToPlayer.magnitude >= ChaseRange) {
            transform.Translate(directionToPlayer.normalized * (chaseSpeed * Time.deltaTime));
        }
    }
}
