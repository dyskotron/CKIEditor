using System;
using CKIEditor;
using strange.extensions.mediation.api;
using UnityEngine;

namespace Framewerk.Managers
{
    public interface IUiManager
    {
        GameObject InstantiateView(string path = "", Transform parent = null);

        T InstantiateView<T>(string path = "", Transform parent = null) where T : IView;

        GameObject InstantiateView(GameObject viewPrefab, Transform parent = null);

        string GetViewName(Type type);
    }

    public class UiManager : IUiManager
    {
        public const string UI_PREFABS_ROOT = "UI/";
        public const string VIEW_SUFFIX = "View";

        [Inject]
        public IAssetManager AssetManager { get; set; }

        [Inject]
        public ViewConfig ViewConfig
        {
            set
            {
                _uiParent = value.UiDefault;
            }
        }

        private Transform _uiParent;

        /// <summary>
        /// Instantiate UI by path.
        /// </summary>
        /// <param name="path">Path to UI prefab from UI root</param>
        /// <param name="parent">Where UI prefab should be instantiated</param>
        /// <returns></returns>
        public GameObject InstantiateView(string path, Transform parent = null)
        {
            if (parent == null)
                parent = _uiParent;

            GameObject uiObj = AssetManager.GetAsset<GameObject>(UI_PREFABS_ROOT + path);
            uiObj.transform.SetParent(parent, false);

            return uiObj;
        }

        /// <summary>
        /// Instantiate UI, finds path by View Class.
        /// </summary>
        /// <param name="path">Optional path that can be added between UI root and prefab name
        /// UI_ROOT/[ADDED CUSTOM PATH/]prefabname</param>
        /// <param name="parent">Where UI prefab should be instantiated</param>
        /// <returns></returns>
        public T InstantiateView<T>(string path = "", Transform parent = null) where T : IView
        {
            if (parent == null)
                parent = _uiParent;

            var uiObj = InstantiateView(GetViewPath(typeof(T), path), parent);
            var component = uiObj.GetComponent<T>();

            if (component == null)
                Debug.LogErrorFormat("UIManager.InstantiateView There is no {0} script attached on {1} Prefab", typeof(T), uiObj);

            return component;
        }
        
        public GameObject InstantiateView(GameObject viewPrefab, Transform parent = null)
        {
            if (viewPrefab == null)
                Debug.LogErrorFormat("UIManager.InstantiateView InstantiateView viewPrefab is null");

            if (parent == null)
                parent = _uiParent;

            GameObject view = GameObject.Instantiate(viewPrefab, parent, false);

            return view;
        }
        
        public string GetViewName(Type type)
        {
            var name = type.Name;
            return name.Substring(0, name.Length - VIEW_SUFFIX.Length);
        }

        protected virtual string GetViewPath(Type type, string customPath)
        {
            return customPath + GetViewName(type);
        }
    }
}