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
		private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();
		private Vector2 m_ScrollPosition = Vector2.zero;


		void OnGUI()
		{
			if (scenes.Count == 0)
			{
				FindSceneInProject();
			}

			using (EditorGUILayoutEx.ScrollView(ref m_ScrollPosition))
			{
				List<string> lastFolders = new List<string>();

				bool bRefresh = false;
				foreach (string folder in scenes.Keys.OrderBy(s => s))
				{
					if (!foldouts[folder])
					{
						lastFolders.Add(folder);
						continue;
					}

					using (GUILayoutEx.Horizontal())
					{
						foldouts[folder] = EditorGUILayout.Foldout(foldouts[folder], "{0}:".format(folder)/*, EditorStyles.boldLabel*/);
						if (!bRefresh && GUILayout.Button("Refresh", GUILayout.Width(80)))
						{
							scenes.Clear();
							break;
						}
						bRefresh = true;
					}

					if (foldouts[folder])
					{
						foreach (string sceneName in scenes[folder])
						{
							using (GUILayoutEx.Horizontal())
							{
								if (GUILayout.Button(Path.GetFileName(sceneName), GUILayout.Height(20)))
								{
									if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
									{
										EditorSceneManager.OpenScene(sceneName);
									}
								}
								if (GUILayout.Button(new GUIContent("S", "Show scene in project view."), GUILayout.Width(20)))
								{
									EditorGUIUtility.PingObject(AssetDatabaseEx.LoadMainAssetAtGUID(new GUID(sceneName)));
								}
							}
						}
					}
				}

				foreach (string folder in lastFolders)
				{
					using (GUILayoutEx.Horizontal())
					{
						foldouts[folder] = EditorGUILayout.Foldout(foldouts[folder], "{0}:".format(folder)/*, EditorStyles.boldLabel*/);
						if (!bRefresh && GUILayout.Button("Refresh", GUILayout.Width(80)))
						{
							scenes.Clear();
							break;
						}
						bRefresh = true;
					}
				}
			}
		}

		private void FindSceneInProject()
		{
			scenes.Clear();
			foldouts.Clear();

			string[] guids;

			guids = AssetDatabase.FindAssets("t:Scene");

			for (int i = 0; i < guids.Length; i++)
			{
				guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
				string scenefolderName = Path.GetDirectoryName(guids[i]);

				List<string> items = scenes.GetOrAdd(scenefolderName, name => {
					foldouts.Add(name, true);
					return new List<string>();
				});
				items.Add(guids[i]);
			}
		}
	}
}
