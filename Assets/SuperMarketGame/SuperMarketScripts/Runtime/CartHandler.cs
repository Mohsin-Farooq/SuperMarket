using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperMarketGame
{
    public class CartHandler : MonoBehaviour
    {
        [SerializeField] private Transform CartTargetPosition;
        [SerializeField] private float lerpDuration;


        private void Start()
        {
            StartCoroutine(LerpCart(CartTargetPosition));
        }

        private IEnumerator LerpCart(Transform CartTargetPosition)
        {
            Vector3 startPosition = transform.position;

            float elapsedTime = 0f;

            while (elapsedTime < lerpDuration)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, CartTargetPosition.position, elapsedTime / lerpDuration);
                yield return null;
            }

            transform.position = CartTargetPosition.position;
            BillingQueueController.instance.ProcessItemOnRamp();
        }
    }
}