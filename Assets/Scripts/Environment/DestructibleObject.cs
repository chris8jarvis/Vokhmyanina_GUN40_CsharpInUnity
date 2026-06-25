using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [Header("Настройки прочности")]
    public float health = 30f;           // Сколько выдержит попаданий
    public GameObject destroyedVersion;
    public float destroyDelay = 0.1f;

    [Header("Эффекты")]
    public ParticleSystem destructionEffect;

    // Start is called before the first frame update
    void Start()
    {
    //  if (destroyedVersion == null)
    //     {
    //         destroyedVersion = CreateDestroyedVersion();
    //     }   
    }

    // Этот метод вызывается, когда в объект попадает пуля
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} получил {damage} урона. Осталось HP: {health}");

        if (health <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        // Создаём разрушенную версию (обломки)
        if (destroyedVersion != null)
        {
            GameObject wreck = Instantiate(destroyedVersion, transform.position, transform.rotation);
            // Разбросаем обломки в стороны
            Rigidbody[] bits = wreck.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody bit in bits)
            {
                bit.AddExplosionForce(300f, transform.position + Vector3.up, 5f);
            }
        }
        Destroy(gameObject, destroyDelay);
    }

    // Нет разрушенной версии в инспекторе - создать автоматически
    private GameObject CreateDestroyedVersion()
    {
        // Создаём кучу обломков на основе дочерних объектов
        GameObject wreck = new GameObject(gameObject.name + "_Destroyed");
        wreck.transform.position = transform.position;
        wreck.transform.rotation = transform.rotation;

        // Проходим по всем дочерним объектам и превращаем их в обломки
        foreach (Transform child in transform)
        {
            // Создаём копию каждого кубика
            GameObject bit = Instantiate(child.gameObject, wreck.transform);
            bit.transform.localPosition = child.localPosition;
            bit.transform.localRotation = child.localRotation;

            // Добавляем Rigidbody, чтобы кубики падали
            Rigidbody rb = bit.AddComponent<Rigidbody>();
            rb.mass = 1f;
            
            // Добавляем BoxCollider для столкновений
            if (bit.GetComponent<Collider>() == null)
            {
                bit.AddComponent<BoxCollider>();
            }
        }

        return wreck;
    }
}
