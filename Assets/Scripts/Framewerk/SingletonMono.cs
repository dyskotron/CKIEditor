using System;
using UnityEngine;

namespace Framewerk
{
    /// <summary>
    /// Be aware this will not prevent a non singleton constructor
    ///   such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    ///
    /// As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Fields

        private static T _instance = null;
        private static bool _instanceInitilized = false;

        private static object _lock = new object();

        private static bool applicationIsQuitting = false;

        #endregion

        #region Properties

        public static bool IsInstanced { get { return _instance != null; } }

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("Singleton" + " Instance '" + typeof(T) +
                        "' already destroyed on application quit." +
                        " Won't create again.");
                    

                    return _instance;
                }

                lock (_lock)
                {
                    if (!_instanceInitilized)
                    {
                        try
                        {
                            //DebugUtil.Assert(Application.isPlaying, "SingletonMono is available only in playmode");
                            _instance = (T)FindObjectOfType(typeof(T));
                            _instanceInitilized = _instance != null;

                            if (FindObjectsOfType(typeof(T)).Length > 1)
                            {
                                Debug.LogWarning("Singleton Something went really wrong " +
                                                               " - there should never be more than 1 singleton!" +
                                                               " Reopenning the scene might fix it.");
                                return _instance;
                            }

                            if (_instance == null)
                            {
                                GameObject singleton = new GameObject();
                                singleton.name = "(singleton) " + typeof(T);
                                DontDestroyOnLoad(singleton);

                                _instance = singleton.AddComponent<T>();
                                _instanceInitilized = _instance != null;
                            }
                            else
                            {
                                Debug.Log("SingletonMono  Using instance already created: " + _instance.gameObject.name);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("SingletonMono singleton init failed: " + ex.Message);
                        }
                    }

                    return _instance;
                }
            }
        }

        #endregion

        #region Constructors
        #endregion

        #region Methods
        
        public TC GetSingleComponent<TC>() where TC : Component
        {
            TC t = this.gameObject.GetComponent(typeof(TC)) as TC;
            if (t == null)
                return gameObject.AddComponent<TC>();

            return t;
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }

        void Awake()
        {
            SingletonMonoInit();
        }

        protected virtual void SingletonMonoInit() { }

        #endregion
    }
}