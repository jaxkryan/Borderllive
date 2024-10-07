using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class Logger
{
    private static string logFilePath = "log.txt";

    public static void Log(string message)
    {
        File.AppendAllText(logFilePath, message + "\n");
    }
}