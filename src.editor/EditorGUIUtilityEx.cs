using System;
using UnityEditor;



namespace UnityEditorEx
{
	public static class EditorGUIUtilityEx
	{
		public static IDisposable LabelWidth(float newWidth)
		{
			return new EditorGUIUtilityLabelWidth(newWidth);
		}
	}

	internal class EditorGUIUtilityLabelWidth : IDisposable
	{
		float oldWidth;

		internal EditorGUIUtilityLabelWidth(float newWidth)
		{
			oldWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = newWidth;
		}

		public void Dispose()
		{
			EditorGUIUtility.labelWidth = oldWidth;
		}
	}
}
