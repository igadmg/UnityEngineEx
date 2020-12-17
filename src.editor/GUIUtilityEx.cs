using System;
using System.Reflection;
using UnityEngine;



namespace UnityEditorEx
{
	public static class GUIUtilityEx
	{
		public static bool ShouldRethrowException(Exception exception)
		{
			return GUIUtilityEx.IsExitGUIException(exception);
		}

		public static bool IsExitGUIException(Exception exception)
		{
			while (exception is TargetInvocationException && exception.InnerException != null)
			{
				exception = exception.InnerException;
			}
			return exception is ExitGUIException;
		}

		public static Rect GUIToScreenRect(this Rect guiRect)
		{
			Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = screenPoint.x;
			guiRect.y = screenPoint.y;
			return guiRect;
		}
	}
}
