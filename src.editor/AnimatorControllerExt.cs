using System.Collections.Generic;
using System.IO;
using SystemEx;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorEx.src.editor.Templates;
using UnityEngine;



namespace UnityEditorEx
{
	public static class AnimatorControllerExt
	{
		[MenuItem("CONTEXT/AnimatorController/Copy State Names")]
		static void CopyAnimatorControllerLayerStateNames()
		{
			var controller = Selection.activeObject as AnimatorController;

			string data = "";
			foreach (AnimatorControllerLayer layer in controller.layers)
			{
				data += "Layer: {0}\n".format(layer.name);

				foreach (ChildAnimatorState state in layer.stateMachine.states)
				{
					data += state.state.name + "\n";
				}
			}

			EditorGUIUtility.systemCopyBuffer = data;
		}

		[MenuItem("CONTEXT/AnimatorController/Create Animation Controller Script")]
		static void CreateAnimationControllerScript()
		{
			var controller = Selection.activeObject as AnimatorController;

			string animatorControllerName = controller.name.Replace("AnimatorController", "");
			string animationControllerPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(controller));
			CreateAnimatorControllerScript<BaseAnimatorController_cs>(controller, animatorControllerName, "AnimatorController", animationControllerPath, false);
		}

		[MenuItem("CONTEXT/AnimatorController/Create State Machine Script")]
		static void CreateAnimationControllerStateMachineScript()
		{
			var controller = Selection.activeObject as AnimatorController;

			string animatorControllerName = controller.name.Replace("StateMachine", "");
			string animationControllerPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(controller));
			CreateAnimatorControllerScript<BaseStateMachine_cs>(controller, animatorControllerName, "StateMachine", animationControllerPath, false);
		}

		public static void CreateAnimatorControllerScript<TemplateType>(AnimatorController controller, string typeName, string postfix, string scriptPath, bool bNewScript)
			 where TemplateType : new()
			=> CreateAnimatorControllerScript<TemplateType>(controller, UnityEditorExSettings.instance.GetNamespaceName(scriptPath), typeName, postfix, scriptPath, bNewScript)

		public static void CreateAnimatorControllerScript<TemplateType>(AnimatorController controller, string namespaceName, string typeName, string postfix, string scriptPath, bool bNewScript)
			 where TemplateType : new()
		{
			List<string> floats = new List<string>();
			List<string> ints = new List<string>();
			List<string> bools = new List<string>();
			List<string> triggers = new List<string>();
			List<string> states = new List<string>();

			foreach (AnimatorControllerParameter parameter in controller.parameters)
			{
				switch (parameter.type)
				{
					case AnimatorControllerParameterType.Float:
						floats.Add(parameter.name);
						break;
					case AnimatorControllerParameterType.Int:
						ints.Add(parameter.name);
						break;
					case AnimatorControllerParameterType.Bool:
						bools.Add(parameter.name);
						break;
					case AnimatorControllerParameterType.Trigger:
						triggers.Add(parameter.name);
						break;
				}
			}

			if (controller.layers.Length > 0)
			{
				foreach (ChildAnimatorState state in controller.layers[0].stateMachine.states)
				{
					states.Add(state.state.name);
				}
			}

			if (bNewScript)
			{
				File.WriteAllText(Path.GetFullPath(scriptPath)
					, Template.TransformToText<DerivedClass_cs>(new Dictionary<string, object>
						{
							{ "namespacename", namespaceName },
							{ "classname", typeName + postfix },
							{ "baseclassname", "Base" + typeName + postfix },
						}));
			}

			string baseScriptPath = Path.Combine(Path.GetDirectoryName(scriptPath), "Base" + typeName + postfix + ".cs");
			File.WriteAllText(Path.GetFullPath(baseScriptPath)
				, Template.TransformToText<TemplateType>(new Dictionary<string, object>
					{
						{ "namespacename", namespaceName },
						{ "typename", typeName },
						{ "floats", floats },
						{ "ints", ints },
						{ "bools", bools },
						{ "triggers", triggers },
						{ "states", states }
					}));
			AssetDatabase.ImportAsset(baseScriptPath);
			AssetDatabase.ImportAsset(scriptPath);
			AssetDatabase.Refresh();
		}
	}
}
