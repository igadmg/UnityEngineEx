using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorEx
{
	public static class EditorGUIEx
	{
		public static IDisposable ChangeCheck(Action action)
		{
			return new EditorGUIChangeCheck(action);
		}

		public static IDisposable DisabledScopeIf(bool isDiasbaled)
		{
			return new EditorGUI.DisabledScope(isDiasbaled);
		}

		public static IDisposable Property(Rect totalPosition, GUIContent label, SerializedProperty property)
		{
			GUIContent content;
			return new EditorGUIProperty(totalPosition, label, property, out content);
		}

		public static IDisposable Property(Rect totalPosition, GUIContent label, SerializedProperty property, out GUIContent content)
		{
			return new EditorGUIProperty(totalPosition, label, property, out content);
		}
	}

	internal class EditorGUIChangeCheck : IDisposable
	{
		Action action;

		internal EditorGUIChangeCheck(Action action)
		{
			this.action = action;
			EditorGUI.BeginChangeCheck();
		}

		public void Dispose()
		{
			if (EditorGUI.EndChangeCheck() && action != null)
			{
				action.Invoke();
			}
		}
	}

	internal class EditorGUIProperty : IDisposable
	{
		internal EditorGUIProperty(Rect totalPosition, GUIContent label, SerializedProperty property, out GUIContent content)
		{
			content = EditorGUI.BeginProperty(totalPosition, label, property);
		}

		public void Dispose()
		{
			EditorGUI.EndProperty();
		}
	}
}
