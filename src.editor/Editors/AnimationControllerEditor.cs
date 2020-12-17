using System.IO;
using System.Linq;
using SystemEx;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorEx.src.editor.Templates;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx
{
	[CustomEditor(typeof(AnimatorController))]
	public class AnimationControllerEditor : Editor<AnimatorController>
	{
		private string m_ScriptName = null;
		private string m_Path = null;
		private GameObject m_GameObject;
		private Component m_Behaviour;



		public override void OnInspectorGUI()
		{
			AnimatorController ac = target;

			ButtonDropWindow.Show("Add State Machine", () => {
				if (string.IsNullOrEmpty(m_ScriptName))
				{
					m_ScriptName = ac.name;
				}
				if (string.IsNullOrEmpty(m_Path))
				{
					if (Selection.activeObject != null)
					{
						m_Path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
					}
				}
			},
				(position, styles) => {
					Event e = Event.current;
					switch (e.type)
					{
						case EventType.KeyDown:
							if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
							{
								string scriptPath = Path.Combine(m_Path, m_ScriptName + ".cs");
								if (!File.Exists(scriptPath))
								{
									CreateScript(scriptPath);

									ButtonDropWindow.Close();
								}
							}
							break;
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
						GUI.Label(rect, "New State Machine", styles.header);

						using (GUILayoutEx.Vertical())
						{
							GUILayout.Label("Script Name:");
							m_ScriptName = EditorGUILayout.TextField(m_ScriptName);

							GUILayout.Label("Path:");
							if (EditorGUILayout.DropdownButton(new GUIContent(m_Path), FocusType.Keyboard))
							{
								GenericMenu menu = new GenericMenu();

								int num = Application.dataPath.Split(Path.DirectorySeparatorChar).Count() - 1;
								menu.AddItem(
									new GUIContent(
										string.Join("/", Application.dataPath.Split(Path.DirectorySeparatorChar).Skip(num).ToArray()))
										, false, null);
								menu.ShowAsContext();
							}


							GUILayout.Label("GameObject:");
							m_GameObject = (GameObject)EditorGUILayout.ObjectField(m_GameObject, typeof(GameObject), true);

							if (m_GameObject != null)
							{
								GUILayout.Label("Behaviour:");
								if (EditorGUILayout.DropdownButton(new GUIContent(m_Behaviour != null ? m_Behaviour.GetType().Name : "<none>"), FocusType.Keyboard))
								{
									GenericMenu menu = new GenericMenu();

									foreach (Component c in m_GameObject.GetComponents<Component>())
									{
										menu.AddItem(new GUIContent(c.GetType().Name), m_Behaviour == c, OnBehaviourSelected, c);
									}
									menu.ShowAsContext();
								}
							}
						}
					}
				});
		}

		private void OnBehaviourSelected(object o)
		{
			m_Behaviour = (Component)o;
		}

		private void CreateScript(string scriptPath)
		{
			File.WriteAllText(Path.GetFullPath(scriptPath)
				, Template.TransformToText<BaseStateMachine_cs>(new {
					namespacename = UnityEditorExSettings.instance.GetNamespaceName(scriptPath),
					typename = m_ScriptName,
				}.ToExpando()));

			AssetDatabase.ImportAsset(scriptPath);
			AssetDatabase.Refresh();

			MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
			script.SetScriptTypeWasJustCreatedFromComponentMenu();

			InternalEditorUtilityEx.AddScriptComponentUncheckedUndoable(m_GameObject, script);
		}
	}
}
