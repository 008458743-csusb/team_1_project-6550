using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class readInput : MonoBehaviour
{
    private string input;
    private string deviceID;

    // Use Awake for initialization
    void Awake()
    {
        // Now you are safely obtaining the deviceID in the Awake method
        deviceID = SystemInfo.deviceUniqueIdentifier;
    }

    public void ReadStringInput(string s)
    {
        input = s;
        Debug.Log("User input name: " + input);
        WriteToFile();
    }

    private async void WriteToFile()
    {
        // Path to the file
        string currentDirectory = Application.persistentDataPath; // Assumes the code file is in the "Assets" directory
        string path = Path.Combine(currentDirectory, "userProfile.txt");
        Debug.Log($"File Path: {path}");

        // Create a file to write to or append if it already exists
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine(deviceID + "," + input);
        }

        await SendDataToAPI();

        // Log to debug that the file has been written
        Debug.Log("File written with Device ID and User Input.");
    }

    private async Task SendDataToAPI()
    {
        string code = "TIW2z6W5irePx4PwY0CHfxh_JfDnX_4uwWzrdy57jpmNAzFunro6UQ==";
        string url = $"https://test1-mathgame.azurewebsites.net/api/game/create?code={code}&device_id={deviceID}&username={input}";

        using (HttpClient client = new HttpClient())
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            Debug.Log("Progress data sent: " + responseString);

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
}