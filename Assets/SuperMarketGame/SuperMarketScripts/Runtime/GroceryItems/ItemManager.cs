using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> itemPrefabs; 
    public Transform cartPosition;
    public BillingQueueController queueController; 
    public int numberOfItemsToSpawn = 6; 

    public void InstantiateRandomItems()
    {
        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            Debug.LogWarning("No item prefabs assigned!");
            return;
        }

        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            
            GameObject randomPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];    
            GameObject newItem = Instantiate(randomPrefab, cartPosition.position,randomPrefab.transform.rotation);
            queueController.AddItemToQueue(newItem);
        }
    }


    private void Start()
    {
        InstantiateRandomItems();
    }
}
