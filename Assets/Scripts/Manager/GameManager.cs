using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Config")]
    [SerializeField] private Player player;
    
    [SerializeField] private AudioClip startingBGM; // Background music to play at start

    private void Start()
    {
        if (startingBGM != null)
        {
            AudioManager.Instance.PlayBGM(startingBGM);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            player.ResetPlayer();
        }
    }
}
