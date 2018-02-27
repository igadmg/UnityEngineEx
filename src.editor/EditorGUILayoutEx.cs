using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorEx
{
    public static class EditorGUILayoutEx
    {
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
