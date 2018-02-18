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

            string animationControllerPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(controller));
            string animationControllerScriptPath = Path.Combine(animationControllerPath, animatorControllerName + "AnimatorController.cs");
            File.WriteAllText(Path.GetFullPath(animationControllerScriptPath), Template.TransformToText<AnimatorController_cs>(parameters));

            AssetDatabase.ImportAsset(animationControllerScriptPath);
            AssetDatabase.LoadAssetAtPath(animationControllerScriptPath, typeof(UnityEngine.Object));
            AssetDatabase.Refresh();
        }
    }
}
