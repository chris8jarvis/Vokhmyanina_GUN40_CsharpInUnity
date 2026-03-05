using UnityEngine;
public enum NeighbourType
{
    None = 0,           // Нет соседей (одиночная клетка)
    Left = 1,           // Слева есть клетка
    Right = 2,          // Справа есть клетка
    Top = 3,            // Сверху есть клетка
    Bottom = 4,         // Снизу есть клетка
    LeftRight = 5,      // Слева и справа
    TopBottom = 6,      // Сверху и снизу
    Cross = 7,          // Все четыре стороны
}


// public enum Team
// {
//     Player1 = 0,
//     Player2 = 1
// }

public enum Player
{
    White,
    Black
}

public enum UnitType
{
    Man,    // Простая шашка
    King    // Дамка
}

public enum GameState
{
    SelectUnit,     // Выбираем фигуру
    SelectDestination, // Выбираем клетку для хода
    EnemyTurn       // Ход противника
}