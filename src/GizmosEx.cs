using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEngineEx
{
	public class GizmosColor : IDisposable
	{
		Color oldColor;

		public GizmosColor(Color color)
		{
			oldColor = Gizmos.color;
			Gizmos.color = color;
		}

		public void Dispose()
		{
			Gizmos.color = oldColor;
		}
	}

	public static class GizmosEx
	{
		public static GizmosColor Color(Color color)
		{
			return new GizmosColor(color);
		}

		public static void DrawLineTo(Vector3 origin, Vector3 direction, Color color)
		{
			using (Color(color))
			{
				Gizmos.DrawLine(origin, origin + direction);
			}
		}
	}
}
