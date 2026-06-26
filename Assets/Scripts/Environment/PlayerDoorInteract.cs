using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoorInteract : MonoBehaviour
{
    public float interactRange = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                DoorController door = hit.collider.GetComponent<DoorController>();
                if (door != null)
                {
                    door.OpenDoor();
                }
            }
        }
    }
}
