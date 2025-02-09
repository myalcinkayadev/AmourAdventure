using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("GameScene");
    }

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") > 0.5f) {
            PlayGame();
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            QuitGame();
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
