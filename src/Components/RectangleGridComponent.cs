using MathEx;
using System;
using SystemEx;
using UnityDissolve;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngineEx
{
	public enum GridFillType
	{
		None,
		DimensionsAreGridSize,
		DimensionsAreCellSize
	}

	[RequireComponent(typeof(RectangleComponent))]
	public class RectangleGridComponent : DissolvedMonoBehaviour
	{
		[Component]
		RectangleComponent rectangle;

		public GridFillType fillType = GridFillType.DimensionsAreGridSize;
		public Vector2 dimensions;
		public UnityEvent onBeforeUpdate;
		public UnityEvent<Rect> onCell;

#if UNITY_EDITOR
		public bool drawGizmoz = true;

		Rect lastBounds;
		GridFillType lastFillType = GridFillType.None;
		[Dispose] DisposableEvent<RectangleComponent> rectangleOnValidate;
		public override void OnValidate()
		{
			base.OnValidate();

			if (UnityEditor.EditorApplication.isPlaying)
				return;

			rectangleOnValidate = rectangleOnValidate.Or(() => new DisposableEvent<RectangleComponent>(o => o.onValidate += OnValidateRectangle, o => o.onValidate -= OnValidateRectangle));
			rectangleOnValidate._ = rectangle;

			if (lastFillType != fillType)
			{
				if (lastFillType != GridFillType.None)
				{
					switch (fillType)
					{
						case GridFillType.DimensionsAreGridSize:
							dimensions = rectangle.bounds.size / dimensions;
							lastFillType = fillType;
							break;
						case GridFillType.DimensionsAreCellSize:
							dimensions = rectangle.bounds.size / dimensions;
							lastFillType = fillType;
							break;
					}
				}
				else
				{
					lastFillType = fillType;
				}
			}

			RebuildGrid();
		}

		protected void OnValidateRectangle(bool changed)
		{
			RebuildGrid();
		}

		private void OnDrawGizmosSelected()
		{
			if (!drawGizmoz)
				return;

			RebuildGrid(cell => GizmosEx.DrawSphere(transform, cell.o.xzy(0).ToVector3(), cell.size.Min() / 2));
		}
#endif

		public void RebuildGrid()
			=> RebuildGrid(cell => onCell.Invoke(cell.ToRect()));

		public void RebuildGrid(Action<aabb2> onCellFn)
		{
			onBeforeUpdate?.Invoke();

			switch (fillType)
			{
				case GridFillType.DimensionsAreGridSize:
					RebuildFillGrid(onCellFn);
					break;
				case GridFillType.DimensionsAreCellSize:
					RebuildDimensionsGrid(onCellFn);
					break;
			}
		}

		protected void RebuildFillGrid(Action<aabb2> onCellFn)
		{
			foreach (var cell in Foreach.Cell(dimensions, rectangle.bounds.size / dimensions))
			{
				onCellFn(cell + rectangle.bounds.center);
			}
		}

		protected void RebuildDimensionsGrid(Action<aabb2> onCellFn)
		{
			foreach (var cell in Foreach.Cell(rectangle.bounds.size / dimensions, dimensions))
			{
				onCellFn(cell + rectangle.bounds.center);
			}
		}
	}
}
