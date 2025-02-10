using UnityEngine;

[System.Serializable]
public class DayPhase
{
    public string phaseName;     // e.g., "Sunrise", "Morning", "Noon", etc.

    [Range(0f, 1f)]
    public float startTime;      // Normalized start time (0 = start of day, 1 = end of day)

    public Color phaseColor;     // Ambient color for this phase.
    public float intensity;      // Light intensity for this phase.
}