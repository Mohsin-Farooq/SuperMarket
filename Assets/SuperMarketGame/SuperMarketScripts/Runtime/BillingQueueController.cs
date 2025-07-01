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
    public float itemMovingSpeed = 5f;
    [SerializeField] private float AllItemTimeToSpawnOnRamp;

    private Queue<GameObject> itemQueue = new Queue<GameObject>();
    private List<GameObject> rampItems = new List<GameObject>();
    private bool isMoving = false;

    [SerializeField] private GameObject BarCodeScanner,Canvas,ButtonManager;
    public static BillingQueueController instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddItemToQueue(GameObject item)
    {
        itemQueue.Enqueue(item); 
    }

    public void ProcessItemOnRamp()
    {
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
            StartCoroutine(MoveItemToRamp(currentItem, targetPosition.position));
            yield return new WaitForSeconds(AllItemTimeToSpawnOnRamp);
        }

        isMoving = false;
        yield return new WaitForSeconds(0.5f);
        ActivateScanner();
    }

    private IEnumerator MoveItemToRamp(GameObject item, Vector3 targetPosition)
    {
        Vector3 startPosition = item.transform.position;
        Vector3 midPosition = new Vector3(startPosition.x, startPosition.y + 12f, startPosition.z); 
        float elapsedTime = 0f;
        float totalDuration = itemMovingSpeed;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            
            float t = elapsedTime / totalDuration;

           
            Vector3 curvePosition = Vector3.Lerp(
                Vector3.Lerp(startPosition, midPosition, t),
                Vector3.Lerp(midPosition, targetPosition, t),
                t
            );

            item.transform.position = curvePosition;

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

        while (elapsedTime < itemMovingSpeed)
        {
            elapsedTime += Time.deltaTime;
            item.transform.position = Vector3.Lerp(startPosition, ScannedPosition.position, elapsedTime / itemMovingSpeed);
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

            if (rampItems.Count > 0)
            {
               
                StartCoroutine(MoveItemToScanner(rampItems[0]));
            }
            else
            {
                CameraTrigger.instacne.TriggerCameraWhenBill();                          
                BarCodeScanner.gameObject.SetActive(false);
                Invoke(nameof(EnableCanvas), 0.5f);
            }
        }
    }

    private void EnableCanvas()
    {
        Canvas.gameObject.SetActive(true);
        ButtonManager.gameObject.SetActive(true);
    }
}
