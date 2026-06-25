using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public RaycastWeapon weapon;
    public KeyCode shootKey = KeyCode.E;
    
    void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        if (weapon != null)
        {
            // Стреляем в центр экрана
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
            RaycastHit hit;
            Vector3 target;
            
            if (Physics.Raycast(ray, out hit, 100f))
            {
                target = hit.point;
            }
            else
            {
                target = ray.GetPoint(100f);
            }
            
            weapon.StartFiring();
            weapon.UpdateWeapon(Time.deltaTime, target);
            weapon.StopFiring();
            
            Debug.Log("Выстрел по клавише " + shootKey);
        }
    }
}
