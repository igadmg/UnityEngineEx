using System.Reflection;
using UnityEditor;
using UnityEngine;



namespace UnityEditorEx
{
	public static class MonoScriptEx
	{
		public static MethodInfo m_SetScriptTypeWasJustCreatedFromComponentMenu = typeof(MonoScript).GetMethod("SetScriptTypeWasJustCreatedFromComponentMenu", BindingFlags.Instance | BindingFlags.NonPublic);

		public static string GetComponentScriptPath(MonoBehaviour behaviour)
		{
			MonoScript script = behaviour != null ? MonoScript.FromMonoBehaviour(behaviour) : null;
			return script != null
				? AssetDatabase.GetAssetPath(script)
				: null;
		}

		public static string GetComponentScriptPath(ScriptableObject scriptableObject)
		{
			MonoScript script = scriptableObject != null ? MonoScript.FromScriptableObject(scriptableObject) : null;
			return script != null
				? AssetDatabase.GetAssetPath(script)
				: null;
		}

		public static void SetScriptTypeWasJustCreatedFromComponentMenu(this MonoScript script)
		{
			m_SetScriptTypeWasJustCreatedFromComponentMenu.Invoke(script, null);
		}
	}
}
