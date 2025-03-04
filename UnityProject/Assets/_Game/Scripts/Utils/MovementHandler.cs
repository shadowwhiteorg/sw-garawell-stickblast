using System.Collections;
using _Game.Enums;
using UnityEngine;

namespace _Game.Utils
{
    public static class MovementHandler<T> where T : MonoBehaviour
     {
         private static Vector3 _startPosition;
         private static float _elapsedTime;

         public static void FollowTarget(T movingObjet,Vector3 targetPosition, Vector3 followOffset = default, float speed = 100f)
         {
             movingObjet.StartCoroutine(FollowTargetCoroutine(movingObjet, targetPosition, followOffset, speed));
         }

         private static IEnumerator FollowTargetCoroutine(T movingObject,Vector3 targetPosition, Vector3 followOffset = default,
             float speed = 100f)
         {
             _startPosition = movingObject.transform.position;
             _elapsedTime = 0f;

             while (Vector3.Distance(movingObject.transform.position, targetPosition) > 0.1f)
             {
                 _elapsedTime += Time.deltaTime;
                 Vector3 newPosition = Vector3.Lerp(_startPosition, targetPosition + followOffset, _elapsedTime * speed);
                 movingObject.transform.position = newPosition;
                 yield return null;
             }
             movingObject.transform.position = targetPosition + followOffset;
         }
         
         
         public static void MoveWithEase(T movingObject, Vector3 targetPosition, float movementSpeed, Easing easing)
         {
             movingObject.StartCoroutine(MoveToTarget(movingObject, targetPosition, movementSpeed, easing));
         }
         
         private static IEnumerator MoveToTarget(T movingObject, Vector3 targetPosition, float movementSpeed, Easing easing)
         {
             _startPosition = movingObject.transform.position;
             _elapsedTime = 0f;
             float journeyLength = Vector3.Distance(_startPosition, targetPosition);

             while (_elapsedTime < journeyLength / movementSpeed)
             {
                 _elapsedTime += Time.deltaTime;
                 float t = _elapsedTime / (journeyLength / movementSpeed);
                 t = CalculateEasing(easing, t);
                 movingObject.transform.position = Vector3.Lerp(_startPosition, targetPosition, t);
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