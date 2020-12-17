using UnityEditor;
using UnityEngineEx;

namespace UnityEditorEx
{
	public static class GameObjectExt
	{
		[MenuItem("GameObject/Select RootTag GameObject %&r", true)]
		private static bool SelectRootTagGameObjectValidate()
			=> Selection.activeGameObject != null;

		[MenuItem("GameObject/Select RootTag GameObject %&r", false)]
		private static void SelectRootTagGameObject()
		{
			var root = Selection.activeGameObject.GetRootObject();
			if (root != null)
				Selection.activeGameObject = root;
		}
	}
}
