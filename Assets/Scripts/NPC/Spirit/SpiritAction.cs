using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpiritAction : MonoBehaviour
{
    [Header("Light Config")]
    [SerializeField] private Light2D innerLight;
    [SerializeField] private float minLightRadius = 1f;
    [SerializeField] private float maxLightRadius = 12f;
    [SerializeField] private float transitionDuration = 3f;

    private void Start()
    {
        DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
    }

    private void OnDialogueEnd(string name)
    {
        if (innerLight != null && gameObject.name == name)
        {
            StartCoroutine(IncreaseLightThenDecrease());
        }
    }

    private IEnumerator IncreaseLightThenDecrease()
    {
        yield return StartCoroutine(ChangeLightOverTime(maxLightRadius, transitionDuration));

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(ChangeLightOverTime(minLightRadius, transitionDuration));
    }

    private IEnumerator ChangeLightOverTime(float targetRadius, float duration)
    {
        float startRadius = innerLight.pointLightOuterRadius;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            innerLight.pointLightOuterRadius = Mathf.Lerp(startRadius, targetRadius, elapsedTime / duration);
            yield return null;
        }

        innerLight.pointLightOuterRadius = targetRadius;
    }
}
