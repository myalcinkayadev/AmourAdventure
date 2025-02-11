using UnityEngine;

public class BoundarySoundTrigger : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip crossingSound;

    [SerializeField] private AudioClip startingBGM; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayCrossingSound();
        }
    }

    private void PlayCrossingSound()
    {
        Debug.Log("PlayCrossingSound");
        if (crossingSound == null) {
            Debug.LogWarning("BoundarySoundTrigger: CrossingSound is not assigned.");
            return;
        }

       AudioManager.Instance.PlayBGM(crossingSound);
    }
}
