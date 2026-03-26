using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Commands
{
    public interface IGameplayCommand
    {
        CommandType Type { get; }
        bool TryInteract(Cell cell, Unit selectedUnit);
    }
}