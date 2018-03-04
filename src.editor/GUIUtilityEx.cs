using UnityEngine;



namespace UnityEditorEx
{
	public static class GUIUtilityEx
	{
        public static Rect GUIToScreenRect(this Rect guiRect)
		{
			Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = screenPoint.x;
			guiRect.y = screenPoint.y;
			return guiRect;
		}
	}
}
