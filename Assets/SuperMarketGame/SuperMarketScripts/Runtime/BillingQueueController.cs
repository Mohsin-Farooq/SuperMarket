using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillingQueueController : MonoBehaviour
{
    public List<Transform> rampPositions;
    public Transform StartPos;
    public Transform finalPosition;
    public float moveSpeed = 5f;

    private Queue<GameObject> itemQueue = new Queue<GameObject>();
    private bool isMoving = false;

    [SerializeField] private GameObject BarCodeScanner;
    public static BillingQueueController instance;
    private int lastProcessedRampIndex = 0;
    private void Awake()
    {
        instance=this;
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

            yield return MoveItemToRamp(currentItem, targetPosition.position,targetPosition);
        }

        isMoving = false;
    }

    private IEnumerator MoveItemToRamp(GameObject item, Vector3 targetPosition,Transform ParentPostion)
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
        item.transform.SetParent(ParentPostion);

        if (itemQueue.Count == 0)
        {
            CameraTrigger.instacne.TriggerCameraWhenScan();
            BarCodeScanner.gameObject.SetActive(true);
            MoveItemsToFinalPosition();
        }
    }

    public void MoveItemsToFinalPosition()
    {
        StartCoroutine(MoveSingleItemFromRampToFinal());
    }
    int i;
    private IEnumerator MoveSingleItemFromRampToFinal()
    {
        int rampsCount = rampPositions.Count;
       
        while (i < rampsCount)
        
        { 
            Debug.Log($"Ramp cound :{rampsCount} and i is {i}");
           
            Transform rampPosition = rampPositions[i];

            if (rampPosition.childCount > 0)
            {
                GameObject item = rampPosition.GetChild(0).gameObject;
                item.GetComponent<Item>().enabled = true;
                Vector3 startPosition = item.transform.position;
                Vector3 targetPosition = finalPosition.position;

                float elapsedTime = 0f;

                while (elapsedTime < moveSpeed)
                {
                    elapsedTime += Time.deltaTime;
                    item.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveSpeed);
                    yield return null;
                }

                item.transform.position = targetPosition;
                i++;
                yield break;      
            }
            
        }
        
        Debug.Log("No more items on any ramp.");
    }
}
