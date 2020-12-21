using System;
using UnityEngine;

namespace UnityEngineEx
{
	public class RectangleComponent : MonoBehaviour
	{
		public Rect bounds = new Rect();

#if UNITY_EDITOR
		public event Action<bool> onValidate;
		Rect lastBounds = new Rect();
		public void OnValidate()
		{
			onValidate?.Invoke(lastBounds != bounds);
			lastBounds = bounds;
		}
#endif
	}
}
