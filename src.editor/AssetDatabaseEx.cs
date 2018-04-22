using System.Reflection;
using UnityEditor;

namespace UnityEditorEx
{
	public static class AssetDatabaseEx
	{
		static private MethodInfo m_GetInstanceIDFromGUID = typeof(AssetDatabase).GetMethod("GetInstanceIDFromGUID", BindingFlags.Static | BindingFlags.NonPublic);

		public static int GetInstanceIDFromGUID(string guid)
		{
			return (int)m_GetInstanceIDFromGUID.Invoke(null, new object[] { (object)guid });
		}

		public static int GetInstanceIDFromAssetPath(string path)
		{
			return GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID(path));
		}
	}
}