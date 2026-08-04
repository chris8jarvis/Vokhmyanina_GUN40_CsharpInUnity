using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace SaveData
{
    public class XMLData : IData
    {
        private string _savePath;

        public XMLData()
        {
            _savePath = Application.persistentDataPath + "/game.xml";
        }

        public void Save(GameData data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                using (FileStream stream = new FileStream(_savePath, FileMode.Create))
                {
                    serializer.Serialize(stream, data);
                    Debug.Log($"[XMLData] Game saved to: {_savePath}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[XMLData] Failed to save: {e.Message}");
            }
        }

        public GameData Load()
        {
            if (!File.Exists(_savePath))
            {
                Debug.Log("[XMLData] No save file found.");
                return null;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));
                using (FileStream stream = new FileStream(_savePath, FileMode.Open))
                {
                    GameData data = serializer.Deserialize(stream) as GameData;
                    Debug.Log($"[XMLData] Game loaded from: {_savePath}");
                    return data;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[XMLData] Failed to load: {e.Message}");
                return null;
            }
        }

        public bool HasSavedData()
        {
            return File.Exists(_savePath);
        }
    }
}
