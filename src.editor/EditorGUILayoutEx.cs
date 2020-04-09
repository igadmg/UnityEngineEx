using System;
using System.Reflection;
using SystemEx;
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
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, options);
			return new DisposableLock(() => EditorGUILayout.EndScrollView());
		}

		public static IDisposable Horizontal()
		{
			EditorGUILayout.BeginHorizontal();
			return new DisposableLock(() => EditorGUILayout.EndHorizontal());
		}

		public static IDisposable Vertical()
		{
			EditorGUILayout.BeginVertical();
			return new DisposableLock(() => EditorGUILayout.EndVertical());
		}
	}
}
