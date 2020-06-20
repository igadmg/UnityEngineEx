using UnityEditor;



namespace UnityEditorEx
{
	public static class UndoEx
	{
		public static void RegisterCreatedObjectUndo(string name, params UnityEngine.Object[] objectsToUndo)
		{
			foreach (UnityEngine.Object o in objectsToUndo)
			{
				Undo.RegisterCreatedObjectUndo(o, name);
			}
		}
	}
}
