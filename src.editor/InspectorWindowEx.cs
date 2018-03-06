using System.Linq;
using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
	public static class InspectorWindowEx
	{
		static MethodInfo m_RepaintAllInspectors = null;



		static InspectorWindowEx()
		{
			var type = typeof(Editor).Assembly.GetTypes().Where(t => t.Name == "InspectorWindow").FirstOrDefault();
			if (type != null)
			{
				m_RepaintAllInspectors = type.GetMethod("RepaintAllInspectors", BindingFlags.Static | BindingFlags.NonPublic);
			}
		}

		public static void RepaintAllInspectors()
		{
			m_RepaintAllInspectors.Invoke(null, null);
		}
	}
}
