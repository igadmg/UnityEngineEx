using System;
using System.Collections.Generic;
using System.IO;
using SystemEx;
using UnityEditor;
using UnityEditorEx.src.editor.Templates;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx
{
	[Spyglass(typeof(GameObject), true)]
	public class GameObjectSpyglass : Editor<GameObject>, ISpyglassEditor
	{
		private string m_Namespace;
		private string m_ScriptName;
		private string m_Path;

		public void OnSpyglassGUI()
		{
			using (EditorGUILayoutEx.Vertical())
			{
				ButtonDropWindow.Show("Create Component", () => InitButtonDropWindow(),
					(position, styles) =>
					{
						Event e = Event.current;
						switch (e.type)
						{
							case EventType.KeyDown:
								OnKeyDown(e);
								break;
						}

						OnGui(position, styles);
					});

				var cameraTransform = UnityEditor.SceneView.lastActiveSceneView.camera.transform;
				var delta = target.transform.position - cameraTransform.position;
				EditorGUILayout.SelectableLabel($"{delta.x}, {delta.y}, {delta.z} : {delta.magnitude}");
			}
		}

		private void InitButtonDropWindow()
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
					m_Namespace = UnityEditorExSettings.instance.namespaceName;
				}
			}

			if (string.IsNullOrEmpty(m_ScriptName))
			{
				m_ScriptName = target.name;
			}

			//if (string.IsNullOrEmpty(m_Path))
			{
				m_Path = ProjectBrowserExt.GetSelectedPath();
			}
		}

		private void OnKeyDown(Event e)
		{
			if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
			{
				string scriptPath = Path.Combine(m_Path, m_ScriptName + ".cs");
				if (!File.Exists(scriptPath))
				{
					CreateScript(scriptPath);

					ButtonDropWindow.Close();
				}
			}
		}

		private void OnGui(Rect position, ButtonDropWindow.Styles styles)
		{
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
				m_Path = EditorGUILayout.TextField(m_Path);

				string scriptPath = Path.Combine(m_Path, m_ScriptName + ".cs");
				using (EditorGUIEx.DisabledScopeIf(File.Exists(scriptPath)))
				{
					if (GUILayout.Button("Create and Add"))
					{
						CreateScript(scriptPath);

						ButtonDropWindow.Close();
					}
				}
			}
		}

		private void CreateScript(string scriptPath)
		{
			File.WriteAllText(Path.GetFullPath(scriptPath)
				, Template.TransformToText<DerivedClass_cs>(new Dictionary<string, object> {
						{ "namespacename", UnityEditorExSettings.instance.namespaceName },
						{ "classname", m_ScriptName },
						{ "baseclassname", "MonoBehaviour" },
					}));

			AssetDatabase.ImportAsset(scriptPath);
			AssetDatabase.Refresh();

			MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
			script.SetScriptTypeWasJustCreatedFromComponentMenu();

			InternalEditorUtilityEx.AddScriptComponentUncheckedUndoable(target, script);
		}
	}
}
