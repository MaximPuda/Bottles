using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameMenu : EditorWindow
{
    [MenuItem("Game/Take Screenshot")]
    public static void TakeScreenshot()
    {
        string path = "Screenshots";

        Directory.CreateDirectory(path);

        int i = 0;
        while (File.Exists(path + "/" + i + ".png"))
        {
            i++;
        }

        ScreenCapture.CaptureScreenshot(path + "/" + i + ".png");
    }

    [MenuItem("Game/Delete Save File")]
    public static void DeleteSaveFile()
    {
        SaveSystem.DeleteSave();
    }

    [MenuItem("Game/Apply all items")]
    public static void ApplyAllItems()
    {
        ItemController[] items = FindObjectsOfType<ItemController>();
        foreach (var item in items)
        {
            item.ApplyStartSetting();
        }
    }
}
