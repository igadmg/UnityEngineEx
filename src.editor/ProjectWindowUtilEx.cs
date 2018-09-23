using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
	public static class ProjectWindowUtilEx
	{
		private static MethodInfo m_CreateScriptAsset;

		static ProjectWindowUtilEx()
		{
			m_CreateScriptAsset = typeof(ProjectWindowUtil).GetMethod("CreateScriptAsset", BindingFlags.Static | BindingFlags.NonPublic);
		}

		public static void CreateScriptAsset(string templatePath, string destName)
		{
			m_CreateScriptAsset.Invoke(null, new object[] { templatePath, destName });
		}
	}
}
