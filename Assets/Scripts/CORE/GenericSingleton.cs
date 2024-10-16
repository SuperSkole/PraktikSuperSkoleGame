// inspired by git-amend's video about Better Singletons:
// https://www.youtube.com/watch?v=LFOXge7Ak3E

using UnityEngine;

namespace CORE
{
    public class GenericSingleton<T> : MonoBehaviour where T : Component
    {
        protected internal static T instance;

        public static bool HasInstance => instance != null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Attempt to find an existing instance in the scene
#if UNITY_2023_1_OR_NEWER
                    instance = FindAnyObjectByType<T>();
#else
                    instance = FindObjectOfType<T>();
#endif
                    if (instance == null)
                    {
                        // Optionally, create a new instance if one doesn't exist
                        Debug.LogWarning($"{typeof(T).Name}: Creating new instance during get_Instance");
                        var go = new GameObject(typeof(T).Name + " Auto-Generated");
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need Awake.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
