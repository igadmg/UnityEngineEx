using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemEx;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngineEx;

namespace UnityEditorEx
{
    class SceneWindow : EditorWindow
    {
        [MenuItem("Window/Scenes")]
        public static void ShowWindow()
        {
            SceneWindow window = EditorWindow.GetWindow<SceneWindow>(false, "Scenes");
        }



        private Dictionary<string, List<string>> scenes = new Dictionary<string, List<string>>();



        void OnGUI()
        {
            if (scenes.Count == 0)
            {
                FindSceneInProject();
            }

            EditorGUILayout.Space();

            bool bRefresh = false;
            foreach (string folder in scenes.Keys.OrderBy(s => s))
            {
                using (GUILayoutEx.Horizontal())
                {
                    GUILayout.Label("{0}:".format(folder), EditorStyles.boldLabel);
                    if (!bRefresh && GUILayout.Button("Refresh", GUILayout.Width(80)))
                    {
                        scenes.Clear();
                        break;
                    }
                    bRefresh = true;
                }

                foreach (string sceneName in scenes[folder])
                {
                    if (GUILayout.Button(Path.GetFileName(sceneName), GUILayout.Height(20)))
                    {
                        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                        EditorSceneManager.OpenScene(sceneName);
                    }
                }
            }
        }

        private void FindSceneInProject()
        {
            scenes.Clear();

            string[] guids;

            guids = AssetDatabase.FindAssets("t:Scene");

            for (int i = 0; i < guids.Length; i++)
            {
                guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                string scenefolderName = Path.GetDirectoryName(guids[i]);

                if (!scenes.ContainsKey(scenefolderName))
                {
                    scenes.Add(scenefolderName, new List<string>());
                }

                scenes[scenefolderName].Add(guids[i]);
            }
        }
    }
}