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

        protected override void Spawn()
        {
            if (spawnPoints == null)
                return;

            foreach (var pt in spawnPoints)
            {
                if (pt == null) continue;

                GameObject prefab = (Random.value < 0.5f) ? meleePrefab : rangedPrefab;
                if (prefab != null)
                    Instantiate(prefab, pt.position, pt.rotation);
            }
        }

        void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => Spawn();
    }
}