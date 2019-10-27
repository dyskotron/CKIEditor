using System;
using System.Diagnostics;
using AssetUsageFinder.Styles;
using UnityEditor;
using UnityEngine;

namespace AssetUsageFinder
{
    internal class FeedbackPopup : EditorWindow
    {
        public string TweetText;
        [SerializeField] private string _initText;
        private const int MaxTweetLenth = 111;
        private GUIContent _tweetContent;
        [SerializeField] private int _contolId;

        public static FeedbackPopup Init(Rect buttonRect)
        {
            var win = CreateInstance<FeedbackPopup>();
            win.ShowUtility();
            win.titleContent = GUIContent.none;
            win.position = buttonRect;
            return win;
        }

        private void OnEnable()
        {
            Focus();
            _tweetContent = new GUIContent(Style.Instance.TweetLabel.Content) {text = string.Empty};
            TweetText = string.Empty;
            _initText = Style.Instance.TweetLabel.Content.text;
            _serializedObject = new SerializedObject(this);
            _pTextArea = _serializedObject.FindProperty("TweetText");
        }

        [Serializable]
        public class Style
        {
            public ContentStylePair HeaderLabel = new ContentStylePair();
            public ContentStylePair ExperienceLabel = new ContentStylePair();
            public ContentStylePair HappyBtn = new ContentStylePair();
            public ContentStylePair UnhappyBtn = new ContentStylePair();
            public ContentStylePair IdeaToggle = new ContentStylePair();
            public ContentStylePair RateUsBtn = new ContentStylePair();


            private static Style _style;
            public Vector2 WindowSize = Vector2.one;
            public Vector2 WindowSmallerSize = Vector2.one;

            public static Style Instance
            {
                get { return _style ?? (_style = DependencyStyle.Instance.Popup); }
            }

            public GUIStyle VerticalScope = new GUIStyle();
            public GUIStyle ExperienceHorizontalScope = new GUIStyle();
            public GUIStyle TweetArea = new GUIStyle();
            public ContentStylePair TweetLabel = new ContentStylePair();
            public ContentStylePair TweetBtn = new ContentStylePair();
            public ContentStylePair ForumBtn = new ContentStylePair();
            public ContentStylePair EmailBtn = new ContentStylePair();
        }

        private StateEnum _state;
        private SerializedObject _serializedObject;
        private SerializedProperty _pTextArea;

        private enum StateEnum
        {
            Happy, 
            Unhappy,
            Idea,
        }
        
        private void OnGUI()
        {
            var s = Style.Instance;
            using (new EditorGUILayout.VerticalScope(s.VerticalScope))
            {
                EditorGUILayout.LabelField(s.HeaderLabel.Content, s.HeaderLabel.Style);
                EditorGUILayout.LabelField(s.ExperienceLabel.Content, s.ExperienceLabel.Style);
                using (new EditorGUILayout.HorizontalScope(s.ExperienceHorizontalScope))
                {
                    if (GUILayout.Toggle(_state == StateEnum.Happy, s.HappyBtn.Content, s.HappyBtn.Style))
                        _state = StateEnum.Happy;
                    if (GUILayout.Toggle(_state == StateEnum.Unhappy, s.UnhappyBtn.Content, s.UnhappyBtn.Style))
                        _state = StateEnum.Unhappy;
                    if (GUILayout.Toggle(_state == StateEnum.Idea, s.IdeaToggle.Content, s.IdeaToggle.Style))
                        _state = StateEnum.Idea;
                }
                using (new EditorGUILayout.VerticalScope())
                {
                    switch (_state)
                    {
                        case StateEnum.Happy:
                            HappyFooter();
                            break;
                        case StateEnum.Unhappy:
                        case StateEnum.Idea:
                            IdeaFooter();
                            break;
                    }
                }
            }
        }

        private void HappyFooter()
        {
            var style = Style.Instance;
            minSize = style.WindowSize;
            maxSize = style.WindowSize;

            if (GUILayout.Button(style.RateUsBtn.Content, style.RateUsBtn.Style))
                Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/account/downloads/search=#PACKAGES");

            GUILayout.Label(_tweetContent.text, style.TweetLabel.Style);
            _serializedObject.Update();
            _pTextArea.stringValue = EditorGUILayout.TextArea(_pTextArea.stringValue, style.TweetArea);
            if (_contolId > 0)
            {
                GUIUtility.keyboardControl = _contolId;
                GUIUtility.hotControl = _contolId;
                _contolId = -1;
            }
            if (_pTextArea.stringValue.Length > MaxTweetLenth)
            {
                _pTextArea.stringValue = _pTextArea.stringValue.Remove(MaxTweetLenth);
                _contolId = GUIUtility.keyboardControl;
                GUIUtility.keyboardControl = 0;
            }
            _serializedObject.ApplyModifiedProperties();

            _tweetContent.text = string.Format("{0} ({1} characters left):", _initText, MaxTweetLenth - TweetText.Length);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.Space();

                if (GUILayout.Button(style.TweetBtn.Content, style.TweetBtn.Style))
                    Application.OpenURL(string.Format("https://twitter.com/intent/tweet?hashtags=assetusagefinder,unitytips&ref_src=twsrc%5Etfw&related=twitterapi%2Ctwitter&text={0}&tw_p=tweetbutton", TweetText));
            }
        }

        private void IdeaFooter()
        {
            var style = Style.Instance;
            minSize = style.WindowSmallerSize;
            maxSize = style.WindowSmallerSize;

            if (GUILayout.Button(style.EmailBtn.Content, style.EmailBtn.Style))
                Process.Start("mailto:assetusagefinder@gmail.com");

            if (GUILayout.Button(style.ForumBtn.Content, style.ForumBtn.Style))
                Application.OpenURL("https://forum.unity3d.com/threads/released-asset-usage-finder-v2-0.404992/");

        }
    }
}