using System;
using SystemEx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditorEx
{
	public static class EditorGUIEx
	{
		public static IDisposable ChangeCheck(Action OnChange, bool ignore = false)
		{
			EditorGUI.BeginChangeCheck();
			return DisposableLock.Lock(() => {
				if (!ignore && EditorGUI.EndChangeCheck())
				{
					OnChange?.Invoke();
				}
			});
		}

		public static void ChangeCheck(Action action, Action OnChange, bool ignore = false)
		{
			using (ChangeCheck(OnChange, ignore))
			{
				action?.Invoke();
			}
		}

		public static T ChangeCheck<T>(Func<T> action, Action<T> OnChange, bool ignore = false)
		{
			T r = default;
			using (ChangeCheck(() => OnChange(r), ignore))
			{
				r = action.Invoke();
			}
			return r;
		}

		public static IDisposable DisabledScopeIf(bool isDiasbaled)
		{
			return new EditorGUI.DisabledScope(isDiasbaled);
		}

		public static IDisposable Property(Rect totalPosition, GUIContent label, SerializedProperty property)
		{
			GUIContent content;
			return Property(totalPosition, label, property, out content);
		}

		public static IDisposable Property(Rect totalPosition, GUIContent label, SerializedProperty property, out GUIContent content)
		{
			content = EditorGUI.BeginProperty(totalPosition, label, property);
			return DisposableLock.Lock(() => {
				EditorGUI.EndProperty();
			});
		}

		public static IDisposable FoldoutHeaderGroup(Rect position, ref bool foldout, string content, [DefaultValue("EditorStyles.foldoutHeader")] GUIStyle style = null, Action<Rect> menuAction = null, GUIStyle menuIcon = null
			, Action ifVisible = null)
		{
			foldout = EditorGUI.BeginFoldoutHeaderGroup(position, foldout, content, style, menuAction, menuIcon);
			return FinishFoldoutHeaderGroup(foldout, ifVisible);
		}

		public static IDisposable FoldoutHeaderGroup(Rect position, ref bool foldout, GUIContent content, [DefaultValue("EditorStyles.foldoutHeader")] GUIStyle style = null, Action<Rect> menuAction = null, GUIStyle menuIcon = null
			, Action ifVisible = null)
		{
			foldout = EditorGUI.BeginFoldoutHeaderGroup(position, foldout, content, style, menuAction, menuIcon);
			return FinishFoldoutHeaderGroup(foldout, ifVisible);
		}

		private static IDisposable FinishFoldoutHeaderGroup(bool foldout, Action ifVisible = null)
		{
			if (foldout)
				try
				{
					ifVisible?.Invoke();
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}

			return new DisposableLock(() => EditorGUI.EndFoldoutHeaderGroup());
		}
	}
}
