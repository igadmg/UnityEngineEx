using System.Linq;
using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
	public static class ToolsEx
	{
		static MethodInfo m_LockHandlePosition = null;
		static MethodInfo m_UnlockHandlePosition = null;



		static ToolsEx()
		{
			var type = typeof(Editor).Assembly.GetTypes().Where(t => t.Name == "Tools").FirstOrDefault();
			if (type != null)
			{
				m_LockHandlePosition = type.GetMethod("LockHandlePosition", BindingFlags.Static | BindingFlags.NonPublic);
				m_UnlockHandlePosition = type.GetMethod("UnlockHandlePosition", BindingFlags.Static | BindingFlags.NonPublic);
			}
		}

		public static void LockHandlePosition()
		{
			m_LockHandlePosition.Invoke(null, null);
		}

		public static void UnlockHandlePosition()
		{
			m_UnlockHandlePosition.Invoke(null, null);
		}
	}
}
