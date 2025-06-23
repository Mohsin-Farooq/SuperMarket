using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Properties")]
    public ItemSO itemData; 
    public float Price => itemData != null ? itemData.price : 0.0f;
    public string ItemName => itemData != null ? itemData.ItemName : "Unknown Item";

}
