using UnityEngine;

public interface IBarcodeDetector
{
    void DetectBarcodes();
    void OnBarcodeDetected(GameObject barcodeObject);
}