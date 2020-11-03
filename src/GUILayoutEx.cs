using System;
using SystemEx;
using UnityEngine;



namespace UnityEngineEx
{
	public static class GUILayoutEx
	{
		public static IDisposable Area(Rect screenRect)
		{
			GUILayout.BeginArea(screenRect);
			return DisposableLock.Lock(() => GUILayout.EndArea());
		}

		public static IDisposable Horizontal(params GUILayoutOption[] options)
		{
			GUILayout.BeginHorizontal(options);
			return DisposableLock.Lock(() => GUILayout.EndHorizontal());
		}

		public static IDisposable Vertical(params GUILayoutOption[] options)
		{
			GUILayout.BeginVertical(options);
			return DisposableLock.Lock(() => GUILayout.EndVertical());
		}

		public static IDisposable ScrollView(ref Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, options);
			return DisposableLock.Lock(() => GUILayout.EndScrollView());
		}
	}
}
