using UnityEngine;
using Zenject;
using Models;

public class GameSaver : MonoBehaviour
{
    [Inject] private PlayerModel _playerModel;
    [SerializeField] private Transform _playerTransform;

    private void Start()
    {
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        if (_playerModel == null || _playerTransform == null)
        {
            return;
        }

        GameData data = _playerModel.GetSaveData(_playerTransform.position);
        SaveManager.SaveGame(data);
    }

    public void LoadGame()
    {
        if (_playerModel == null || _playerTransform == null)
        {
            return;
        }

        GameData data = SaveManager.LoadGame();
        if (data != null)
        {
            _playerModel.LoadSaveData(data);
            _playerTransform.position = data.GetPlayerPosition();
        }
    }
}
