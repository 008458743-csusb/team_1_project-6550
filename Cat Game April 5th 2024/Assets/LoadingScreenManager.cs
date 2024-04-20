using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public Text countdownText;
    public float countdownDuration = 3f;
    public GameObject catGraphic;
    public GameObject coloredSquare;
    public GameObject[] additionSymbols;

    private Vector3 originalCatPosition;
    private Quaternion originalCatRotation;
    private Vector3[] originalSymbolPositions;

    void Start()
    {
        
        originalCatPosition = catGraphic.transform.position;
        originalCatRotation = catGraphic.transform.rotation;
        originalSymbolPositions = new Vector3[additionSymbols.Length];
        for (int i = 0; i < additionSymbols.Length; i++)
        {
            originalSymbolPositions[i] = additionSymbols[i].transform.position;
        }
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        float timer = countdownDuration;

        while (timer > 0f)
        {
            countdownText.text = Mathf.CeilToInt(timer).ToString();
            UpdateCatPosition();
            UpdateAdditionSymbols();
            UpdateColoredSquareColor(timer);
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        TransitionToNextScene();
    }

    void UpdateCatPosition()
    {
        float swayAmount = 50f;
        float swaySpeed = 2.5f;
        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        float swayY = Mathf.Sin(Time.time * swaySpeed * 0.7f) * swayAmount;
        catGraphic.transform.position = originalCatPosition + new Vector3(swayX, swayY, 0f);

        float tiltAmount = 10f;
        float tiltSpeed = 2f;
        float tiltAngle = Mathf.Sin(Time.time * tiltSpeed) * tiltAmount;
        catGraphic.transform.rotation = originalCatRotation * Quaternion.Euler(0f, 0f, tiltAngle);
    }

    void UpdateAdditionSymbols()
    {
        float swayAmount = 50f;
        float swaySpeed = 2f;

        // Swaying up and down for the first symbol
        float firstSwayY = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        additionSymbols[0].transform.position = originalSymbolPositions[0] + new Vector3(0f, firstSwayY, 0f);

        // Swaying left and right for the second symbol
        float secondSwayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        additionSymbols[1].transform.position = originalSymbolPositions[1] + new Vector3(secondSwayX, 0f, 0f);

        // Moving in a circle for the third symbol
        float circleRadius = 10f;
        float circleSpeed = 3f;
        float angle = Time.time * circleSpeed;
        float circleX = Mathf.Cos(angle) * circleRadius;
        float circleY = Mathf.Sin(angle) * circleRadius;
        additionSymbols[2].transform.position = originalSymbolPositions[2] + new Vector3(circleX, circleY, 0f);
    }

    void UpdateColoredSquareColor(float timeRemaining)
    {
        float colorChangeDuration = 1f;
        float progress = 1f - (timeRemaining / countdownDuration);
        float hue = Mathf.Lerp(240, 360, progress); // Interpolate between blue (240) and red (360)
        coloredSquare.GetComponent<Image>().color = Color.HSVToRGB(hue / 360f, 1f, 1f);
    }

    void TransitionToNextScene()
    {
        SceneManager.LoadScene("main");
    }
}