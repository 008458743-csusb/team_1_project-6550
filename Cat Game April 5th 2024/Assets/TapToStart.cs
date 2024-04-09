using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameButton : MonoBehaviour
{
    public void LoadGameScene()
    {
        // Load the "game" scene when the button is tapped
        SceneManager.LoadScene("game");
    }
}
