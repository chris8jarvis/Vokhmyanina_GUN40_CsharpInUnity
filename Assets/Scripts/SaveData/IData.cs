using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{
    public interface IData
    {
        void Save(GameData data);
        GameData Load();
        bool HasSavedData();
    }
}
