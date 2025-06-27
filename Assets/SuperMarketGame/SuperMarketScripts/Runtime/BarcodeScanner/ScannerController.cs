using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerController : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float dragSmoothSpeed = 15f;
    [SerializeField] private float fixedZ = 0f;

    [Header("Barcode Settings")]
    [SerializeField] private Transform scannerFace;
    [SerializeField] private LayerMask barcodeLayer;
    [SerializeField] private float detectionDistance = 2.0f;

    private ITouchInputHandler inputHandler;
    private IBarcodeDetector barcodeDetector;
    private IDraggable draggable;
    private Vector3 startPosition;

    public static Action ResetScannerPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        ResetScannerPosition += ResetPosition;
    }

    private void OnDisable()
    {
        ResetScannerPosition -= ResetPosition;
    }

    private void Start()
    {       
        draggable = new ScannerDrag(transform, Camera.main, dragSmoothSpeed, fixedZ);
        barcodeDetector = new BarcodeDetector(scannerFace, barcodeLayer, detectionDistance);
  
        inputHandler = new ScannerInputHandler(draggable);       
    }

    private void Update()
    {      
        inputHandler.HandleInput();
        barcodeDetector.DetectBarcodes();
    }


    private void ResetPosition()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothResetScannerPos());
    }


    private IEnumerator SmoothResetScannerPos()
    {
        float duration = 0.2f; 
        float elapsed = 0f;

        Vector3 initialPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, startPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
    }
}
