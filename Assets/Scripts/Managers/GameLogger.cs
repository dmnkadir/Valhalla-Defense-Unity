using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public static class GameLogger
{
    // --- SAVE PATH ---
    private static string folderPath = Path.Combine(Application.dataPath, "GameLog");
    private static string filePath = Path.Combine(Application.dataPath, "GameLog", "GameLog.txt");

    private static Dictionary<string, int> idCounters = new Dictionary<string, int>();

    public static void InitLog(string mapName, int startLives, int startMoney)
    {
        // Create folder if it doesn't exist
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Reset ID counters
        idCounters.Clear();

        string dateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        string startMessage =
          $"[{dateTime}] --- NEW GAME STARTED ---\n" +
          $"Scenario: '{mapName}' | Starting Lives: {startLives} | Money: {startMoney}\n" +
          $"--------------------------------------------------------------------\n";

        // WRITE FILE FROM SCRATCH (Overwrite Mode)
        // If file exists, it deletes and writes a new one. If it throws an error (file open), it logs to Debug.
        try
        {
            File.WriteAllText(filePath, startMessage);
            Debug.Log($"<color=green>LOG FILE RESET:</color> {filePath}");
        }
        catch (System.Exception e)
        {
            // Even if the file is open, show it in the Unity console at least, don't stay silent.
            Debug.LogError($"LOG FILE COULD NOT BE CREATED! Could the file be open? Error: {e.Message}");
        }
    }

    // ADD LOG
    public static void Write(string message)
    {
        try
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string line = $"[{time}] {message}\n";

            File.AppendAllText(filePath, line);
        }
        catch { }
    }

    // ID generator
    public static string GetNewID(string baseName)
    {
        if (!idCounters.ContainsKey(baseName))
            idCounters[baseName] = 1;
        else
            idCounters[baseName]++;

        return $"{baseName}-ID{idCounters[baseName]:D3}";
    }
}