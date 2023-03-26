using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelContexMenu 
{
    [MenuItem("Assets/Set Level as Current")]
    public static void SetAsCurrent()
    {
        LevelPrefs selection = Selection.activeGameObject.GetComponent<LevelPrefs>();

        Object.FindObjectOfType<GameManager>().CurrentLevel = selection;
    }

    [MenuItem("Assets/Set Level as Current", true)]
    public static bool SetAsCurrentValidator()
    {
        return Selection.activeGameObject?.GetComponent<LevelPrefs>() != null;
    }
}
