using System.Collections;
using UnityEngine;

public class FlowerInteraction : MonoBehaviour
{
    [SerializeField] private ParticleSystem sparkleEffect;
    [SerializeField] private AudioClip bloomSound;

    [SerializeField] private float minPitch = 0.6f;  // Lower limit (slightly deep sound)
    [SerializeField] private float maxPitch = 1.2f;  // Upper limit (slightly high sound)

    private bool isTouched = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTouched)
        {
            StartCoroutine(TriggerFlowerEffect());
        }
    }

    private IEnumerator TriggerFlowerEffect()
    {
        sparkleEffect.Play();

        if (bloomSound != null) {
            float pitch = Random.Range(minPitch, maxPitch);
            AudioManager.Instance.PlaySFX(bloomSound, 1, pitch);
        }

        yield return new WaitForSeconds(0.5f);

        sparkleEffect.Stop();

        isTouched = false;
    }
}
