using System.Collections.Generic;
using System.IO;
using SystemEx;
using UnityEditor;
using UnityEditorEx.src.editor.Templates;
using UnityEngine;


namespace UnityEditorEx
{
	public class ProjectExt
	{
		[MenuItem("Assets/Create/Material From Texture")]
		public static void MaterialFromTexture()
		{
			foreach (Object o in Selection.objects)
			{
				string assetPath = AssetDatabase.GetAssetPath(o);
				assetPath = assetPath.Substring(0, assetPath.LastIndexOf('/'));

				var materail = new Material(Shader.Find("Unlit/Transparent"));
				materail.mainTexture = o as Texture2D;
				AssetDatabase.CreateAsset(materail, assetPath + "/" + o.name + ".mat");
			}
		}

		[MenuItem("Assets/Copy path to clipboard")]
		public static void CopyAssetPathToClipboard()
		{
			foreach (Object o in Selection.objects)
			{
				EditorGUIUtility.systemCopyBuffer = AssetDatabase.GetAssetPath(o);
			}
		}

		[MenuItem("Assets/Create/New C# Script", priority = 0)]
		private static void CreateNewCSharpScript()
		{
			string scriptPath = Path.Combine(ProjectBrowserExt.GetSelectedPath(), "NewBehaviourScript.cs");
			string templatePath = Path.Combine(Path.GetTempPath(), "NewBehaviourScript.cs");

			File.WriteAllText(Path.GetFullPath(templatePath)
				, Template.TransformToText<DerivedClass_cs>(new Dictionary<string, object>
					{
						{ "namespacename", UnityEditorExSettings.instance.namespaceName },
						{ "classname", "#SCRIPTNAME#" },
						{ "baseclassname", "MonoBehaviour" },
					}));

			ProjectWindowUtilEx.CreateScriptAsset(templatePath, scriptPath);
		}
	}
}
