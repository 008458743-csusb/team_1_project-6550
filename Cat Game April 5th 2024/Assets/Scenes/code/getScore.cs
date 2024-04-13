using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class getScore : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI showScore;
    private string userName;
    void Start()
    {
        Debug.Log("Inside getScore");

        if (showScore != null)
        {
            // Read the contents of the "output.txt" file
            string currentDirectory = Application.dataPath;
            string filePath = Path.Combine(currentDirectory, "showScore.txt");
            string deviceID = SystemInfo.deviceUniqueIdentifier;
            Debug.Log("Device ID: " + deviceID);

            string userProfilePath = Path.Combine(currentDirectory, "userProfile.txt");
            GetUserName(userProfilePath, deviceID);

            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);

                // Split the data by commas
                string[] data = fileContent.Split(',');

                // Check if there are enough elements in the array
                if (data.Length >= 4)
                {
                    int totalQuestions = int.Parse(data[0]);
                    int correctAnswersCount = int.Parse(data[1]);
                    float accuracy = float.Parse(data[2]);
                    float rate = float.Parse(data[3]);

                    // Display the data in the TextMeshProUGUI component
                    showScore.text = $"Total Questions: {totalQuestions}\nCorrect Answers: {correctAnswersCount}\nAccuracy: {accuracy}%\nRate: {rate:F2}/min";
                }
                else
                {
                    Debug.LogError("Invalid data format in the 'output.txt' file.");
                    showScore.text = "Error: Invalid data format";
                }
            }
            else
            {
                Debug.LogError("The 'output.txt' file does not exist.");
                showScore.text = "Error: File not found";
            }
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is not assigned to the 'print' variable.");
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
            Debug.Log("Lines from userProfile: "+lines);

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

            Debug.Log("User name found: "+userName);
        }
        else
        {
            Debug.LogError("The 'userProfile.txt' file does not exist.");
        }

        // At this point, userName is either the name found in the file or "null"
    }
}
