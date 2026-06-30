using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController door;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.OpenDoor();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.CloseDoor();
        }
    }
}
