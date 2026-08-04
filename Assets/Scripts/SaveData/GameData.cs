using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Health;
    public int Score;
    public float PlayerPosX;
    public float PlayerPosY;
    public float PlayerPosZ;

    public GameData() { }

    public GameData(int health, int score, Vector3 playerPos)
    {
        Health = health;
        Score = score;
        PlayerPosX = playerPos.x;
        PlayerPosY = playerPos.y;
        PlayerPosZ = playerPos.z;
    }

    public Vector3 GetPlayerPosition()
    {
        return new Vector3(PlayerPosX, PlayerPosY, PlayerPosZ);
    }
}
