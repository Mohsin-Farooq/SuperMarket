using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillingQueueController : MonoBehaviour
{
    public List<Transform> rampPositions;
    public Transform StartPos;
    public Transform finalPosition;
    public Transform ScannedPosition; 
    public float moveSpeed = 5f;

    private Queue<GameObject> itemQueue = new Queue<GameObject>();
  [SerializeField]  private List<GameObject> rampItems = new List<GameObject>();
    private bool isMoving = false;

    [SerializeField] private GameObject BarCodeScanner;
    public static BillingQueueController instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddItemToQueue(GameObject item)
    {
        itemQueue.Enqueue(item);

        if (!isMoving)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isMoving = true;
        int currentRampIndex = 0;

        while (itemQueue.Count > 0 && currentRampIndex < rampPositions.Count)
        {
            GameObject currentItem = itemQueue.Dequeue();
            Transform targetPosition = rampPositions[currentRampIndex];
            currentRampIndex++;

            rampItems.Add(currentItem);
            yield return MoveItemToRamp(currentItem, targetPosition.position);
        }

        isMoving = false;
        ActivateScanner();
    }

    private IEnumerator MoveItemToRamp(GameObject item, Vector3 targetPosition)
    {
        Vector3 startPosition = StartPos.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            item.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveSpeed);
            yield return null;
        }

        item.transform.position = targetPosition;
    }

    private void ActivateScanner()
    {
        if (rampItems.Count > 0)
        {
            BarCodeScanner.gameObject.SetActive(true);
            CameraTrigger.instacne.TriggerCameraWhenScan();
            StartCoroutine(MoveItemToScanner(rampItems[0]));
        }
    }

    private IEnumerator MoveItemToScanner(GameObject item)
    {
        Vector3 startPosition = item.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            elapsedTime += Time.deltaTime;
            item.transform.position = Vector3.Lerp(startPosition, ScannedPosition.position, elapsedTime / moveSpeed);
            yield return null;
        }

        item.transform.position = ScannedPosition.position;
        item.GetComponentInChildren<Item>().enabled = true;
    }
    //function to call when item scanned
    public void ProcessScannedItem()
    {
        if (rampItems.Count > 0)
        {
            GameObject scannedItem = rampItems[0];
            rampItems.RemoveAt(0); 
            
            Debug.Log($"Scanned: {scannedItem.name}");

            if (rampItems.Count > 0)
            {
                StartCoroutine(MoveItemToScanner(rampItems[0]));
            }
            else
            {
                Debug.Log("All items scanned.");
                BarCodeScanner.gameObject.SetActive(false);
            }
        }
    }
}
