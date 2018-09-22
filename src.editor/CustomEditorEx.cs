using System;
using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
	[InitializeOnLoad]
	public static class CustomEditorEx
	{
		private static FieldInfo m_InspectedType;

		static CustomEditorEx()
		{
			m_InspectedType = typeof(CustomEditor).GetField("m_InspectedType", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public static Type GetInspectedType(this CustomEditor attribute)
		{
			return (Type)m_InspectedType.GetValue(attribute);
		}
	}
}
