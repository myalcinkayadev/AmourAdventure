using UnityEngine;

public enum WeatherType
{
    Clear,
    Rain,
    Snow
}

public class WeatherSystem : MonoBehaviour
{
    [Header("World Timer")]
    [SerializeField] private WorldTimer worldTimer;

    [Header("Weather Effects")]
    [SerializeField] private ParticleSystem rainEffect;
    [SerializeField] private ParticleSystem snowEffect;
    
    [Header("Ambient Colors")]
    [SerializeField] private Color clearAmbientColor = Color.white;
    [SerializeField] private Color rainAmbientColor = new Color(0.7f, 0.7f, 0.8f);
    [SerializeField] private Color snowAmbientColor = new Color(0.9f, 0.9f, 1f);

    private void OnEnable()
    {
        if (worldTimer != null) {
            worldTimer.OnWeatherChanged += OnWeatherChanged;
        }
    }

    private void OnDisable()
    {
        if (worldTimer != null) {
            worldTimer.OnWeatherChanged -= OnWeatherChanged;
        }
    }

    private void OnWeatherChanged(WeatherType weather)
    {
        if (rainEffect != null) rainEffect.Stop();
        if (snowEffect != null) snowEffect.Stop();

        Debug.Log($"Weather: {weather}");

        Color ambientColor = clearAmbientColor;
        switch (weather)
        {
            case WeatherType.Clear:
                ambientColor = clearAmbientColor;
                break;
            case WeatherType.Rain:
                ambientColor = rainAmbientColor;
                if (rainEffect != null) rainEffect.Play();
                break;
            case WeatherType.Snow:
                ambientColor = snowAmbientColor;
                if (snowEffect != null) snowEffect.Play();
                break;
        }

        RenderSettings.ambientLight = ambientColor;
    }

}
