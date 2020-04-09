using System;
using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
	[InitializeOnLoad]
	public static class CustomEditorEx
	{
		private static Lazy<FieldInfo> m_InspectedType
			= new Lazy<FieldInfo>(() => typeof(CustomEditor).GetField("m_InspectedType", BindingFlags.Instance | BindingFlags.NonPublic));

		public static Type GetInspectedType(this CustomEditor attribute)
		{
			return (Type)m_InspectedType.Value.GetValue(attribute);
		}
	}
}
