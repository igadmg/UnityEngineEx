using UnityEditor;
using UnityEditor.SceneManagement;


namespace UnityEditorEx
{
	public class EditorExt
	{
		[MenuItem("File/Save All %&s")]
		private static void SaveAll()
		{
			AssetDatabase.SaveAssets();
			EditorSceneManager.SaveOpenScenes();
		}


		[MenuItem("GameObject/Copy path to clipboard %&c", true)]
		private static bool ValidateCopyPathToClipboard()
		{
			return Selection.activeGameObject != null;
		}

		[MenuItem("GameObject/Copy path to clipboard %&c")]
		private static void CopyPathToClipboard()
		{
			string path = "";
			var current = Selection.activeGameObject.transform;

			while (current != null)
			{
				path = current.name + (path.Length > 0 ? "/" + path : "");
				current = current.parent;
			}

			EditorGUIUtility.systemCopyBuffer = path;
		}
	}
}
