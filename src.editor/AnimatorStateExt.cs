using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SystemEx;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorEx.src.editor.Templates;
using UnityEngine;



namespace UnityEditorEx
{
	public static class AnimatorStateExt
	{
		[MenuItem("CONTEXT/AnimatorState/Create And Add State Script")]
		static void CreateAndAddStateScript(MenuCommand command)
		{
			AnimatorState state = command.context as AnimatorState;
			string scriptPath = Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(state)), state.name + "State.cs");
			File.WriteAllText(Path.GetFullPath(scriptPath)
				, Template.TransformToText<StateMachineState_cs>(new Dictionary<string, object>
					{
						{ "namespacename", UnityEditorExSettings.instance.namespaceName },
						{ "statename", state.name + "State" },
						{ "statemachinetype", "StateMachineType" },
						{ "controllertype", "ControllerType" },
						{ "partial", true }
					}));
			AssetDatabase.ImportAsset(scriptPath);
			AssetDatabase.Refresh();

			MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
			script.SetScriptTypeWasJustCreatedFromComponentMenu();

			int behaviourId = AnimatorController.CreateStateMachineBehaviour(script);
			typeof(AnimatorState).GetMethod("AddBehaviour", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(state, new object[] { behaviourId });
		}


		[MenuItem("CONTEXT/Transform/Add GameObject Script")]
		public static void AddGameObjectScript(MenuCommand command)
		{
			Transform transfrom = command.context as Transform;
			string scriptPath = Path.Combine(ProjectBrowserExt.GetSelectedPath(), transfrom.gameObject.name + ".cs");

			File.WriteAllText(Path.GetFullPath(scriptPath)
					, Template.TransformToText<DerivedClass_cs>(new Dictionary<string, object>
						{
							{ "namespacename", UnityEditorExSettings.instance.namespaceName },
							{ "classname", transfrom.gameObject.name },
							{ "baseclassname", "MonoBehaviour" },
						}));

			AssetDatabase.ImportAsset(scriptPath);
			AssetDatabase.Refresh();

			MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
			script.SetScriptTypeWasJustCreatedFromComponentMenu();
			InternalEditorUtilityEx.AddScriptComponentUncheckedUndoable(transfrom.gameObject, script);
		}
	}
}