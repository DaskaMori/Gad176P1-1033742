using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spawners
{
    public class ChestSpawner : AbstractSpawner
    {
        [Header("Chest Prefab & Locations")]
        public GameObject chestPrefab;
        public List<Transform> chestSpawnPoints;

        protected override void Spawn()
        {
            if (chestPrefab == null || chestSpawnPoints == null)
                return;

            foreach (var point in chestSpawnPoints)
            {
                if (point != null)
                    Instantiate(chestPrefab, point.position, point.rotation);
            }
        }

        void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Spawn();
        }
    }
}
