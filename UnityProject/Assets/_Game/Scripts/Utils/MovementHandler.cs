using System.Collections;
using _Game.Enums;
using UnityEngine;

namespace _Game.Utils
{
    public static class MovementHandler<T> where T : MonoBehaviour
     {
         
         public static void MoveWithEase(T movingObject, Vector3 targetPosition, float movementSpeed, Easing easing)
         {
             movingObject.StartCoroutine(MoveToTarget(movingObject, targetPosition, movementSpeed, easing));
         }
         
         private static IEnumerator MoveToTarget(T movingObject, Vector3 targetPosition, float movementSpeed, Easing easing)
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
            switch (easing)
            {
                case Easing.InSine:
                    return 1 - Mathf.Cos((t * Mathf.PI) / 2);
                case Easing.OutSine:
                    return Mathf.Sin((t * Mathf.PI) / 2);
                case Easing.InOutSine:
                    return 0.5f * (1 - Mathf.Cos(t * Mathf.PI));
                case Easing.OutBack:
                    float c1 = 1.70158f;
                    float c3 = c1 + 1;
                    return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
                default:
                    return t;
            }
        }
        
    }
    
}