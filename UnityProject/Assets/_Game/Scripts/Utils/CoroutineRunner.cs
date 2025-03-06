using UnityEngine;

namespace _Game.Utils
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }
}