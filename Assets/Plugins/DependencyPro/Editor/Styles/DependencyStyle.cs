using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace AssetUsageFinder.Styles
{
    internal class DependencyStyle : ScriptableObject
    {
        [CustomEditor(typeof (DependencyStyle))]
        private class Editor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();
                base.OnInspectorGUI();
                if (EditorGUI.EndChangeCheck())
                    InternalEditorUtility.RepaintAllViews();
            }
        }

        [SerializeField, HideInInspector] private bool _pro;
        [FormerlySerializedAs("Row")]public DependencyWindow.Style DependencyWindow = new DependencyWindow.Style();
        public FeedbackPopup.Style Popup= new FeedbackPopup.Style();

        public static DependencyStyle Instance
        {
            get
            {
                if (_cache) return _cache;
                return _cache = ByType<DependencyStyle>().FirstOrDefault(s => s._pro == EditorGUIUtility.isProSkin);
            }
        }

        private static DependencyStyle _cache;

        private static IEnumerable<T> ByType<T>() where T : Object
        {
            var typeName = typeof(T).FullName;

            var guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeName));
            if (!guids.Any()) 
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
            Assert.IsTrue(guids.Length > 0, string.Format("No '{0}' assets found", typeName));
            return guids.Select(g => AssetDatabase.GUIDToAssetPath(g)).Select(t => (T)AssetDatabase.LoadAssetAtPath(t, typeof (T)));
        }
    }
}