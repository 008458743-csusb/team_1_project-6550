using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class DisplayLastFiveScores : MonoBehaviour
{
    public TextMeshProUGUI scoreTableText;
    public TextMeshProUGUI showName;
    public TextMeshProUGUI showCorrectAnswers;
    public TextMeshProUGUI showAccuracy;
    public TextMeshProUGUI showRate;
    private string userName;


    private void Start()
    {
        DisplayScores();
    }

    private void DisplayScores()
    {
        string filePath = Path.Combine(Application.dataPath, "userProgress.txt");

        if (File.Exists(filePath))
        {
            string deviceID = SystemInfo.deviceUniqueIdentifier;
            string currentDirectory = Application.dataPath;
            string userProfilePath = Path.Combine(currentDirectory, "userProfile.txt");
            GetUserName(userProfilePath, deviceID);

            var lines = File.ReadAllLines(filePath);

            var matchingData = lines
            .Select(line => line.Split(','))
            .Where(data => data.Length > 2 && data[0].Trim() == deviceID && data[1].Trim() == userName)
            .Reverse() // Reverse to get the last entries first
            .Take(5) // Take only the last 5 entries
            .Reverse() // Reverse again to display them in the original order
            .ToList();

            showName.text = userName;

            if (matchingData.Any())
            {
                showCorrectAnswers.text = "";
                showAccuracy.text = "";
                showRate.text = "";

                foreach (var record in matchingData)
                {
                    // scoreTableText.text += $"{record[2]} | {record[3]}% | {record[4]}/min\n";
                    Debug.Log("records: "+record[3]);
                    showCorrectAnswers.text += $"{record[3]}\n";
                    showAccuracy.text += $"{record[4]}%\n";
                    showRate.text += $"{record[5]}/min\n";
                }
            }
            else
            {
                Debug.LogWarning("No records found for the device ID in 'userProgress.txt'.");
                scoreTableText.text = "Error: No data found";
            }
        }
        else
        {
            Debug.LogWarning("File not found.");
            scoreTableText.text = "Error: File not found";
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

            Debug.Log("User name found in getProgress: "+userName);
        }
        else
        {
            Debug.LogError("The 'userProfile.txt' file does not exist.");
        }

        // At this point, userName is either the name found in the file or "null"
    }
}