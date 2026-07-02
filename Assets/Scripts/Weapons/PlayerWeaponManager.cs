using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("Настройки")]
    public Transform weaponHolder;
    public KeyCode[] weaponKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    private List<GameObject> weapons = new List<GameObject>();
    private int currentWeaponIndex = -1;
    private GameObject currentWeaponInstance;

    void Start()
    {
        foreach (Transform child in weaponHolder)
        {
            RaycastWeapon weapon = child.GetComponent<RaycastWeapon>();
            if (weapon != null)
            {
                weapons.Add(child.gameObject);
                currentWeaponInstance = child.gameObject;
                currentWeaponIndex = 0;
                Debug.Log($"Найдено оружие: {child.name}");
                break;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < weaponKeys.Length; i++)
        {
            if (Input.GetKeyDown(weaponKeys[i]) && i < weapons.Count)
            {
                SwitchWeapon(i);
            }
        }

        // Стрельба по ЛКМ
        if (currentWeaponInstance != null && Input.GetButtonDown("Fire1"))
        {
            RaycastWeapon weapon = currentWeaponInstance.GetComponent<RaycastWeapon>();
            if (weapon != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                RaycastHit hit;
                Vector3 target = Physics.Raycast(ray, out hit, 100f) ? hit.point : ray.GetPoint(100f);

                weapon.StartFiring();
                weapon.UpdateWeapon(Time.deltaTime, target);
                weapon.StopFiring();
            }
        }
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        if (weapons.Count >= 5)
        {
            Debug.Log("Нельзя взять больше 5 оружия.");
            return;
        }

        GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation);
        newWeapon.transform.SetParent(weaponHolder);
        newWeapon.SetActive(false);

        weapons.Add(newWeapon);
    }

    void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;

        if (currentWeaponInstance != null)
        {
            currentWeaponInstance.SetActive(false);
        }

        currentWeaponInstance = weapons[index];
        currentWeaponInstance.SetActive(true);
        currentWeaponIndex = index;
        Debug.Log($"Переключено на: {currentWeaponInstance.name}");
    }
}
