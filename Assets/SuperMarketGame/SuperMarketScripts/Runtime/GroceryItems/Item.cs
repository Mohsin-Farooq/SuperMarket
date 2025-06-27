using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Properties")]
    public ItemSO itemData; 
    public float Price => itemData != null ? itemData.price : 0.0f;
    public string ItemName => itemData != null ? itemData.ItemName : "Unknown Item";

    public static Action ItemPostionChange;

    private void OnEnable()
    {
        ItemPostionChange += ChangeItemPos;
    }

    private void OnDisable()
    {
        ItemPostionChange -= ChangeItemPos;
    }

    private void ChangeItemPos()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothResetScannerPos());
    }

    private IEnumerator SmoothResetScannerPos()
    {
        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + transform.right * 10.0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
         gameObject.SetActive(false);
    }

}
