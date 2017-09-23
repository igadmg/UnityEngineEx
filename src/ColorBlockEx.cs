using UnityEngine;
using UnityEngine.UI;

namespace UnityEngineEx
{
	public static class ColorBlockEx
	{
		public static ColorBlock NormalColor(this ColorBlock cb, Color normalColor)
		{
			cb.normalColor = normalColor;
			cb.highlightedColor = normalColor * 0.9f;
			return cb;
		}
	}
}
