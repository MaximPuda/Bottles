using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelContexMenu 
{
    [MenuItem("Assets/Set Level as Current")]
    public static void SetAsCurrent()
    {
        ScriptableObject selection = Selection.activeObject as ScriptableObject;
        Level level = selection as Level;

        Object.FindObjectOfType<GameManager>().CurrentLevel = level;
    }

    [MenuItem("Assets/Set Level as Current", true)]
    public static bool SetAsCurrentValidator()
    {
        ScriptableObject selection = Selection.activeObject as ScriptableObject;
        Level level = selection as Level;
        return level != null;
    }
}
