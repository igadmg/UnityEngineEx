using System;
using UnityEditor;
using UnityEngine;



namespace UnityEditorEx
{
    public class ButtonDropWindow : EditorWindow
    {
        static ButtonDropWindow _instance;
        static Styles _styles;

        string _name;
        Action<Rect, Styles> _onGui;

        public static void Show(string buttonLabel, Action onInit, Action<Rect, Styles> onGui)
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<ButtonDropWindow>();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUIStyle style = (GUIStyle)"AC Button";
			GUIContent buttonNameLabel = new GUIContent("Add Component");
			Rect rect = GUILayoutUtility.GetRect(buttonNameLabel, style);

            if (EditorGUI.DropdownButton(rect, buttonNameLabel, FocusType.Passive, style))
            {
                //rect.y += 26f;
                //rect.x += rect.width;
                //rect.width = style.fixedWidth;
                _instance.Init(rect, onInit, onGui);
                _instance.Repaint();
            }
			
			GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void Init(Rect rect, Action onInit, Action<Rect, Styles> onGui)
        {
			rect = rect.GUIToScreenRect();

			onInit();
			_onGui = onGui;
            ShowAsDropDown(rect, new Vector2(rect.width, 320f));
            Focus();
			wantsMouseMove = true;
        }

        void OnGUI()
        {
            if (_styles == null)
            {
                _styles = new Styles();
            }

			_onGui(position, _styles);
        }

        public class Styles
        {
            public GUIStyle header = new GUIStyle((GUIStyle)"In BigTitle");
            public GUIStyle componentButton = new GUIStyle((GUIStyle)"PR Label");
            public GUIStyle background = (GUIStyle)"grey_border";
            public GUIStyle previewBackground = (GUIStyle)"PopupCurveSwatchBackground";
            public GUIStyle previewHeader = new GUIStyle(EditorStyles.label);
            public GUIStyle previewText = new GUIStyle(EditorStyles.wordWrappedLabel);
            public GUIStyle rightArrow = (GUIStyle)"AC RightArrow";
            public GUIStyle leftArrow = (GUIStyle)"AC LeftArrow";
            public GUIStyle groupButton;

            public Styles()
            {
                this.header.font = EditorStyles.boldLabel.font;
                this.componentButton.alignment = TextAnchor.MiddleLeft;
                this.componentButton.padding.left -= 15;
                this.componentButton.fixedHeight = 20f;
                this.groupButton = new GUIStyle(this.componentButton);
                this.groupButton.padding.left += 17;
                this.previewText.padding.left += 3;
                this.previewText.padding.right += 3;
                ++this.previewHeader.padding.left;
                this.previewHeader.padding.right += 3;
                this.previewHeader.padding.top += 3;
                this.previewHeader.padding.bottom += 2;
            }
        }
    }
}