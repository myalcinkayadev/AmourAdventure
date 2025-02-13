using System.Collections;
using UnityEngine;

public class FlowerInteraction : MonoBehaviour
{
    [SerializeField] private ParticleSystem sparkleEffect;
    [SerializeField] private AudioClip bloomSound;

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
            AudioManager.Instance.PlaySFX(bloomSound, 1, 1);
        }

        yield return new WaitForSeconds(0.5f);

        sparkleEffect.Stop();

        isTouched = false;
    }
}
