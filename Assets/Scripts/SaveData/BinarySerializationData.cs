using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveData
{
    public class BinarySerializationData : IData
    {
        private string _savePath;

        public BinarySerializationData()
        {
            _savePath = Application.persistentDataPath + "/game.binary";
        }

        public void Save(GameData data)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(_savePath, FileMode.Create))
                {
                    formatter.Serialize(stream, data);
                    Debug.Log($"[BinarySerialization] Game saved to: {_savePath}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[BinarySerialization] Failed to save: {e.Message}");
            }
        }

        public GameData Load()
        {
            if (!File.Exists(_savePath))
            {
                Debug.Log("[BinarySerialization] No save file found.");
                return null;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(_savePath, FileMode.Open))
                {
                    GameData data = formatter.Deserialize(stream) as GameData;
                    Debug.Log($"[BinarySerialization] Game loaded from: {_savePath}");
                    return data;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[BinarySerialization] Failed to load: {e.Message}");
                return null;
            }
        }

        public bool HasSavedData()
        {
            return File.Exists(_savePath);
        }
    }
}
