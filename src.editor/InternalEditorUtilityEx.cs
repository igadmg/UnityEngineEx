using System.Reflection;
using UnityEditor;
using UnityEngine;



namespace UnityEditorEx
{
	public static class InternalEditorUtilityEx
	{
		private static MethodInfo m_AddScriptComponentUncheckedUndoable = typeof(UnityEditorInternal.InternalEditorUtility).GetMethod("AddScriptComponentUncheckedUndoable", BindingFlags.Static | BindingFlags.NonPublic);


		public static void AddScriptComponentUncheckedUndoable(GameObject gameObject, MonoScript script)
		{
			m_AddScriptComponentUncheckedUndoable.Invoke(null, new object[] { gameObject, script });
		}
	}
}