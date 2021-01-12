using System;
using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
	public static class ProjectWindowUtilEx
	{
		private static Lazy<MethodInfo> m_CreateScriptAsset
			= new Lazy<MethodInfo>(() => typeof(ProjectWindowUtil).GetMethod("CreateScriptAsset", BindingFlags.Static | BindingFlags.NonPublic));

		public static void CreateScriptAsset(string templatePath, string destName)
		{
			m_CreateScriptAsset.Value.Invoke(null, new object[] { templatePath, destName });
		}
	}
}
