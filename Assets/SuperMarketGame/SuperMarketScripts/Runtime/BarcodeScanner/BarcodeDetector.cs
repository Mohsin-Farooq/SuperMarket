using UnityEngine;

public class BarcodeDetector : IBarcodeDetector
{
    private readonly Transform scannerFace;
    private readonly LayerMask barcodeLayer;
    private readonly float detectionDistance;

    private float totalAmountData= 0;
    public BarcodeDetector(Transform scannerFace, LayerMask barcodeLayer, float detectionDistance)
    {
        this.scannerFace = scannerFace;
        this.barcodeLayer = barcodeLayer;
        this.detectionDistance = detectionDistance;
    }

    public void DetectBarcodes()
    {
        if (Physics.Raycast(scannerFace.position, scannerFace.forward, out RaycastHit hit, detectionDistance, barcodeLayer))
        {
            OnBarcodeDetected(hit.collider.gameObject);
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
}
