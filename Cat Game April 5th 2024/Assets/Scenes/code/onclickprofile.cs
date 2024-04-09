using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileMenu : MonoBehaviour
{
    public GameObject profileMenuPanel;

    private void Start()
    {
        // Ensure the profile menu panel is initially inactive
        if (profileMenuPanel != null)
        {
            profileMenuPanel.SetActive(false);
        }
    }

    public void OpenProfileMenu()
    {
        // Activate the profile menu panel when the profile button is clicked
        if (profileMenuPanel != null)
        {
            profileMenuPanel.SetActive(true);
        }
    }

    public void ExitToMainScene()
    {
        // Load the "Main" scene when the exit button is clicked
        SceneManager.LoadScene("Main");
    }
}

