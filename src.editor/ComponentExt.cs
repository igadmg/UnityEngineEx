using System.Collections.Generic;
using System.IO;
using SystemEx;
using UnityEditor;
using UnityEditorEx.src.editor.Templates;
using UnityEngine;



namespace UnityEditorEx
{
    public static class ComponentExt
    {
        [MenuItem("CONTEXT/Component/Create Editor Script")]
        static void CreateEditorScriptForComponent(MenuCommand command)
        {
            var component = command.context as Component;

            if (component != null)
            {
                string editorPath = Path.Combine(Application.dataPath, "Editor");
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
                if (!File.Exists(editorScriptPath))
                {
                    File.WriteAllText(editorScriptPath, Template.TransformToText<ComponentEditor_cs>(parameters));
                }
            }
        }
    }
}
