using System;
using System.Collections;
using UnityEngine;

namespace SuperMarketGame
{
    public class Item : MonoBehaviour
    {
        [Header("Item Properties")]
        public ItemSO itemData;
        public float Price => itemData != null ? itemData.price : 0.0f;
        public string ItemName => itemData != null ? itemData.ItemName : "Unknown Item";
        public static Action ItemPostionChange;
        private void OnEnable()
        {
            ItemPostionChange += ItemPosDelay;
        }

        private void OnDisable()
        {
            ItemPostionChange -= ItemPosDelay;
        }


        private void ItemPosDelay()
        {
            Invoke(nameof(ChangeItemPos), 0.3f);
        }

        private void ChangeItemPos()
        {
            StopAllCoroutines();
            StartCoroutine(SmoothResetItemPos());
        }

        private IEnumerator SmoothResetItemPos()
        {
            float duration = 0.5f;
            float elapsed = 0f;

            Vector3 initialPosition = transform.parent.position;
            Vector3 targetPosition = initialPosition + Vector3.right * 10.0f;
            while (elapsed < duration)
            {
                transform.parent.position = Vector3.Lerp(initialPosition, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.parent.gameObject.SetActive(false);
        }
    }
}