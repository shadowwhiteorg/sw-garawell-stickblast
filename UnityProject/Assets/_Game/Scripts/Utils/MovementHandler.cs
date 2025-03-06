// In _Game/Utils/MovementHandler.cs
using System.Collections;
using _Game.Enums;
using UnityEngine;

namespace _Game.Utils
{
    public static class MovementHandler
    {
        public static void MoveWithEase(MonoBehaviour movingObject, Vector3 targetPosition, float movementSpeed, Easing easing)
        {
            movingObject.StartCoroutine(MoveToTarget(movingObject.transform, targetPosition, movementSpeed, easing));
        }

        public static void MoveWithEase(Transform movingTransform, Vector3 targetPosition, float movementSpeed, Easing easing)
        {
            CoroutineRunner.Instance.StartCoroutine(MoveToTarget(movingTransform, targetPosition, movementSpeed, easing));
        }

        private static IEnumerator MoveToTarget(Transform movingTransform, Vector3 targetPosition, float movementSpeed, Easing easing)
        {
            Vector3 startPosition = movingTransform.position;
            float elapsedTime = 0f;
            float journeyLength = Vector3.Distance(startPosition, targetPosition);

            while (elapsedTime < journeyLength / movementSpeed)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (journeyLength / movementSpeed);
                t = CalculateEasing(easing, t);
                movingTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            movingTransform.position = targetPosition;
        }

        private static float CalculateEasing(Easing easing, float t)
        {
            return easing switch
            {
                Easing.InSine => 1 - Mathf.Cos((t * Mathf.PI) / 2),
                Easing.OutSine => Mathf.Sin((t * Mathf.PI) / 2),
                Easing.InOutSine => 0.5f * (1 - Mathf.Cos(t * Mathf.PI)),
                Easing.OutBack => 1 + 2.70158f * Mathf.Pow(t - 1, 3) + 1.70158f * Mathf.Pow(t - 1, 2),
                _ => t
            };
        }
    }
}