using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

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

		public static IDisposable Horizontal(params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginHorizontal(options);
			return new DisposableLock(() => EditorGUILayout.EndHorizontal());
		}

		public static IDisposable Vertical(params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginVertical(options);
			return new DisposableLock(() => EditorGUILayout.EndVertical());
		}

		public static IDisposable FoldoutHeaderGroup(ref bool foldout, string content, [DefaultValue("EditorStyles.foldoutHeader")] GUIStyle style = null, Action<Rect> menuAction = null, GUIStyle menuIcon = null
			, Action ifVisible = null)
		{
			foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, content, style, menuAction, menuIcon);

			if (foldout)
				try
				{
					ifVisible?.Invoke();
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}

			return new DisposableLock(() => EditorGUILayout.EndFoldoutHeaderGroup());
		}

		public static int Popup(string label, int selectedIndex, IEnumerable<string> displayedOptions, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(label, selectedIndex, displayedOptions.ToArray(), options);
		public static int Popup(int selectedIndex, IEnumerable<string> displayedOptions, GUIStyle style, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(selectedIndex, displayedOptions.ToArray(), style, options);
		public static int Popup(int selectedIndex, IEnumerable<GUIContent> displayedOptions, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(selectedIndex, displayedOptions.ToArray(), options);
		public static int Popup(int selectedIndex, IEnumerable<GUIContent> displayedOptions, GUIStyle style, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(selectedIndex, displayedOptions.ToArray(), style, options);
		public static int Popup(GUIContent label, int selectedIndex, IEnumerable<string> displayedOptions, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(label, selectedIndex, displayedOptions.ToArray(), options);
		public static int Popup(string label, int selectedIndex, IEnumerable<string> displayedOptions, GUIStyle style, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(label, selectedIndex, displayedOptions.ToArray(), style, options);
		public static int Popup(GUIContent label, int selectedIndex, IEnumerable<GUIContent> displayedOptions, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(label, selectedIndex, displayedOptions.ToArray(), options);
		public static int Popup(GUIContent label, int selectedIndex, IEnumerable<GUIContent> displayedOptions, GUIStyle style, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(label, selectedIndex, displayedOptions.ToArray(), style, options);
		public static int Popup(int selectedIndex, IEnumerable<string> displayedOptions, params GUILayoutOption[] options)
			=> EditorGUILayout.Popup(selectedIndex, displayedOptions.ToArray(), options);
	}
}
