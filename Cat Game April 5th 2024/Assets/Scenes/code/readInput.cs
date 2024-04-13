using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

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

    private void WriteToFile()
    {
        // Path to the file
        string currentDirectory = Application.dataPath; // Assumes the code file is in the "Assets" directory
        string path = Path.Combine(currentDirectory, "userProfile.txt");
        Debug.Log($"File Path: {path}");

        // Create a file to write to or append if it already exists
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine(deviceID + "," + input);
        }

        // Log to debug that the file has been written
        Debug.Log("File written with Device ID and User Input.");
    }
}