using UnityEngine;

public class BoundarySoundTrigger : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip crossingSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayCrossingSound();
        }
    }

    private void PlayCrossingSound()
    {
        if (crossingSound == null) {
            Debug.LogWarning("BoundarySoundTrigger: CrossingSound is not assigned.");
            return;
        }

       AudioManager.Instance.PlayBGM(crossingSound);
    }
}
