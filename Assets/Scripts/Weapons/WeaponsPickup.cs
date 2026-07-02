using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsPickup : MonoBehaviour
{
    private bool isPickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            PlayerWeaponManager manager = other.GetComponent<PlayerWeaponManager>();
            if (manager != null)
            {
                manager.AddWeapon(gameObject);
                isPickedUp = true;
                gameObject.SetActive(false);
                Debug.Log($"Оружие подобрано: {gameObject.name}");
            }
        }
    }
}
