
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Grocery/Item")]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public float price;
}
