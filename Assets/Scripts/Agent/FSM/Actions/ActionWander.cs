using UnityEngine;

public class ActionWander : FSMAction
{
    [Header("Config")]
    [SerializeField] private float speed;
    [SerializeField] private float wanderTime;
    [SerializeField] private Vector2 moveRange;

    private Vector3 movePosition;
    private float timer;

    private AgentAnimation wanderAnimation;

    public override void Act()
    {
        if (timer <= 0f)
        {
            SetDestination();
            ResetWanderTimer();
        }

        MoveTowardsDestination();
        timer -= Time.deltaTime;
    }

    private void MoveTowardsDestination()
    {
        if (Vector3.Distance(transform.position, movePosition) < 0.5f) return;

        Vector3 direction = (movePosition - transform.position).normalized;
        Vector3 movement = direction * (speed * Time.deltaTime);
        transform.Translate(movement);

        if (wanderAnimation != null) wanderAnimation.SetMoveAnimation(movement);
    }

    private void Awake()
    {
        wanderAnimation = GetComponent<AgentAnimation>();

        ResetWanderTimer();
        SetDestination();
    }

    private void SetDestination() {
        float randomX = Random.Range(-moveRange.x, moveRange.x);
        float randomY = Random.Range(-moveRange.y, moveRange.y);
        movePosition = transform.position + new Vector3(randomX, randomY);
    }

    private void ResetWanderTimer()
    {
        timer = wanderTime;
    }

    private void OnDrawGizmosSelected() {
        if (moveRange == Vector2.zero) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, moveRange * 2f);
        Gizmos.DrawLine(transform.position, movePosition);
        Gizmos.DrawSphere(movePosition, 0.2f);
    }
}
