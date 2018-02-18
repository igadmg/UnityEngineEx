using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace UnityEditorEx
{
	public class EditorExt : MonoBehaviour
	{
		[MenuItem("File/Save All %&s")]
		static void SaveAll()
		{
			AssetDatabase.SaveAssets();
			EditorSceneManager.SaveOpenScenes();
		}

	
		[MenuItem("GameObject/Copy path to clipboard %&c", true)]
		static bool ValidateCopyPathToClipboard()
		{
			return Selection.activeGameObject != null;
		}

		[MenuItem("GameObject/Copy path to clipboard %&c")]
		static void CopyPathToClipboard()
		{
			string path = "";
			var current = Selection.activeGameObject.transform;

			while (current != null) {
				path = current.name + (path.Length > 0 ? "/" + path : ""); 
				current = current.parent;
			}

			EditorGUIUtility.systemCopyBuffer = path;
		}
	}
}
