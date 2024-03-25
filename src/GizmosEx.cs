using MathEx;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngineEx {
	public class GizmosColor : IDisposable {
		Color oldColor;

		public GizmosColor(Color color) {
			oldColor = Gizmos.color;
			Gizmos.color = color;
		}

		public void Dispose() {
			Gizmos.color = oldColor;
		}
	}

	public static class GizmosEx {
		public static GizmosColor Color(Color color) {
			return new GizmosColor(color);
		}

		public static void DrawLineTo(Vector3 origin, Vector3 direction, Color color) {
			using (Color(color)) {
				Gizmos.DrawLine(origin, origin + direction);
			}
		}

		public static void DrawMultiLine(Vector3[] points) {
			for (int i = 1; i < points.Length; i++) {
				Gizmos.DrawLine(points[i - 1], points[i]);
			}
		}

		public static void DrawMultiLine(List<Vector3> points) {
			for (int i = 1; i < points.Count; i++) {
				Gizmos.DrawLine(points[i - 1], points[i]);
			}
		}

		public static void DrawSphere(Transform t, Vector3 center, float radius) {
			Gizmos.DrawSphere(t.TransformPoint(center), t.localScale.Min() * radius);
		}
	}
}
