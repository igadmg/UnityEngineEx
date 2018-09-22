using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemEx;
using UnityEditor;
using UnityEditorEx.src.editor.Templates;
using UnityEngine;



namespace UnityEditorEx
{
	public static class ComponentExt
	{
		[MenuItem("CONTEXT/Component/Create or Show Editor Script")]
		static void CreateEditorScriptForComponent(MenuCommand command)
		{
			var component = command.context as Component;

			if (component == null)
				return;

			Editor editor = Editor.CreateEditor(component);

			if (editor != null)
			{
				MonoScript script = MonoScript.FromScriptableObject(editor);
				EditorGUIUtility.PingObject(script);
			}
			else
			{
				string editorPath = UnityEditorExSettings.instance.editorScriptsPath;
				if (!Directory.Exists(editorPath))
				{
					Directory.CreateDirectory(editorPath);
				}

				string componentTypeName = component.GetType().Name;
				Dictionary<string, object> parameters = new Dictionary<string, object>
				{
					{ "componentname", componentTypeName },
				};

				string editorScriptPath = Path.Combine(editorPath, componentTypeName + "Editor.cs");
				string editorScriptFullPath = Path.GetFullPath(editorScriptPath);
				if (!File.Exists(editorScriptFullPath))
				{
					File.WriteAllText(editorScriptFullPath, Template.TransformToText<ComponentEditor_cs>(parameters));

					AssetDatabase.ImportAsset(editorScriptFullPath);
					AssetDatabase.Refresh();
				}
			}
		}
	}
}
