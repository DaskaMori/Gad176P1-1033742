using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spawners
{
    public class MonsterSpawner : AbstractSpawner
    {
        [Header("Monster Prefabs")]
        public GameObject meleePrefab;
        public GameObject rangedPrefab;

        [Header("Spawn Points")]
        public List<Transform> spawnPoints;

        private bool hasDoneInitialSpawn = false;

        public new void Start()
        {
            if (spawnPoints != null)
                spawnPoints.RemoveAll(pt => pt == null);

            Spawn();
            hasDoneInitialSpawn = true;
        }

        protected override void Spawn()
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
                return;

            foreach (var pt in spawnPoints)
            {
                var prefab = Random.value < 0.5f ? meleePrefab : rangedPrefab;
                if (prefab != null)
                    Instantiate(prefab, pt.position, pt.rotation);
            }
        }
        void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (hasDoneInitialSpawn)
                Spawn();
        }
    }
}