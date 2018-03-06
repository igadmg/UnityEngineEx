using System.Linq;
using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
	public static class ToolbarEx
	{
		static FieldInfo m_Get = null;
		static MethodInfo m_Repaint = null;



		static ToolbarEx()
		{
			var type = typeof(Editor).Assembly.GetTypes().Where(t => t.Name == "Toolbar").FirstOrDefault();
			if (type != null)
			{
				m_Get = type.GetField("get", BindingFlags.Static | BindingFlags.Public);
				m_Repaint = type.GetMethod("Repaint");
			}
		}

		public static void Repaint()
		{
			object toolbar = m_Get.GetValue(null);
			if (toolbar != null)
			{
				m_Repaint.Invoke(toolbar, null);
			}
		}
	}
}
