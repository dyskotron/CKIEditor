using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AssetUsageFinder
{
    [Serializable]
    internal class SearchTarget
    {
        public Object Target;
        public Object[] Nested;
        public Object Root;
        public Scene Scene;

        public SearchTarget(Object target, string scenePath = null)
        {
            Assert.IsNotNull(target, "Asset you're trying to search is corrupted");
            Target = target;

            if (string.IsNullOrEmpty(scenePath))
            {
                var path = AssetDatabase.GetAssetPath(Target);
                if (AssetDatabase.IsSubAsset(Target))
                {
                    Root = AssetDatabase.LoadMainAssetAtPath(path);
                    Nested = new[] {Target};
                }
                else
                {
                    if (target is SceneAsset)
                        Nested = new[] {Target};
                    else
                        Nested = AssetDatabase.LoadAllAssetsAtPath(path);
                }
            }
            else
            {
                if (Target is GameObject)
                {
                    var gg = (GameObject) Target;
                    Nested = gg.GetComponents<Component>().OfType<Object>().ToArray();
                    Scene = SceneManager.GetSceneByPath(scenePath);
                }
                else if (Target is Component)
                {
                    Nested = new[] {Target};
                    var comp = (Component) Target;
                    Root = comp.gameObject;
                    Scene = SceneManager.GetSceneByPath(scenePath);
                }
                else
                {
                    Root = Target;
                    Scene = SceneManager.GetSceneByPath(scenePath);
                    Nested = new Object[0];
                }
            }
        }

        public bool Check(Object t)
        {
            var tt = Target == t;
            return t && Nested.Aggregate(tt, (current, o) => current || o == t);
        }
    }
}