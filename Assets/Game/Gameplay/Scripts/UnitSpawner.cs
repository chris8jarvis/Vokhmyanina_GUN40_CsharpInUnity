using UnityEngine;

namespace SampleProject
{
    public sealed class UnitSpawner : MonoBehaviour
    {
        [Header("Unit Prefab")]
        [SerializeField]
        private GameObject prefab;

        [Header("Spawn Count")]
        [SerializeField]
        private int count = 20;

        [Header("Spawn size")]
        [SerializeField]
        private float areaSize = 8f;

        [Header("Spawn height")]
        [SerializeField]
        private float spawnY = 0.5f;

        private void Start()
        {
            if (this.prefab == null)
            {
                Debug.LogError("[UnitSpawner] Prefab not set.");
                return;
            }

            for (int i = 0; i < this.count; i++)
            {
                Vector3 pos = new Vector3(
                    Random.Range(-this.areaSize, this.areaSize),
                    this.spawnY,
                    Random.Range(-this.areaSize, this.areaSize)
                );

                Instantiate(this.prefab, pos, Quaternion.identity, this.transform);
            }

            Debug.Log($"[UnitSpawner] Spawned {this.count} units.");
        }
    }
}