using MathEx;
using System;
using UnityEditor;
using UnityEngine;



namespace UnityEditorEx
{
	public static class HandlesEx
	{
		public static Vector3 DoPositionHandle(Transform t, Vector3 position)
			=> t.InverseTransformPoint(Handles.DoPositionHandle(t.TransformPoint(position), t.rotation));

		public static Vector3 DoPositionHandle(Transform t, Vector3 position, Quaternion rotation)
			=> t.InverseTransformPoint(Handles.DoPositionHandle(t.TransformPoint(position), t.rotation * rotation));

		public static Vector3 DoPositionHandle(Vector3 position, Quaternion rotation, Action<Vector3> onChanged)
		{
			using (EditorGUIEx.ChangeCheck(() => onChanged(position)))
			{
				position = Handles.DoPositionHandle(position, rotation);
				return position;
			}
		}

		public static Quaternion DoRotationHandle(Quaternion rotation, Vector3 position, Action<Quaternion> onChanged)
		{
			using (EditorGUIEx.ChangeCheck(() => onChanged(rotation)))
			{
				rotation = Handles.DoRotationHandle(rotation, position);
				return rotation;
			}
		}

		public static IDisposable DrawingScope(Color color)
		{
			return new Handles.DrawingScope(color);
		}

		public static IDisposable DrawingScope(Color color, Matrix4x4 matrix)
		{
			return new Handles.DrawingScope(color, matrix);
		}

		public static IDisposable DrawingScope(Matrix4x4 matrix)
		{
			return new Handles.DrawingScope(matrix);
		}

		public static void DrawLineArrow(Vector3 start, Vector3 end, float size)
		{
			if (start == end)
				return;

			Vector3 direction = (end - start).normalized;
			Handles.DrawLine(start, end);
			Handles.ArrowHandleCap(0, end - size * 1.14f * direction, Quaternion.LookRotation(direction), size, EventType.Repaint);
		}

		public static void DrawCameraFrustum(Camera camera)
		{
			Vector3[] near = new Vector3[4];
			Vector3[] far = new Vector3[4];
			float frustumAspect;
			if (!camera.GetFrustum(near, far, out frustumAspect))
				return;

			for (int index = 0; index < 4; ++index)
			{
				Handles.DrawLine(near[index], near[(index + 1) % 4]);
				Handles.DrawLine(far[index], far[(index + 1) % 4]);
				Handles.DrawLine(near[index], far[index]);
			}
		}
	}
}
