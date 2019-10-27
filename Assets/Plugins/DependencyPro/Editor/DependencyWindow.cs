using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using System.Linq;
using AssetUsageFinder.Styles;
using UnityEditor.SceneManagement;

namespace AssetUsageFinder
{
    internal class DependencyWindow : EditorWindow
    {
        private Vector2 _scrollPos;
        [SerializeField] private DependencyBase _data;
        [SerializeField] private bool _fileMode;
        [SerializeField] private int _enableFeedback;
        private bool _expandFiles = true;
        private bool _expandScenes = true;
        private static GUIContent _sceneIcon;
        private Rect _popupButtonRect;
        private PrevClick _click;
        private float _rowPropWidth;
        private float _labelMaxWidth;
        private List<Action> _postponedActions;
        private const string EditorPrefsKey = "AUF_EnableFeedbackFooter1";

        [Serializable]
        public class Style
        {
            public ContentStylePair LookupBtn = new ContentStylePair();
            public GUIStyle TabBreadcrumb0 = new GUIStyle();
            public GUIStyle TabBreadcrumb1 = new GUIStyle();
            public GUIStyle RowMainAssetBtn = new GUIStyle();
            public Vector2 Size = new Vector2(250f, 800f);
            public ContentStylePair FeedbackPopupBtn = new ContentStylePair();

            public static Style Instance
            {
                get { return _style ?? (_style = DependencyStyle.Instance.DependencyWindow); }
            }

            private static Style _style;
            public GUIStyle RowLabel = new GUIStyle();
        }

        private bool EnableFeedback
        {
            get { return _enableFeedback != 2; }
            set { _enableFeedback = value == false ? 2 : 1; }
        }

        private void OnEnable()
        {
            _postponedActions = new List<Action>();
            _enableFeedback = EditorPrefs.GetInt(EditorPrefsKey);
        }

        private void OnGUI()
        {
            if (_postponedActions == null || _data == null || Event.current.keyCode == KeyCode.Escape)
            {
                _postponedActions = new List<Action>(); 
                _postponedActions.Add(() => Close()); 
                return;
            }
            EditorGUILayout.BeginVertical();
            {
                BreadCrumbs();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                {
                    EditorGUILayout.Space();
                    ShowDependencies(_data.Dependencies);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            Footer();
        }

        private void Footer()
        {

            var style = Style.Instance;

            if (!EnableFeedback)
            {
                return;
            }
            if (GUILayout.Button(style.FeedbackPopupBtn.Content, style.FeedbackPopupBtn.Style))
                FeedbackPopup.Init(_popupButtonRect);

            if (Event.current.type == EventType.Repaint)
            {
                _popupButtonRect = GUILayoutUtility.GetLastRect();
                _popupButtonRect.position += position.position;
            }
        }

        //    [MenuItem("Window/Dependency Window")]
        private static void InitFileWindow()
        {
            var window = CreateInstance<DependencyWindow>();
            window.Init(new FileDependency(Selection.activeObject));
            var p = window.position;
            p.size = Style.Instance.Size;
            window.position = p;
            window.Show();
        }

        private void Init(DependencyBase d)
        {
            _data = d;
            _labelMaxWidth = CalculateContentMaxWidth(EditorStyles.label, _data.Dependencies.SelectMany(dd => dd.Properties.Select(p => p.Content)));
            _rowPropWidth = CalculateContentMaxWidth(EditorStyles.label, _data.Target.Nested.Union(new[] {_data.Target.Root}).Where(o => o).Select(o => new GUIContent((o is ScriptableObject || o is MonoScript) ? o.ToString() : o.name)));

            _fileMode = d is FileDependency;
            var sceneDependency = d as InSceneDependency;
            titleContent = new GUIContent(sceneDependency != null ? "Scene Objects" : "File Usages");
        }

        [MenuItem("Assets/- File Usages", false, 30)]
        private static void ContextMenu()
        {
            InitFileWindow();
        }


        [MenuItem("GameObject/- Usages in Scene", false, -1)]
        private static void FindReferencesToAsset(MenuCommand data)
        {
            var selected = Selection.activeObject;
            if (!selected) return;

            var scenePath = SceneManager.GetActiveScene().path;

            InitSceneWindow(selected, scenePath);
        }

        [MenuItem("CONTEXT/Component/- Component Usages (Scene)", false, 159)]
        private static void FindReferencesToComponent(MenuCommand data)
        {
            Object selected = data.context;
            if (!selected) return;

            var scenePath = SceneManager.GetActiveScene().path;

            InitSceneWindow(selected, scenePath);
        }

        private static void InitSceneWindow(Object target, string scenePath)
        {
            DependencyWindow window = CreateInstance<DependencyWindow>();
            window.Init(new InSceneDependency(target, scenePath));
            window.Show();
        }

        private const string FileDependencies = "Used in following files:";
        private const string SceneDependencies = "Used in current scene:";

        private void BreadCrumbs()
        {
            var parents = _data.Parents();
            parents.Reverse();
            var w = 0f;
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int i = 0; i < parents.Count; i++)
                    {
                        var parent = parents[i];
                        var style = i == 0 ? Style.Instance.TabBreadcrumb0 : Style.Instance.TabBreadcrumb1;

                        var styleWidth = style.CalcSize(parent.TabContent).x;
                        if (w > EditorGUIUtility.currentViewWidth - styleWidth)
                        {
                            w = 0f;
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                        }
                        w += styleWidth;

                        if (i == parents.Count - 1)
                            GUILayout.Toggle(true, parent.TabContent, style);
                        else if (GUILayout.Button(parent.TabContent, style))
                            _postponedActions.Add(() => Init(parent));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void Update()
        {
            if (!_postponedActions.Any()) return;
            foreach (var a in _postponedActions)
                a();
            _postponedActions.Clear();
        }

        private void ShowDependencies(ResultRow[] dependencies)
        {
            _expandFiles = EditorGUILayout.Foldout(_expandFiles, _fileMode ? FileDependencies : SceneDependencies);
            if (!_fileMode)
            {
                if (_data.Target.Scene.IsValid() && !_data.Target.Scene.isLoaded)
                    return; 
            }
            if (_expandFiles)
            {
                if (dependencies.Any())
                {
                    foreach (var dependency in dependencies)
                        DrawRow(dependency);
                }
                else
                {
                    EditorGUILayout.LabelField("No file dependencies found.");
                }
            }
            EditorGUILayout.Space();

            var fileDep = _data as FileDependency;
            if (fileDep == null)
                return;

            if (fileDep.ScenePaths == null)
            {
                if (GUILayout.Button("Search Scenes"))
                    fileDep.ScenePaths = FindDependencies.ScenesThatContain(_data.Target.Target).Select(p => new FileDependency.Pair { Path = p, NicifiedPath = p.Replace("Assets/", string.Empty) }).ToArray();
                return;
            }

            _expandScenes = EditorGUILayout.Foldout(_expandScenes, "Scenes:");

            if (!_expandScenes) return;

            if (!fileDep.ScenePaths.Any())
            {
                EditorGUILayout.LabelField("No scene dependencies found.");
                return;
            }

            for (int i = 0; i < fileDep.ScenePaths.Length; i++)
            {
                var p = fileDep.ScenePaths[i];
                using (new EditorGUILayout.HorizontalScope())
                {
                    SceneIcon.text = p.NicifiedPath;

                    if (GUILayout.Button(SceneIcon, EditorStyles.label, GUILayout.Height(16f)))
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(p.Path);

                    if (!GUILayout.Button("Open scene & search", GUILayout.Width(200f)))
                        continue;

                    var s = SceneManager.GetSceneByPath(p.Path);
                    if (s.isLoaded)
                    {
                        InitSceneWindow(_data.Target.Target, p.Path);
                    }
                    else
                    {
                        if (s.isDirty && !EditorUtility.DisplayDialog("Unsaved Scene", "You have unsaved shanges in scene", "Discard scene", "Cancel search"))
                            return;

                        EditorSceneManager.OpenScene(p.Path);
                        EditorSceneExtensions.FireOnSceneOpenAndForget(() => InitSceneWindow(_data.Target.Target, p.Path));
                    }
                }
            }
        }

        private struct PrevClick
        {
            private Object _target;
            private float _timeClicked;

            public PrevClick(Object target)
            {
                _target = target;
                _timeClicked = Time.realtimeSinceStartup;
            }

            private const float DoubleClickTime = 0.5f;

            public bool IsDoubleClick(Object o)
            {
                return _target == o && Time.realtimeSinceStartup - _timeClicked < DoubleClickTime;
            }
        }


        private void DrawRow(ResultRow dependency)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(dependency.LabelContent, Style.Instance.RowMainAssetBtn))
                    {
                        if (_click.IsDoubleClick(dependency.Main))
                            Selection.activeObject = dependency.Main;
                        else
                            EditorGUIUtility.PingObject(dependency.Main);

                        _click = new PrevClick(dependency.Main);
                    }
                    if (GUILayout.Button(Style.Instance.LookupBtn.Content, Style.Instance.LookupBtn.Style))
                    {
                        _postponedActions.Add(() =>
                            Init(_data.Nest(dependency.Main)));
                    }
                }
                dependency.SerializedObject.Update();
                EditorGUI.BeginChangeCheck();
                if (dependency.Target)
                {
                    foreach (var prop in dependency.Properties)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            var locked = prop.Property.objectReferenceValue is MonoScript;
                            var f = GUI.enabled;

                            if (locked) GUI.enabled = false;

                            EditorGUILayout.LabelField(prop.Content, Style.Instance.RowLabel, GUILayout.MaxWidth(_labelMaxWidth));
                            EditorGUILayout.PropertyField(prop.Property, GUIContent.none, true, GUILayout.MinWidth(_rowPropWidth));

                            if (locked) GUI.enabled = f;
                        }
                    }
                }

                if (EditorGUI.EndChangeCheck())
                    dependency.SerializedObject.ApplyModifiedProperties();
            }
        }


        private static float CalculateContentMaxWidth(GUIStyle rowStyle, IEnumerable<GUIContent> guiContents)
        {
            var maxWidth = 0f;
            foreach (var guiContent in guiContents)
            {
                float min, max;
                rowStyle.CalcMinMaxWidth(guiContent, out min, out max);
                maxWidth = Mathf.Max(maxWidth, max);
            }
            return maxWidth;
        }

        private static GUIContent SceneIcon
        {
            get { return _sceneIcon ?? (_sceneIcon = new GUIContent(AssetPreview.GetMiniTypeThumbnail(typeof (SceneAsset)))); }
        }

        internal static class EditorSceneExtensions
        {
            private static Action _delayedAction;

            public static void FireOnSceneOpenAndForget(Action a)
            {
                _delayedAction = a;
                EditorApplication.hierarchyWindowChanged += Callback;
            }

            private static void Callback()
            {
                EditorApplication.hierarchyWindowChanged -= Callback;
                _delayedAction();
                _delayedAction = null;
            }
        }
    }
}