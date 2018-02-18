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
                && animator.runtimeAnimatorController == null;
        }

        [MenuItem("CONTEXT/Animator/Create And Add or Update Animator Script")]
        static void CreateAndAddAnimatorScript(MenuCommand command)
        {
            var animator = command.context as Animator;
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(animator.runtimeAnimatorController));

            string animatorControllerName = controller.name.Replace("AnimatorController", "");
            List<string> floats = new List<string>();
            List<string> ints = new List<string>();
            List<string> bools = new List<string>();
            List<string> triggers = new List<string>();

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

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "animatorcontrollername", animatorControllerName },
                { "floats", floats },
                { "ints", ints },
                { "bools", bools },
                { "triggers", triggers },
            };

            var animatorControllerBehaviour = animator.gameObject.GetComponent(animatorControllerName + "AnimatorController") as MonoBehaviour;
            MonoScript animatorControllerScript = MonoScript.FromMonoBehaviour(animatorControllerBehaviour);
            string animationControllerPath = animatorControllerScript == null
                ? Path.GetDirectoryName(AssetDatabase.GetAssetPath(controller))
                : Path.GetDirectoryName(AssetDatabase.GetAssetPath(animatorControllerScript));
            string animationControllerScriptPath = Path.Combine(animationControllerPath, animatorControllerName + "AnimatorController.cs");
            File.WriteAllText(Path.GetFullPath(animationControllerScriptPath), Template.TransformToText<AnimatorController_cs>(parameters));

            AssetDatabase.ImportAsset(animationControllerScriptPath);
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(animationControllerScriptPath);
            AssetDatabase.Refresh();

            script.GetType().GetMethod("SetScriptTypeWasJustCreatedFromComponentMenu", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(script, null);
            if (animatorControllerBehaviour == null)
            {
                typeof(UnityEditorInternal.InternalEditorUtility).GetMethod("AddScriptComponentUncheckedUndoable", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { animator.gameObject, script });
            }
        }
    }
}