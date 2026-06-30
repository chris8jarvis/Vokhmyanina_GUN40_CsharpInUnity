using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Настройки урона")]
    public int damage = 10;
    public float damageCooldown = 1f;

    private Coroutine damageCoroutine;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок попал в ловушку");
            
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDamage(other));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyDamage(Collider player)
    {
        while (true)
        {
            if (player == null || !player.gameObject.activeInHierarchy)
            {
                break;
            }

            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage, Vector3.up);
                Debug.Log($"Ловушка нанесла {damage} урона");
            }
            else
            {
                Debug.LogWarning("У игрока нет PlayerHealth");
                break;
            }

            yield return new WaitForSeconds(damageCooldown);
        }

        damageCoroutine = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
