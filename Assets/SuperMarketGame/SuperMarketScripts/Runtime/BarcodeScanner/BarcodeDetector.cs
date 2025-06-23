using UnityEngine;

public class BarcodeDetector : IBarcodeDetector
{
    private readonly Transform scannerFace;
    private readonly LayerMask barcodeLayer;
    private readonly float detectionDistance;

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
        Debug.Log($"Barcode Scanned: {barcodeObject.name}");
    }
}
