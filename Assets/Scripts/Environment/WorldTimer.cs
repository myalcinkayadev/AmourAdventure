using System;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    [Header("Day–Night Settings")]
    [SerializeField] private float dayDuration = 120f;
    [SerializeField, Range(0f, 1f)] private float startTime = 0.25f;

    [Header("Weather Settings")]
    [SerializeField] private float minWeatherDuration = 120f;
    [SerializeField] private float maxWeatherDuration = 300f;
    [SerializeField] private WeatherType initialWeather = WeatherType.Clear;

    public event Action<float> OnDayCycleUpdate;  // Normalized day time [0,1]
    public event Action<WeatherType> OnWeatherChanged;

    private float dayTimer;
    private float weatherTimer;
    private WeatherType currentWeather;

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

        weatherTimer -= Time.deltaTime;
        if (weatherTimer <= 0f)
        {
            ChangeWeather();
            weatherTimer = UnityEngine.Random.Range(minWeatherDuration, maxWeatherDuration);
        }
    }
    private void ChangeWeather()
    {
        Debug.Log("ChangeWeather");
        int weatherCount = Enum.GetNames(typeof(WeatherType)).Length;
        currentWeather = (WeatherType)UnityEngine.Random.Range(0, weatherCount);
        OnWeatherChanged?.Invoke(currentWeather);
    }
}
