using UnityEngine;
using UnityEngine.UI;

public class AdToggleButton : MonoBehaviour
{
    public Button toggleButton; // Reference to the button component

    void Start()
    {
        // Add listener to the button click event
        toggleButton.onClick.AddListener(ToggleAdVisibility);
    }

    void ToggleAdVisibility()
    {
        // Call the toggle function on the AdManager script
        AdManager.Instance.ToggleAdVisibility();
    }
}
