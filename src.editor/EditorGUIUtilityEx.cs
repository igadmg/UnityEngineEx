using System;
using System.Reflection;
using SystemEx;
using UnityEditor;
using UnityEngine;

namespace UnityEditorEx
{
	public static class EditorGUIUtilityEx
	{
		static Lazy<MethodInfo> m_LoadIconRequired = new Lazy<MethodInfo>(() =>
			typeof(EditorGUIUtility).GetMethod("LoadIconRequired", BindingFlags.Static | BindingFlags.NonPublic));

		public static IDisposable LabelWidth(float newWidth)
		{
			float oldWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = newWidth;

			return DisposableLock.Lock(() => EditorGUIUtility.labelWidth = oldWidth);
		}
		public static Texture2D LoadIconRequired(string name)
			=> (Texture2D)m_LoadIconRequired.Value.Invoke(null, new object[] { name });
	}
}
