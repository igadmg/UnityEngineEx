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
	public static class AnimatorExt
	{
		[MenuItem("CONTEXT/Animator/Create And Set Animator Controller", true)]
		static bool ValidateCreateAndSetAnimatorController(MenuCommand command)
		{
			Animator animator = command.context as Animator;

			return animator != null
				&& animator.runtimeAnimatorController == null;
		}

		[MenuItem("CONTEXT/Animator/Create And Set Animator Controller")]
		static void CreateAndSetAnimatorController(MenuCommand command)
		{
			Animator animator = command.context as Animator;
			string path = ProjectBrowserExt.GetSelectedPath();
			string name = Selection.activeObject.name.Replace(" ", "") + "AnimatorController";

			path = Path.Combine(path, name + ".controller");
			AnimatorController animatorController = new AnimatorController();
			animatorController.name = Path.GetFileName(path);
			AssetDatabase.CreateAsset(animatorController, path);
			//animatorController.pushUndo = false;
			animatorController.AddLayer("Base Layer");
			//animatorController.pushUndo = true;
			AssetDatabase.ImportAsset(path);

			animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(path);
		}

		[MenuItem("CONTEXT/Animator/Create And Add or Update Animator Script", true)]
		static bool ValidateCreateAndAddAnimatorScript(MenuCommand command)
		{
			Animator animator = command.context as Animator;

			return animator != null
				&& animator.runtimeAnimatorController != null;
		}

		[MenuItem("CONTEXT/Animator/Create And Add or Update Animator Script")]
		static void CreateAndAddAnimatorScript(MenuCommand command)
		{
			var animator = command.context as Animator;
			var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(animator.runtimeAnimatorController));

			MonoScript script;
			string animatorControllerName = controller.name.Replace("AnimatorController", "");
			if (CreateAnimatorScript<BaseAnimatorController_cs>(animator, controller, animatorControllerName, "AnimatorController", out script))
			{
				InternalEditorUtilityEx.AddScriptComponentUncheckedUndoable(animator.gameObject, script);
			}
		}

		[MenuItem("CONTEXT/Animator/Create And Add or Update State Machine Script", true)]
		static bool ValidateCreateAndAddStateMachineScript(MenuCommand command)
		{
			Animator animator = command.context as Animator;

			return animator != null
				&& animator.runtimeAnimatorController != null;
		}

		[MenuItem("CONTEXT/Animator/Create And Add or Update State Machine Script")]
		static void CreateAndAddStateMachineScript(MenuCommand command)
		{
			var animator = command.context as Animator;
			var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(animator.runtimeAnimatorController));

			MonoScript script;
			string stateMachineName = controller.name.Replace("StateMachine", "");
			if (CreateAnimatorScript<BaseStateMachine_cs>(animator, controller, stateMachineName, "StateMachine", out script))
			{
				InternalEditorUtilityEx.AddScriptComponentUncheckedUndoable(animator.gameObject, script);
			}
		}

		public static bool CreateAnimatorScript<TemplateType>(Animator animator, AnimatorController controller, string typeName, string postfix, out MonoScript script)
			 where TemplateType : new()
		{
			bool bNewScript = false;
			string scriptPath = MonoScriptEx.GetComponentScriptPath(animator.gameObject.GetComponent(typeName + postfix) as MonoBehaviour);
			if (scriptPath == null)
			{
				bNewScript = true;
				scriptPath = Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(controller)), typeName + postfix + ".cs");
			}

			AnimatorControllerExt.CreateAnimatorControllerScript<TemplateType>(controller, typeName, postfix, scriptPath, bNewScript);
			script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);

			if (bNewScript)
			{
				script.SetScriptTypeWasJustCreatedFromComponentMenu();
			}

			return bNewScript;
		}
	}
}
