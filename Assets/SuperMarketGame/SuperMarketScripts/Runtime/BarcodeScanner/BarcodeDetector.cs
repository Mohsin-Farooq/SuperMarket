using MoreMountains.NiceVibrations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BarcodeDetector : IBarcodeDetector
{
    private  Transform scannerFace;
    private readonly LayerMask barcodeLayer;
    private readonly float detectionDistance;
    private readonly IDraggable draghandler;

    private HashSet<GameObject> DetectedObjects = new HashSet<GameObject>();
    private float totalAmountData = 0;
    private Vector3 ScannerStartpostion;
    public BarcodeDetector(Transform scannerFace, LayerMask barcodeLayer, float detectionDistance,IDraggable dragHandler)
    {
        this.scannerFace = scannerFace;
        this.barcodeLayer = barcodeLayer;
        this.detectionDistance = detectionDistance;

        ScannerStartpostion = scannerFace.position;

        this.draghandler = dragHandler;
    }

    

    public void DetectBarcodes()
    {
        if (Physics.Raycast(scannerFace.position, scannerFace.forward, out RaycastHit hit, detectionDistance, barcodeLayer))
        {
            if (!DetectedObjects.Contains(hit.collider.gameObject))
            {
                AudioManager._instance.PlaySound("Scan");
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
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
        }
        else
        {
            Debug.LogWarning("Scanned object does not have an Item component!");
        }
    }

    private void MethodsInvoker()
    {
        draghandler.EndDrag(0);
        Item.ItemPostionChange?.Invoke();
        BillingQueueController.instance.ProcessItemWithDelay();

    }
}
