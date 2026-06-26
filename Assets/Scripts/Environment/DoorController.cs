using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Настройки двери")]
    public float openAngle = -90f;
    public float openSpeed = 2f;
    public bool isLocked = true;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        if (!isMoving) return;

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            transform.rotation = targetRotation;
            isMoving = false;
        }
    }

    public void OpenDoor()
    {
        if (isLocked)
        {
            Debug.Log("Дверь заперта!");
            return;
        }

        if (!isOpen)
        {
            isOpen = true;
            isMoving = true;
            Debug.Log("Дверь открывается.");
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            isMoving = true;
            Debug.Log("Дверь закрывается.");
        }
    }
}
