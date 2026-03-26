using System;
public enum NeighbourType
{
    None   = 0,
    Left   = 1 << 0,
    Right  = 1 << 1,
    Top    = 1 << 2,
    Bottom = 1 << 3,
}

public enum Player
{
    White,
    Black
}

public enum GameState
{
    SelectUnit,
    SelectDestination,
    AttackChain,
    EnemyTurn
}