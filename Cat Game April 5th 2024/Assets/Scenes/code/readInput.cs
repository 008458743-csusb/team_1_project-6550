using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class ReadInput : MonoBehaviour
{
    private string input;
    private string deviceID;

    void Awake()
    {
        deviceID = SystemInfo.deviceUniqueIdentifier;
    }

    public void ReadStringInput(string s)
    {
        input = s;
        Debug.Log("User input name: " + input);
        StartCoroutine(HandleInput());
    }

    IEnumerator HandleInput()
    {
        yield return WriteToFile();
        Debug.Log("After WriteToFile");
    }

    private async Task WriteToFile()
    {
        string currentDirectory = Application.dataPath;
        string path = Path.Combine(currentDirectory, "userProfile.txt");
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine(deviceID + "," + input);
        }
        Debug.Log("File written with Device ID and User Input.");

        await SendDataToAPI();
    }

    private async Task SendDataToAPI()
    {
        string code = "tAyYdMOMtdfKtuFfjrEpaO_bsqRM6JcCtDGpB3VRFV6OAzFujEw6fw==";
        string url = $"https://test1-mathgame.azurewebsites.net/api/game/create?code={code}&device_id={deviceID}&username={input}";
        
        using (HttpClient client = new HttpClient())
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                Debug.Log("Progress data sent: " + responseString);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to send progress data: {ex.Message}");
            }
        }
    }
}