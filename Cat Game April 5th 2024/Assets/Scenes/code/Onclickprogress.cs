using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickProgress : MonoBehaviour
{
    public void LoadProgressScene()
    {
        // Load the "showprogress" scene
        SceneManager.LoadScene("showProgress");
    }
    public void Quit()
    {
        SceneManager.LoadScene("main");
    }
    public void Restart()
    {
        SceneManager.LoadScene("game");
    }
}