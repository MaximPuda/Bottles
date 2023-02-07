using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static bool Save(Save save)
    {
        if (save == null)
        {
            Debug.LogWarning("Save is null!");
            return false;
        }

        string path = Application.persistentDataPath + "Save.sf";
        var formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, save);
        stream.Close();

        return true;
    }

    public static Save Load()
    {
        string path = Application.persistentDataPath + "Save.sf";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        if (stream.Length == 0)
        {
            Debug.LogWarning("Save file doesn't exist or empty " + path);
            return null;
        }

        var formatter = new BinaryFormatter();

        Save save = (Save)formatter.Deserialize(stream);
        stream.Close();

        return save;
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "Save.sf";
        if(File.Exists(path))
        {
            File.Delete(path);
            Debug.LogWarning("Save file deleted!");
        }
        else Debug.LogWarning("Save file doesn't exist or empty " + path);
    }
}
