using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CriticalHealthEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume volume;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Effect Settings")]
    [SerializeField, Range(0f, 1f)] private float criticalHealthThreshold = 0.3f; 
    [SerializeField, Range(0f, 1f)] private float normalIntensity = 0f;    
    [SerializeField, Range(0f, 1f)] private float blinkIntensity = 1f;
    [SerializeField] private float blinkInterval = 0.5f;

    private ChromaticAberration chromaticAberration;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        if (volume != null && volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            chromaticAberration.intensity.value = normalIntensity;
        }
        else
        {
            Debug.LogError("CriticalHealthEffect: No Chromatic Aberration component found on the Volume.");
        }
    }

    public void StartBlinking()
    {
        if (blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkRoutine());
        }
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = normalIntensity;
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null) {
            playerHealth.OnHealthChanged += OnHealthChanged;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null) {
            playerHealth.OnHealthChanged -= OnHealthChanged;
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        float healthPercantage = currentHealth / maxHealth;
        if (healthPercantage < criticalHealthThreshold)
        {
            if (blinkCoroutine == null) {
                blinkCoroutine = StartCoroutine(BlinkRoutine());
            }
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            if (chromaticAberration != null) {
                chromaticAberration.intensity.value = normalIntensity;
                chromaticAberration.active = false;
            }
        }
    }

    private IEnumerator BlinkRoutine()
    {
        bool toggle = false;
        while (true)
        {
            if (chromaticAberration != null) {
                chromaticAberration.active = true;
                chromaticAberration.intensity.value = toggle ? blinkIntensity : normalIntensity;
            }
            toggle = !toggle;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
