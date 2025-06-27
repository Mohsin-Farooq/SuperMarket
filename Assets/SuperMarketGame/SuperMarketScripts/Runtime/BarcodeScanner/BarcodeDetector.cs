using System.Collections.Generic;
using UnityEngine;

public class BarcodeDetector : IBarcodeDetector
{
    private  Transform scannerFace;
    private readonly LayerMask barcodeLayer;
    private readonly float detectionDistance;
    

    private HashSet<GameObject> DetectedObjects = new HashSet<GameObject>();
    private float totalAmountData = 0;
    private Vector3 ScannerStartpostion;
    public BarcodeDetector(Transform scannerFace, LayerMask barcodeLayer, float detectionDistance)
    {
        this.scannerFace = scannerFace;
        this.barcodeLayer = barcodeLayer;
        this.detectionDistance = detectionDistance;

        ScannerStartpostion = scannerFace.position;
    }

    public void DetectBarcodes()
    {
        if (Physics.Raycast(scannerFace.position, scannerFace.forward, out RaycastHit hit, detectionDistance, barcodeLayer))
        {
            if (!DetectedObjects.Contains(hit.collider.gameObject))
            {
                DetectedObjects.Add(hit.collider.gameObject);
                OnBarcodeDetected(hit.collider.gameObject);
                MethodsInvoker();
            }
        }
    }

    public void OnBarcodeDetected(GameObject barcodeObject)
    {

        Item item = barcodeObject.GetComponent<Item>();
        if (item != null)
        {
            totalAmountData += item.Price;
            CashCounterEvent.OnAmountUpdate?.Invoke(totalAmountData);
            Debug.Log($"Scanned Item: {item.ItemName}, Price: ${item.Price}");
        }
        else
        {
            Debug.LogWarning("Scanned object does not have an Item component!");
        }
    }

    private void MethodsInvoker()
    {
        ScannerController.ResetScannerPosition?.Invoke();
        Item.ItemPostionChange?.Invoke();
    }
  
}
