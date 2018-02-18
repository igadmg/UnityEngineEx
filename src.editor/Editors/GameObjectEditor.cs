using UnityEditor;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx
{
    //[CustomEditor(typeof(GameObject))]
    public class GameObjectEditor : Editor<GameObject>
    {
        private string m_Namespace;
        private string m_ScriptName;
        private string m_Path;

        public override void OnInspectorGUI()
        {
            ButtonDropWindow.Show("Create Component", (position, styles) =>
            {
                if (string.IsNullOrEmpty(m_Namespace))
                {
                    foreach (MonoBehaviour behaviour in target.GetComponents<MonoBehaviour>())
                    {
                        string ns = behaviour.GetType().Namespace;
                        if (!string.IsNullOrEmpty(ns) && !ns.StartsWith("Unity"))
                        {
                            m_Namespace = ns;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(m_Namespace))
                    {
                        m_Namespace = Application.productName;
                    }
                }

                if (string.IsNullOrEmpty(m_ScriptName))
                {
                    m_ScriptName = target.name;
                }

                //if (string.IsNullOrEmpty(m_Path))
                {
                    m_Path = ProjectBrowserExt.GetSelectedPath();

                    int i = m_Path.IndexOf('/');
                    m_Path = m_Path.Remove(0, i > 0 ? i + 1 : m_Path.Length);
                }

                GUI.Label(new Rect(0.0f, 0.0f, position.width, position.height), GUIContent.none, styles.background);

                var rect = position;
                rect.x = +1f;
                rect.y = +0f;
                rect.width -= 2f;
                //rect.height -= 2f;

                using (GUILayoutEx.Area(rect))
                {
                    rect = GUILayoutUtility.GetRect(10f, 25f);
                    GUI.Label(rect, "New Script", styles.header);

                    GUILayout.Label("Namespace:");
                    m_Namespace = EditorGUILayout.TextField(m_Namespace);

                    GUILayout.Label("Name:");
                    m_ScriptName = EditorGUILayout.TextField(m_ScriptName);

                    GUILayout.Label("Path:");
                    if (EditorGUILayout.DropdownButton(new GUIContent(m_Path), FocusType.Keyboard))
                    {
                    /*
                    GenericMenu menu = new GenericMenu();

                    int num = Application.dataPath.Split(Path.DirectorySeparatorChar).Count() - 1;
                    menu.AddItem(
                        new GUIContent(
                            string.Join("/", Application.dataPath.Split(Path.DirectorySeparatorChar).Skip(num).ToArray()))
                            , false, null);
                    menu.ShowAsContext();
                    */
                    }

                    if (GUILayout.Button("Create and Add"))
                    {

                    }
                }
            });
        }
    }
}