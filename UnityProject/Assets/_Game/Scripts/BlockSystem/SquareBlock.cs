using System.Collections;
using Mono.Cecil;
using UnityEngine;

namespace _Game.BlockSystem
{
    public sealed class SquareBlock : GridObjectBase
    {
        [SerializeField] private GameObject model;
        [SerializeField] private ParticleSystem blastParticle;
        public SquareBlock(Vector2Int gridPosition, Vector2 worldPosition)
        {
            SetPosition(gridPosition.x, gridPosition.y, worldPosition.x, worldPosition.y); 
        }

        public void InitializeVisually()
        {
            StartCoroutine(ScaleZeroToTargetSmoothly());
        }

        public void Blast()
        {
            blastParticle.transform.SetParent(null);
            blastParticle.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        
        private IEnumerator ScaleZeroToTargetSmoothly()
        {
            float duration = .25f;
            Vector3 initialScale = Vector3.zero;
            Vector3 targetScale = model.transform.localScale;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                model.transform.localScale = Vector3.Lerp(initialScale, targetScale, t / duration);
                yield return null;
            }

            model.transform.localScale = targetScale;
        }
    }
}