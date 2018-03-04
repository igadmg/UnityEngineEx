using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityEditorEx
{
    public static class EditorGUILayoutEx
	{
		static FieldInfo s_LastRect = typeof(EditorGUILayout).GetField("s_LastRect", BindingFlags.Static | BindingFlags.NonPublic);

		public static Rect GetLastRect()
		{
			return (Rect)s_LastRect.GetValue(null);
		}

		public static IDisposable ScrollView(ref Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			return new EditorGUILayoutScrollView(ref scrollPosition, options);
		}
	}

	internal class EditorGUILayoutScrollView : IDisposable
	{
		public EditorGUILayoutScrollView(ref Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, options);
		}

		public void Dispose()
		{
			EditorGUILayout.EndScrollView();
		}
	}
}
