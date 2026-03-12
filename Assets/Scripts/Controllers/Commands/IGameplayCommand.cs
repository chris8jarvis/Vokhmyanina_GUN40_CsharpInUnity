using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public interface IGameplayCommand
    {
        void Interact(Cell cell);
    }
}