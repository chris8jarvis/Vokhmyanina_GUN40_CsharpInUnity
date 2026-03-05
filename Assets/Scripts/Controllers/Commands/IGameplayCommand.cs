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

// public class IGameplayCommand : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
