using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spawners
{
    public class PlayerSpawner : AbstractSpawner
    {
        [Header("Player Prefabs & Spawn Points")]
        public GameObject player1Prefab;
        public Transform player1Spawn;
        public GameObject player2Prefab;
        public Transform player2Spawn;

   
        protected override void Spawn()
        {
            bool hasP1 = false;
            bool hasP2 = false;

            var players = FindObjectsOfType<PlayerMovement>();
            foreach (var pm in players)
            {
                if (pm.playerID == PlayerID.One)
                {
                    hasP1 = true;
                    pm.transform.position = player1Spawn.position;
                    pm.transform.rotation = player1Spawn.rotation;
                }
                else if (pm.playerID == PlayerID.Two)
                {
                    hasP2 = true;
                    pm.transform.position = player2Spawn.position;
                    pm.transform.rotation = player2Spawn.rotation;
                }
            }

            // Mainly will be used if the player game object gets disabled due to dying 
            if (!hasP1 && player1Prefab != null && player1Spawn != null)
                Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation);

            if (!hasP2 && player2Prefab != null && player2Spawn != null)
                Instantiate(player2Prefab, player2Spawn.position, player2Spawn.rotation);
        }
        void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Spawn();
        }

    }
}

