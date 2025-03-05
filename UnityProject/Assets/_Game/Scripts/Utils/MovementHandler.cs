using System.Collections;
using _Game.Enums;
using UnityEngine;

namespace _Game.Utils
{
    public class MovementHandler
    {
        public static void MoveWithEase(MonoBehaviour movingObject, Vector3 targetPosition, float movementSpeed,
            Easing easing)
        {
            movingObject.StartCoroutine(MoveToTarget(movingObject, targetPosition, movementSpeed, easing));
        }

        private static IEnumerator MoveToTarget(MonoBehaviour movingObject, Vector3 targetPosition, float movementSpeed,
            Easing easing)
        {
            Vector3 startPosition = movingObject.transform.position;
            float elapsedTime = 0f;
            float journeyLength = Vector3.Distance(startPosition, targetPosition);

            while (elapsedTime < journeyLength / movementSpeed)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (journeyLength / movementSpeed);
                t = CalculateEasing(easing, t);
                movingObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            movingObject.transform.position = targetPosition;
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