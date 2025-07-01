using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private List<Transform> cartPositions;
    [SerializeField] private BillingQueueController queueController;
    [SerializeField] private int numberOfItemsToSpawn = 6;

    public void InstantiateRandomItems()
    {
        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            Debug.LogWarning("No item prefabs assigned!");
            return;
        }

        if (cartPositions == null || cartPositions.Count == 0)
        {
            Debug.LogWarning("No cart positions assigned!");
            return;
        }

        for (int i = 0; i < Mathf.Min(numberOfItemsToSpawn, cartPositions.Count); i++)
        {
            GameObject randomPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)]; 
            Transform spawnPosition = cartPositions[i];
            GameObject newItem = Instantiate(randomPrefab, spawnPosition.position, randomPrefab.transform.rotation);
            newItem.transform.SetParent(spawnPosition);
            queueController.AddItemToQueue(newItem);
        }
    }

    private void Start()
    {
        InstantiateRandomItems();
    }
}
