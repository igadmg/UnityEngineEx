using System;
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

		public static IDisposable Horizontal(out Rect rect, params GUILayoutOption[] options)
		{
			rect = EditorGUILayout.BeginHorizontal(options);
			return new DisposableLock(() => EditorGUILayout.EndHorizontal());
		}

		public static IDisposable Vertical(params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginVertical(options);
			return new DisposableLock(() => EditorGUILayout.EndVertical());
		}

		public static IDisposable Vertical(out Rect rect, params GUILayoutOption[] options)
		{
			rect = EditorGUILayout.BeginVertical(options);
			return new DisposableLock(() => EditorGUILayout.EndVertical());
		}

		public static void FoldoutHeaderGroup(ref bool foldout, string content, [DefaultValue("EditorStyles.foldoutHeader")] GUIStyle style = null, Action<Rect> menuAction = null, GUIStyle menuIcon = null
			, Action ifVisible = null)
		{
			foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, content, style, menuAction, menuIcon);
			if (foldout) SafeExecuteAction(ifVisible);
			EditorGUILayout.EndFoldoutHeaderGroup();
		}

		public static void FoldoutHeaderGroup(ref bool foldout, GUIContent content, [DefaultValue("EditorStyles.foldoutHeader")] GUIStyle style = null, Action<Rect> menuAction = null, GUIStyle menuIcon = null
			, Action ifVisible = null)
		{
			foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, content, style, menuAction, menuIcon);
			if (foldout) SafeExecuteAction(ifVisible);
			EditorGUILayout.EndFoldoutHeaderGroup();
		}

		public static void InspectorTitlebar(ref bool foldout, UnityEngine.Object[] targetObjs
			, Action ifVisible = null)
		{
			foldout = EditorGUILayout.InspectorTitlebar(foldout, targetObjs);
			if (foldout) SafeExecuteAction(ifVisible);
		}
		public static void InspectorTitlebar(ref bool foldout, UnityEngine.Object targetObj, bool expandable
			, Action ifVisible = null)
		{
			foldout = EditorGUILayout.InspectorTitlebar(foldout, targetObj, expandable);
			if (foldout) SafeExecuteAction(ifVisible);
		}
		public static void InspectorTitlebar(ref bool foldout, UnityEngine.Object targetObj
			, Action ifVisible = null)
		{
			foldout = EditorGUILayout.InspectorTitlebar(foldout, targetObj);
			if (foldout) SafeExecuteAction(ifVisible);
		}
		public static void InspectorTitlebar(ref bool foldout, UnityEngine.Object[] targetObjs, bool expandable
			, Action ifVisible = null)
		{
			foldout = EditorGUILayout.InspectorTitlebar(foldout, targetObjs, expandable);
			if (foldout) SafeExecuteAction(ifVisible);
		}

		private static void SafeExecuteAction(Action ifVisible = null)
		{
			try
			{
				ifVisible?.Invoke();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
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
