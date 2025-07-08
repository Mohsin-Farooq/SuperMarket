using UnityEngine;

namespace SuperMarketGame
{
    public interface IBarcodeDetector
    {
        void DetectBarcodes();
        void OnBarcodeDetected(GameObject barcodeObject);

    }
}