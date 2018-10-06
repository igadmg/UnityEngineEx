using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityEditorEx
{
	[InitializeOnLoad]
	public static class AssetDatabaseEx
	{
		static private MethodInfo m_GetInstanceIDFromGUID;
		static private MethodInfo m_LoadMainAssetAtGUID;

		static AssetDatabaseEx()
		{
			m_GetInstanceIDFromGUID = typeof(AssetDatabase).GetMethod("GetInstanceIDFromGUID", BindingFlags.Static | BindingFlags.NonPublic);
			m_LoadMainAssetAtGUID = typeof(AssetDatabase).GetMethod("LoadMainAssetAtGUID", BindingFlags.Static | BindingFlags.NonPublic);
		}



		public static int GetInstanceIDFromGUID(string guid)
		{
			return (int)m_GetInstanceIDFromGUID.Invoke(null, new object[] { (object)guid });
		}

		public static int GetInstanceIDFromAssetPath(string path)
		{
			return GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID(path));
		}

		public static UnityEngine.Object LoadMainAssetAtGUID(GUID guid)
		{
			return (UnityEngine.Object)m_LoadMainAssetAtGUID.Invoke(null, new object[] { guid });
		}
	}
}