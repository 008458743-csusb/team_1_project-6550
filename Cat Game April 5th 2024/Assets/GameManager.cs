using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import SceneManager to handle scene changes
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;
using System.IO; // Added for file I/O
using ClassLibrary1;
using System.Security.Cryptography;
using System.Net.Http;
using System.Threading.Tasks;
using Pcg;

public class MathGame : MonoBehaviour
{
    public Text questionText;
    public Button[] answerButtons;
    public Text questionCounterText;
    public GameObject Blurbackground;
    public GameObject pauseMenu; // Add reference to the pause menu panel
    public GameObject correctAnswerPrompt;
    public GameObject wrongAnswerPrompt;
    public GameObject problemQuestionCanvas;
    public GameObject[] catUnits;
    public Vector3[] startPositions; // Animation Functionality (off screen position)
    public Vector3[] targetPositions;
    public float speed = 0f; // Animation Functionality: Speed at which the cat moves
    public GameObject catContainer; // Animation Functionality: Assign this in the Inspector
    public AudioSource correctAnswerSound;
    public AudioSource wrongAnswerSound;
    public GameObject[] allCats;
    public GameObject HappyCat_0;
    public GameObject SadCat_1;


    private Button correctButton;
    private Color defaultButtonColor = Color.white; // The default color for buttons
    private Color correctButtonColor = Color.green; // The color for correct answers
    private Color incorrectButtonColor = Color.red; // The color for incorrect answers
    private Vector3 offScreenPosition = new Vector3(-10f, 0f, 0f); // Animation Functionality: Example off-screen position

    private int questionCounter = 0;
    private bool quizCompleted = false;
    private bool gamePaused = false; // Add variable to track game pause state
    private int correctAnswers = 0;
    private int totalQuestions = 5;
    private float rate;
    private float accuracy;
    private float startTime;
    private float endTime;
    private float totalTime;
    private string userName;
    private bool buttonsRespondingToInput = true; // Flag to track whether buttons should respond to input
    private PcgRandom randomGenerator;

    void Start()
    {
        randomGenerator = new PcgRandom(); // Initialize the PcgRandom generator 
        //startTime = Time.time;
        //InitializeCatsAtStartPositions();
        InitializeCats();
        GenerateQuestion();
    }



    /* IEnumerator DelayBeforeNextQuestion()
    {
         yield return   new WaitForSeconds(0f); // Adjust the delay time as needed
         // Generate the next question after the delay if the quiz is not completed and the game is not paused
         if (!quizCompleted && !gamePaused)
         {
             GenerateQuestion();
         }

     } */
    void DisplayCatsForQuestion(int num1, int num2)
    {
        // Deactivate all cats initially
        foreach (GameObject cat in catUnits)
        {
            cat.SetActive(false);
        }

        // Calculate the index offsets for each row
        int row1Offset = 0;       // Start of the first row
        int row2Offset = 4;       // Start of the second row
        int row3Offset = 8;       // Start of the third row

        // We need to distribute num1 over the first two rows (8 cats max)
        int num1FirstRow = Mathf.Min(num1, 4);  // Cats in the first row
        int num1SecondRow = Mathf.Max(0, num1 - 4);  // Remaining cats in the second row

        // Activate cats based on num1 for the first row
        for (int i = 0; i < num1FirstRow; i++)
        {
            catUnits[row1Offset + i].SetActive(true);
        }

        // Activate cats based on remaining num1 for the second row
        for (int i = 0; i < num1SecondRow; i++)
        {
            catUnits[row2Offset + i].SetActive(true);
        }

        // Activate cats based on num2 for the third row
        for (int i = 0; i < num2 && i < 4; i++)
        {
            catUnits[row3Offset + i].SetActive(true);
        }
    }

    void GenerateQuestion()
    {

        if (!quizCompleted && !gamePaused) // Check if the quiz is not completed and the game is not paused
        {
            HappyCat_0.SetActive(false);
            SadCat_1.SetActive(false);
            //ResetCatPositionsAndAnimations(); // Animation functionality 
            // Increment question counter
            questionCounter++;

            // Generate random numbers for the addition question
            int num1 = randomGenerator.Next(1, 6); // Generates a random number between 1 and 5 
            int num2 = randomGenerator.Next(1, 3); // Generates a random number between 1 and 2

            
            //int num1 = UnityEngine.Random.Range(1, 6); // Change the range as per your requirement
            //int num2 = UnityEngine.Random.Range(1, 3);

            //int answer = num1 + num2;
            BasicMathsFunctions math = new BasicMathsFunctions(); // Instantiate BasicMathsFunctions from NuGet package
            int answer = (int)math.Addition(num1, num2); // Call the Addition method and cast the result to int

            //DisplayCatsForQuestion(num1, num2);
            ActivateCats(num1, num2);

            if (questionCounter <= totalQuestions)
            {
                questionCounterText.text = $"{questionCounter} / {totalQuestions}";
                // Display the question
                questionText.text = num1 + " + " + num2 + "= ?";
            }
            else
            {
                /*questionCounterText.text = ""; // Hide question counter after 3 questions
                questionText.text = ""; //Hide question text after 3 questions*/
                quizCompleted = true;
                LoadShowScoreScene();// Load the "showScore" scene
            }

            // Reset the flag to allow button input for the new question
            SetButtonsRespondingToInput(true);

            // List to store wrong answers
            List<int> wrongAnswers = new List<int>();

            // Generate random answer options
            int correctButtonIndex = UnityEngine.Random.Range(0, answerButtons.Length);

            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (questionCounter <= totalQuestions)
                {
                    answerButtons[i].gameObject.SetActive(true); // Show answer buttons for first 3 questions
                }
                /*else
                {
                    answerButtons[i].gameObject.SetActive(false); // Hide answer buttons after 3 questions
                }*/

                if (i == correctButtonIndex)
                {
                    answerButtons[i].GetComponentInChildren<Text>().text = answer.ToString();
                    answerButtons[i].onClick.RemoveAllListeners(); // Remove previous listeners
                    correctButton = answerButtons[i];
                    answerButtons[i].onClick.AddListener(CorrectAnswer);
                }
                else
                {
                    int wrongAnswer = UnityEngine.Random.Range(answer / 2, answer + 3); // Change the range as per your requirement
                    while (wrongAnswers.Contains(wrongAnswer) || wrongAnswer == answer)
                    {
                        wrongAnswer = UnityEngine.Random.Range(answer / 2, answer + 3);
                    }
                    wrongAnswers.Add(wrongAnswer); // Add wrong answer to the list
                    answerButtons[i].GetComponentInChildren<Text>().text = wrongAnswer.ToString();
                    answerButtons[i].onClick.RemoveAllListeners(); // Remove previous listeners
                    answerButtons[i].onClick.AddListener(WrongAnswer);
                }
            }

            if (questionCounter > totalQuestions)
            {
                quizCompleted = true;
                endTime = Time.time; // Record the end time when the quiz is completed
                totalTime = endTime - startTime; // Calculate the total time taken
                accuracy = ((float)correctAnswers / totalQuestions) * 100;
                accuracy = Mathf.Round(accuracy * 100) / 100; // Round accuracy to two decimal places
                rate = (totalQuestions / totalTime) * 60f;

                string currentDirectory = Application.persistentDataPath; // Assumes the code file is in the "Assets" directory
                string filePath = Path.Combine(currentDirectory, "showScore.txt");
                Debug.Log($"File Path: {filePath}");

                string userProfilePath = Path.Combine(currentDirectory, "userProfile.txt");
                string deviceID = SystemInfo.deviceUniqueIdentifier;
                GetUserName(userProfilePath, deviceID);

                string csvContent = $"{totalQuestions},{correctAnswers},{accuracy:F2},{rate:F2}";

                try
                {
                    File.WriteAllText(filePath, csvContent);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error writing to file: {e.Message}");
                }

                // Code for user progress - written by Manish.

                string currentDirectory1 = Application.persistentDataPath; // Assumes the code file is in the "Assets" directory
                string filePath1 = Path.Combine(currentDirectory1, "userProgress.txt");
                Debug.Log($"File Path: {filePath1}");

                Debug.Log("Device ID: " + deviceID);

                string csvContent1 = $"{deviceID},{userName},{totalQuestions},{correctAnswers},{accuracy:F2},{rate:F2}\n"; // Add newline character

                try
                {
                    File.AppendAllText(filePath1, csvContent1); // Appends text to the file, creating the file if it doesn't already exist.
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error writing to file: {e.Message}");
                }

                SendProgressData();

                //LoadShowScoreScene(); // Load the "showScore" scene
                //Moving it out of if block
            }
        }
    }

    private async Task SendProgressData()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        string code = "tAyYdMOMtdfKtuFfjrEpaO_bsqRM6JcCtDGpB3VRFV6OAzFujEw6fw==";
        string url = $"https://test1-mathgame.azurewebsites.net/api/game/progress?code={code}&device_id={deviceID}&username={userName}&questions={totalQuestions}&correct_answers={correctAnswers}&accuracy={accuracy:F2}&rate={rate:F2}";

        using (HttpClient client = new HttpClient())
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                Debug.Log("Progress data sent: " + responseString);
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                Debug.LogError($"Failed to send progress data: {response.StatusCode} - {errorResponse}");
            }
        }
    }

    public void LoadShowScoreScene()
    {
        SceneManager.LoadScene("showScore"); // Load the scene with the name "showScore"
    }

    void CorrectAnswer()
    {
        if (!buttonsRespondingToInput)
            return;
        Debug.Log("Correct!");
        correctAnswers++;
        HighlightButton(correctButton, correctButtonColor);
        StartCoroutine(ShowPrompt(correctAnswerPrompt));
        correctAnswerSound.Play();
        buttonsRespondingToInput = false; // Disable further button input
        HappyCat_0.SetActive(true);
    }

    void WrongAnswer()
    {
        if (!buttonsRespondingToInput)
            return;
        Debug.Log("Wrong!");
        HighlightButton(correctButton, correctButtonColor);
        Button incorrectButton = (Button)UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        HighlightButton(incorrectButton, incorrectButtonColor);
        StartCoroutine(ShowPrompt(wrongAnswerPrompt));
        wrongAnswerSound.Play();
        buttonsRespondingToInput = false; // Disable further button input
        SadCat_1.SetActive(true);
    }

    // Helper method to enable/disable button input
    void SetButtonsRespondingToInput(bool responding)
    {
        buttonsRespondingToInput = responding;
    }

    void HighlightButton(Button button, Color color)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
            StartCoroutine(ResetButtonColor(buttonImage));
        }
    }

    IEnumerator ResetButtonColor(Image buttonImage)
    {
        if (quizCompleted == true)
        {
            yield return new WaitForSeconds(0.0f);
        }
        yield return new WaitForSeconds(1.0f); // Adjust the delay time as needed
        buttonImage.color = defaultButtonColor;
    }

    // Helper method to set interactable state of an array of buttons
    private void SetButtonsInteractable(Button[] buttons, bool interactable)
    {
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        pauseMenu.SetActive(true);
        problemQuestionCanvas.SetActive(false);
        Time.timeScale = 0f; // Effectively pauses the game
        SetButtonsInteractable(answerButtons, false);
        Blurbackground.SetActive(true);

        // Deactivate cats directly in this method
        //foreach (GameObject cat in catUnits)
        foreach (GameObject cat in allCats)
        {
            // Directly disable the SpriteRenderer component
            SpriteRenderer catSprite = cat.GetComponent<SpriteRenderer>();
            if (catSprite != null)
            {
                catSprite.enabled = false;
            }

            // Pause animations by setting animator speed to 0
            Animator catAnimator = cat.GetComponent<Animator>();
            if (catAnimator != null)
            {
                catAnimator.speed = 0;
            }
        }
        Time.timeScale = 0f; // added for animation functionality 
    }


    public void ResumeGame()
    {
        gamePaused = false;
        pauseMenu.SetActive(false);
        problemQuestionCanvas.SetActive(true);
        Time.timeScale = 1f;
        ReactivateCats();
        SetButtonsInteractable(answerButtons, true);
        Blurbackground.SetActive(false);
    }

    void ReactivateCats()
    {
        //foreach (GameObject cat in catUnits)
        foreach (GameObject cat in allCats)
        {
            // Re-enable the SpriteRenderer component
            SpriteRenderer catSprite = cat.GetComponent<SpriteRenderer>();
            if (catSprite != null)
            {
                catSprite.enabled = true;
            }

            // Resume animations if they were paused
            Animator catAnimator = cat.GetComponent<Animator>();
            if (catAnimator != null)
            {
                catAnimator.speed = 1;
            }
        }
    }



    public void RestartGame()
    {
        questionCounter = 0;
        quizCompleted = false;
        gamePaused = false;
        pauseMenu.SetActive(false);
        problemQuestionCanvas.SetActive(true);
        Time.timeScale = 1f;
        Blurbackground.SetActive(false);

        //foreach (GameObject cat in catUnits)
        foreach (GameObject cat in allCats)
        {
            cat.SetActive(false); // Ensure cats start from a deactivated state for consistency
        }

        ReactivateCats(); // Ensure visual components are active before they're needed
        SetButtonsInteractable(answerButtons, true);
        GenerateQuestion(); // This should include activating the necessary cats
    }

    public void Quit()
    {
        gamePaused = false;
        pauseMenu.SetActive(false); // Hide the pause menu panel
        Time.timeScale = 1f; // Restore normal time flow

        SceneManager.LoadScene(0); // Load the main menu scene
    }
    IEnumerator ShowPrompt(GameObject prompt)
    {
        if (quizCompleted == true)
        {
            yield return new WaitForSeconds(0.0f);
        }
        prompt.SetActive(true);
        yield return new WaitForSeconds(1.0f); // Wait for 2 seconds
        prompt.SetActive(false);
        GenerateQuestion();
        //StartCoroutine(DelayBeforeNextQuestion()); // Call DelayBeforeNextQuestion after the delay
    }




    // Added animation functionality starts here 
    void Update()
    {
        for (int i = 0; i < catUnits.Length; i++)
        {
            GameObject cat = catUnits[i];
            if (!gamePaused && !quizCompleted && cat.activeSelf)
            {
                MoveCatToTarget(i);
            }
        }
    }

    public void MoveCatToTarget(int catIndex)
    {
        if (catIndex < 0 || catIndex >= catUnits.Length || catIndex >= targetPositions.Length)
        {
            Debug.LogError("Index out of range.", this);
            return;
        }

        GameObject cat = catUnits[catIndex];
        Vector3 targetPos = targetPositions[catIndex];
        cat.transform.position = Vector3.MoveTowards(cat.transform.position, targetPos, speed * Time.deltaTime);

        Animator animator = cat.GetComponent<Animator>();
        if (animator != null)
        {
            bool hasReachedTarget = cat.transform.position == targetPos;
            animator.SetBool("IsWalking", !hasReachedTarget);
        }
    }

    void ResetCatPositionsAndAnimations()
    {
        for (int i = 0; i < catUnits.Length; i++)
        {
            if (i < startPositions.Length)
            {
                catUnits[i].transform.position = startPositions[i];
                Animator animator = catUnits[i].GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool("IsWalking", false);
                }
            }
        }
    }

    public void MakeCatsWalkOff()
    {
        StartCoroutine(MoveCatsOffScreen());
    }

    private IEnumerator MoveCatsOffScreen()
    {
        foreach (GameObject cat in catUnits)
        {
            if (cat.activeSelf)
            {
                yield return MoveCatOffScreen(cat.transform);
            }
        }
    }

    private IEnumerator MoveCatOffScreen(Transform catTransform)
    {
        Vector3 offScreenPosition = new Vector3(-10f, 0f, 0f);
        while (catTransform.position.x != offScreenPosition.x)
        {
            catTransform.position = Vector3.MoveTowards(catTransform.position, offScreenPosition, speed * Time.deltaTime);
            yield return null;
        }
        catTransform.gameObject.SetActive(false);
    }
    void InitializeCatsAtStartPositions()
    {
        for (int i = 0; i < catUnits.Length; i++)
        {
            if (i < startPositions.Length)
            {
                catUnits[i].transform.position = startPositions[i];
            }
            else
            {
                Debug.LogWarning($"No start position for cat at index {i}, using cat's current position.");
            }
        }
    }

    void GetUserName(string filePath, string searchDeviceID)
    {
        Debug.Log("Inside getScore");
        // Initialize userName as "null" to ensure it has a value even if the file doesn't exist or the ID isn't found
        userName = "null";

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            Debug.Log("Lines from userProfile: " + lines);

            // Iterate through the file from the end using a reverse for loop
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                string line = lines[i];
                // Split the data by commas
                string[] data = line.Split(',');
                if (data.Length == 2 && data[0].Trim() == searchDeviceID)
                {
                    // If the deviceID matches, set the userName variable
                    userName = data[1].Trim();
                    break; // Exit the loop after finding the match
                }
            }

            Debug.Log("User name found: " + userName);
        }
        else
        {
            Debug.LogError("The 'userProfile.txt' file does not exist.");
        }

        // At this point, userName is either the name found in the file or "null"
    }
    void InitializeCats()
    {
        foreach (GameObject cat in allCats)
        {
            cat.SetActive(false); // Start with all cats deactivated
        }
    }
    public void UpdateCatSpawnBasedOnQuestion(string questionText)
    {
        // Example questionText: "6 + 3 = ?"
        string[] parts = questionText.Split(' ');
        int firstNumber = int.Parse(parts[0]); // This is '6' from the example
        int secondNumber = int.Parse(parts[2]); // This is '3' from the example

        ActivateCats(firstNumber, secondNumber);
    }
    void ActivateCats(int countTop, int countBottom)
    {
        InitializeCats(); // Deactivate all cats to start fresh each time

        // Activate the top row cats for the first number
        for (int i = 0; i < countTop && i < 6; i++) // Ensure it does not exceed the top row count
        {
            allCats[i].SetActive(true);
        }

        // Activate the bottom row cats for the second number
        int startBottomIndex = 6; // Start from CatAnimation_0 (6)
        for (int i = 0; i < countBottom && i < 3; i++) // Ensure it does not exceed the bottom row count
        {
            int index = startBottomIndex + i;
            if (index < allCats.Length)
            {
                allCats[index].SetActive(true); // Activates CatAnimation_0 (6) to CatAnimation_0 (8)
            }
        }
    }
}