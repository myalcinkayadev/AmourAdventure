using UnityEngine;

public class ActionPatrol : FSMAction
{
    [Header("Config")]
    [SerializeField] private float speed = 1.5f;

    private Waypoint waypoint;
    private int pointIndex;

    private Vector3 CurrentPosition => waypoint.GetPosition(pointIndex);

    public override void Act()
    {
        FollowPath();
    }

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();

        if (waypoint == null || waypoint.Points.Length == 0)
        {
            Debug.LogError($"ActionPatrol: No waypoints found on {gameObject.name}");
            enabled = false;
        }
    }

    private void FollowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, CurrentPosition) <= 0.1f)
        {
            UpdateNextPosition();
        }
    }

    private void UpdateNextPosition()
    {
        pointIndex = (pointIndex + 1) % waypoint.Points.Length;
    }
}
