using UnityEngine;

namespace ScpQuizUltimate
{
    public class CoroutineStarter : MonoBehaviour
    {
        private static CoroutineStarter instance = null;

        public static CoroutineStarter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CoroutineStarter>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = "CoroutineStarter";
                        instance = go.AddComponent<CoroutineStarter>();
                        DontDestroyOnLoad(go);
                    }
                }
                return instance;
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
