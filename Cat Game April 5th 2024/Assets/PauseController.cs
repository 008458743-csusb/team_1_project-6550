using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the pause button (e.g., "P" key)
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Stop time
        pauseMenu.SetActive(true); // Show pause menu
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        pauseMenu.SetActive(false); // Hide pause menu
        isPaused = false;
    }

    public void QuitGame()
    {
        // Quit the application (works in standalone builds)
        Application.Quit();
    }
}