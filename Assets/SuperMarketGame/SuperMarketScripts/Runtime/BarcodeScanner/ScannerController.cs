using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerController : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float dragSmoothSpeed = 15f;

    [Header("Barcode Settings")]
    [SerializeField] private Transform scannerFace;
    [SerializeField] private LayerMask barcodeLayer;
    [SerializeField] private float detectionDistance = 2.0f;

    private ITouchInputHandler inputHandler;
    private IBarcodeDetector barcodeDetector;
    private IDraggable draggable;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }


    private void Start()
    {       
        draggable = new ScannerDrag(transform, Camera.main, dragSmoothSpeed, this);
        inputHandler = new ScannerInputHandler(draggable);
        barcodeDetector = new BarcodeDetector(scannerFace, barcodeLayer, detectionDistance, draggable);
    }

    private void Update()
    {      
        inputHandler.HandleInput();
        barcodeDetector.DetectBarcodes();
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (scannerFace != null)
        {
          
            Gizmos.color = Color.red;         
            Vector3 rayDirection = scannerFace.forward * detectionDistance;    
            Gizmos.DrawLine(scannerFace.position, scannerFace.position + rayDirection);        
            if (Physics.Raycast(scannerFace.position, scannerFace.forward, out RaycastHit hit, detectionDistance, barcodeLayer))
            {         
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
        }
    }

#endif

}
