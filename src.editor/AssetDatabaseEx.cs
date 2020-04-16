using System;
using System.Reflection;
using UnityEditor;

namespace UnityEditorEx
{
	[InitializeOnLoad]
	public static class AssetDatabaseEx
	{
		static private Lazy<MethodInfo> m_GetInstanceIDFromGUID
			= new Lazy<MethodInfo>(() => typeof(AssetDatabase).GetMethod("GetInstanceIDFromGUID", BindingFlags.Static | BindingFlags.NonPublic));
		static private Lazy<MethodInfo> m_LoadMainAssetAtGUID
			= new Lazy<MethodInfo>(() => typeof(AssetDatabase).GetMethod("LoadMainAssetAtGUID", BindingFlags.Static | BindingFlags.NonPublic));


		public static int GetInstanceIDFromGUID(string guid)
		{
			return (int)m_GetInstanceIDFromGUID.Value.Invoke(null, new object[] { (object)guid });
		}

		public static int GetInstanceIDFromAssetPath(string path)
		{
			return GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID(path));
		}

		[Obsolete("Use AssetDatabase.LoadMainAssetAtPath instead.")]
		public static UnityEngine.Object LoadMainAssetAtGUID(GUID guid)
		{
			return (UnityEngine.Object)m_LoadMainAssetAtGUID.Value.Invoke(null, new object[] { guid });
		}
	}
}