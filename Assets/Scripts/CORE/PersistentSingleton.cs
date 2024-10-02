// inspired by git-amend's video about Bettter Singletons:
// https://www.youtube.com/watch?v=LFOXge7Ak3E

using UnityEngine;

namespace CORE 
{
    /// <summary>
    /// A generic singleton class for Unity components that persists across scenes.
    /// </summary>
    /// <typeparam name="T">The type of the singleton component.</typeparam>
    public class PersistentSingleton<T> : MonoBehaviour where T : Component 
    {
        /// <summary>
        /// If true, the GameObject will be unparented on Awake(). Useful for keeping singletons across scenes.
        /// </summary>
        public bool AutoUnparentOnAwake = true;

        protected static T instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Checks if the singleton instance exists.
        /// </summary>
        public static bool HasInstance => instance != null;

        /// <summary>
        /// Gets the singleton instance, creating it if necessary.
        /// </summary>
        public static T Instance 
        {
            get 
            {
                if (!instance) 
                {
                    lock (_lock)
                    {
#if UNITY_EDITOR
                        Debug.Log("Inside singleton lock");
#endif
                        if (!instance)
                        {
#if UNITY_2023_1_OR_NEWER
                            Debug.Log("Creating new instance using FindAnyObjectByType");
                            instance = FindAnyObjectByType<T>();
#else
                            Debug.Log("Creating new instance using FindObjectOfType");
                            instance = FindObjectOfType<T>();
#endif
                            if (!instance) 
                            {
                                var go = new GameObject(typeof(T).Name + " Auto-Generated");
                                instance = go.AddComponent<T>();
                            }
                        }
                    }
                }
                
                return instance;
            }
        }

        /// <summary>
        /// Initializes the singleton instance.
        /// <para><b>IMPORTANT:</b> If you override Awake(), you <b>must</b> call base.Awake() to ensure proper singleton initialization.</para>
        /// </summary>
        protected virtual void Awake() 
        {
            InitializeSingleton();
        }

        /// <summary>
        /// Initializes the singleton instance, ensuring that it persists across scene loads.
        /// </summary>
        /// <remarks>
        /// If the application is not playing, the method will return early.
        /// If <c>AutoUnparentOnAwake</c> is true, the GameObject's transform will be unparented.
        /// Ensures that there is only one instance of the singleton in the scene and sets it to
        /// not be destroyed on load. If a duplicate instance is found, the new instance is destroyed.
        protected virtual void InitializeSingleton() 
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (AutoUnparentOnAwake)
            {
                transform.SetParent(null);
            }

            if (!instance)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            } 
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //private static void InitializeOnLoad()
        //{
        //    // Force singleton initialization
        //    _ = Instance;
        //}
    }
}
