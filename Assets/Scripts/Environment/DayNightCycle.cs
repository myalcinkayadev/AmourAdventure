using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [Header("World Timer")]
    [SerializeField] private WorldTimer worldTimer;

    [Header("Global Light Settings")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private float defaultYRotation = 0f;

    [Header("Day Phases - sunrise, morning, noon, afternoon, evening, night")]
    [SerializeField, Tooltip("Configure the day phases from Sunrise to Night. Ensure the phases are sorted by their start time.")]
    private DayPhase[] dayPhases = new DayPhase[]
    {
        new()
        {
            phaseName = "Sunrise",
            startTime = 0f,
            phaseColor = new Color(1f, 0.5f, 0.2f), // Soft orange
            intensity = 0.2f
        },
        new()   
        {
            phaseName = "Morning",
            startTime = 0.2f,
            phaseColor = new Color(1f, 0.8f, 0.5f), // Warm yellow
            intensity = 0.8f
        },
        new()
        {
            phaseName = "Noon",
            startTime = 0.4f,
            phaseColor = Color.white, // Bright white
            intensity = 1f
        },
        new()
        {
            phaseName = "Afternoon",
            startTime = 0.6f,
            phaseColor = new Color(1f, 0.95f, 0.9f), // Soft, warm light
            intensity = 0.9f
        },
        new()
        {
            phaseName = "Evening",
            startTime = 0.7f,
            phaseColor = new Color(1f, 0.6f, 0.4f), // Pinkish orange
            intensity = 0.5f
        },
        new()
        {
            phaseName = "Night",
            startTime = 0.8f,
            phaseColor = new Color(0.1f, 0.15f, 0.4f), // Deep blue
            intensity = 0.01f
        }
    };

    private void Start()
    {
        if (globalLight == null)
        {
            Debug.LogWarning("No Light assigned");
        }
    }

    private void OnEnable()
    {
        if (worldTimer != null) {
            worldTimer.OnDayCycleUpdate += OnUpdateDayCycle;
        }
    }

    private void OnDisable()
    {
        if (worldTimer != null) {
            worldTimer.OnDayCycleUpdate -= OnUpdateDayCycle;
        }
    }

    private void OnUpdateDayCycle(float normalizedTime)
    {
        if (globalLight == null) return;

        // Find the current phase index based on the normalized time.
        int currentIndex = 0;
        for (int i = 0; i < dayPhases.Length; i++)
        {
            if (dayPhases[i].startTime <= normalizedTime)
                currentIndex = i;
        }
        int nextIndex = (currentIndex + 1) % dayPhases.Length;
        
        // Calculate the interpolation factor between the current and next phase.
        float t = CalculatePhaseInterpolationFactor(normalizedTime, currentIndex, nextIndex);

        // Interpolate the light color and intensity.
        Color phaseColor = Color.Lerp(dayPhases[currentIndex].phaseColor, dayPhases[nextIndex].phaseColor, t);
        float phaseIntensity = Mathf.Lerp(dayPhases[currentIndex].intensity, dayPhases[nextIndex].intensity, t);
        
        // Compute the sun angle from normalized time.
        float sunAngle = normalizedTime * 360f - 90f;
        globalLight.transform.rotation = Quaternion.Euler(sunAngle, defaultYRotation, 0f);
        globalLight.color = phaseColor;
        globalLight.intensity = phaseIntensity;

        //RenderSettings.ambientLight = globalLight.color * globalLight.intensity;
    }

    private float CalculatePhaseInterpolationFactor(float normalizedTime, int currentIndex, int nextIndex)
    {
        float currentPhaseStart = dayPhases[currentIndex].startTime;
        float t = 0f;

        // Handle wrap-around: when nextIndex is 0, we are transitioning from the last phase to the first.
        if (nextIndex == 0)
        {
            // Duration from the current phase start to the end of the day plus the start of the first phase.
            float wrapDuration = 1f - currentPhaseStart + dayPhases[0].startTime;
            t = (normalizedTime - currentPhaseStart) / wrapDuration;
        }
        else
        {
            float nextPhaseStart = dayPhases[nextIndex].startTime;
            t = Mathf.InverseLerp(currentPhaseStart, nextPhaseStart, normalizedTime);
        }
        
        return t;
    }
}
