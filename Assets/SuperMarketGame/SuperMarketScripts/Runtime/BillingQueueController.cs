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

            yield return MoveItemToRamp(currentItem, targetPosition.position);
        }

        isMoving = false;
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

        if (itemQueue.Count == 0)
        {
            CameraTrigger.instacne.TriggerCameraWhenScan();
            BarCodeScanner.gameObject.SetActive(true);
            MoveItemsToFinalPosition();
        }
    }

    public void MoveItemsToFinalPosition()
    {
        StartCoroutine(MoveItemsFromRampToFinal());
    }

    private IEnumerator MoveItemsFromRampToFinal()
    {
        foreach (Transform rampPosition in rampPositions)
        {
            if (rampPosition.childCount > 0) 
            {
                GameObject item = rampPosition.GetChild(0).gameObject; 
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

               
                item.SetActive(false);
            }
        }
    }
}
