using MathEx;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngineEx;

namespace UnityEditorEx
{
	[EditorTool("Resize", typeof(RectangleComponent))]
	public class RectangleComponentResizeTool : EditorTool<RectangleComponent>
	{
		public override Texture2D Image => EditorGUIUtilityEx.LoadIconRequired("MoveTool");

		public override bool IsAvailable()
			=> target != null;

		public override void OnToolGUI(EditorWindow window)
		{
			target.bounds = EditorGUIEx.ChangeCheck(() => {
				Rect bounds = target.bounds;
				bounds.min = HandlesEx.DoPositionHandle(target.transform, target.bounds.min.xzy(0)).xz();
				bounds.max = HandlesEx.DoPositionHandle(target.transform, target.bounds.max.xzy(0)).xz();
				return bounds;
			}, bounds => {
				EditorUtility.SetDirty(target);
				target.OnValidate();
			});
		}
	}

	[CustomEditor(typeof(RectangleComponent))]
	public class RectangleComponentEditor : Editor<RectangleComponent>
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.EditorToolbarForTarget(target);

			DrawDefaultInspector();
		}
	}
}
