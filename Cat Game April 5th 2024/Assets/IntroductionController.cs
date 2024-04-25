using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroductionController : MonoBehaviour
{
    public Animator symbolAnimator;
    public Text appNameText;
    public Text devTeamText;
    public float transitionDelay = 2f; // Delay before transitioning to the next scene

    private bool transitionStarted = false;

    void Start()
    {
        // Hide the text initially
        appNameText.enabled = false;
        devTeamText.enabled = false;

        // Subscribe to the animation event to trigger when the symbol animation finishes
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = "ShowText";
        animationEvent.time = symbolAnimator.GetCurrentAnimatorStateInfo(0).length; // Set event time to end of animation
        symbolAnimator.gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].AddEvent(animationEvent);
    }

    // Function to show the text after the symbol animation finishes
    public void ShowText()
    {
        if (!transitionStarted)
        {
            appNameText.enabled = true;
            devTeamText.enabled = true;

            // Start loading the next scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScreen");
            asyncLoad.allowSceneActivation = false; // Prevent automatic scene activation

            // Wait for a delay before transitioning to the next scene
            StartCoroutine(WaitAndTransition(asyncLoad));
        }
    }

    // Coroutine to wait for the delay before transitioning to the next scene
    IEnumerator WaitAndTransition(AsyncOperation asyncLoad)
    {
        transitionStarted = true;
        yield return new WaitForSeconds(transitionDelay);

        // Allow scene activation
        asyncLoad.allowSceneActivation = true;
    }
}