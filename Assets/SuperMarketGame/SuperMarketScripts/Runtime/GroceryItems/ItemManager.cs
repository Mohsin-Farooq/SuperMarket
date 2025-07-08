using System.Collections.Generic;
using UnityEngine;

namespace SuperMarketGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> itemPrefabs;
        [SerializeField] private List<Transform> cartPositions;
        [SerializeField] private BillingQueueController queueController;
        [SerializeField] private int numberOfItemsToSpawn = 6;

        public void InstantiateRandomItems()
        {
            if (itemPrefabs == null || itemPrefabs.Count == 0 || cartPositions == null || cartPositions.Count == 0)
            {
                Debug.LogWarning("Missing item prefabs or spawn positions!");
                return;
            }

            int spawnCount = Random.Range(1, Mathf.Max(8, cartPositions.Count + 1));
            List<Transform> availableSpots = new List<Transform>(cartPositions);

            for (int i = 0; i < spawnCount; i++)
            {
                GameObject randomPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
                int randomIndex = Random.Range(0, availableSpots.Count);

                Transform randomSpawn = availableSpots[randomIndex];
                availableSpots.RemoveAt(randomIndex);


                GameObject newItem = Instantiate(randomPrefab, randomSpawn.position, randomPrefab.transform.rotation);
                newItem.transform.SetParent(randomSpawn);

                queueController.AddItemToQueue(newItem);
            }
        }


        private void Start()
        {
            InstantiateRandomItems();
        }
    }
}