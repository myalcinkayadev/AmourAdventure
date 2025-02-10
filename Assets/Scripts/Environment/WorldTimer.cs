using System;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    [Header("Day–Night Settings")]
    [SerializeField] private float dayDuration = 120f;
    [SerializeField, Range(0f, 1f)] private float startTime = 0.25f;

    public event Action<float> OnDayCycleUpdate;  // Normalized day time [0,1]

    private float dayTimer;

    private float NormalizedTime => dayTimer / dayDuration;
    
    private void Start()
    {
        dayTimer = Mathf.Clamp01(startTime) * dayDuration;
        OnDayCycleUpdate?.Invoke(NormalizedTime);
    }

    private void Update()
    {
        // Day–Night cycle.
        dayTimer += Time.deltaTime;
        if (dayTimer > dayDuration) {
            dayTimer -= dayDuration;
        }
        OnDayCycleUpdate?.Invoke(NormalizedTime);
    }
}
