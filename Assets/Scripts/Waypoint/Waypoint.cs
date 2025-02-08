using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Vector3[] points;

    public Vector3[] Points => points;
    public Vector3 EntityPosition { get; set; }

    private bool gameStarted;
    
    private void Start()
    {
        EntityPosition = transform.position;
        gameStarted = true;
    }

    public Vector3 GetPosition(int pointIndex)
    {
        if (points == null || points.Length == 0)
        {
            Debug.LogWarning($"Waypoint '{name}' has no points assigned.");
            return EntityPosition;
        }
        
        return EntityPosition + points[pointIndex];
    }
    
    private void OnDrawGizmos()
    {
        if (gameStarted == false && transform.hasChanged)
        {
            EntityPosition = transform.position;
        }
    }
}
