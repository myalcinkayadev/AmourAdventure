using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed;

    private readonly int moveXHash = Animator.StringToHash("MoveX");
    private readonly int moveYHash = Animator.StringToHash("MoveY");

    private Waypoint waypoint;
    private Animator animator;
    private Vector3 previousPosition;
    private int currentPointIndex;

    private void Awake() {
        waypoint = GetComponent<Waypoint>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        MoveTowardsNextPoint();
    }

    private void MoveTowardsNextPoint()
    {
        Vector3 targetPosition = waypoint.GetPosition(currentPointIndex);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        UpdateAnimation(targetPosition);

        if (IsAtTargetPosition(targetPosition)) {
            previousPosition = targetPosition;
            currentPointIndex = (currentPointIndex + 1) % waypoint.Points.Length;
        }
    }

    private void UpdateAnimation(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - previousPosition).normalized;

        animator.SetFloat(moveXHash, direction.x);
        animator.SetFloat(moveYHash, direction.y);
    }

    private bool IsAtTargetPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= 0.2f;
    }
}
