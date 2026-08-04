using UnityEngine;
using SaveData;

public static class SaveManager
{
    private static IData _dataHandler = new BinarySerializationData();

    // private static IData _dataHandler = new XMLData();

    public static void SaveGame(GameData data)
    {
        if (_dataHandler != null)
        {
            _dataHandler.Save(data);
        }
        else
        {
            Debug.LogError("[SaveManager] No data handler assigned");
        }
    }

    public static GameData LoadGame()
    {
        if (_dataHandler != null)
        {
            return _dataHandler.Load();
        }
        else
        {
            Debug.LogError("[SaveManager] No data handler assigned");
            return null;
        }
    }

    public static bool HasSavedData()
    {
        if (_dataHandler != null)
        {
            return _dataHandler.HasSavedData();
        }
        return false;
    }
}
