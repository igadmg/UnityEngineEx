using MathEx;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemEx;
using UniRx;
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

		public Rect bounds => rectangle.bounds;
		public GridFillType fillType = GridFillType.DimensionsAreGridSize;
		public vec2 dimensions;

		public UnityEvent<IEnumerable<aabb2>> onBeforeUpdate;
		public UnityEvent<aabb2> onCell;

		[NonSerialized]
		Subject<IEnumerable<aabb2>> observeBeforeUpdate_ = null;
		Subject<IEnumerable<aabb2>> observeBeforeUpdate => onBeforeUpdate.GetPersistentEventCount() > 0 ? (Subject<IEnumerable<aabb2>>)ObserveBeforeUpdate() : observeBeforeUpdate_;
		public IObservable<IEnumerable<aabb2>> ObserveBeforeUpdate()
		{
			return observeBeforeUpdate_ ??= new Subject<IEnumerable<aabb2>>()
				.Also(s => s.Subscribe(cells => onBeforeUpdate?.Invoke(cells)));
		}

		[NonSerialized]
		Subject<aabb2> observeCell_ = null;
		Subject<aabb2> observeCell => onCell.GetPersistentEventCount() > 0 ? (Subject<aabb2>)ObserveCell() : observeCell_;
		public IObservable<aabb2> ObserveCell()
		{
			return observeCell_ ??= new Subject<aabb2>()
				.Also(s => s.Subscribe(cell => onCell?.Invoke(cell)));
		}



#if UNITY_EDITOR
		public bool drawGizmoz = false;

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

		public vec2i GridSize {
			get {
				switch (fillType)
				{
					case GridFillType.DimensionsAreGridSize:
						return dimensions;
					case GridFillType.DimensionsAreCellSize:
						return rectangle.bounds.size / dimensions;
				}

				return vec2i.empty;
			}
		}

		public vec2 CellSize {
			get {
				switch (fillType)
				{
					case GridFillType.DimensionsAreGridSize:
						return rectangle.bounds.size / dimensions;
					case GridFillType.DimensionsAreCellSize:
						return dimensions;
				}

				return vec2.empty;
			}
		}

		public IEnumerable<aabb2> Cells
		{
			get {
				switch (fillType)
				{
					case GridFillType.DimensionsAreGridSize:
					case GridFillType.DimensionsAreCellSize:
						return Foreach.Cell(GridSize, CellSize)
							.Select(cell => cell + rectangle.bounds.center);
				}

				return Enumerable.Empty<aabb2>();
			}
		}

		public void RebuildGrid()
			=> RebuildGrid(observeCell != null ? cell => observeCell?.OnNext(cell) : (Action<aabb2>)null);

		public void RebuildGrid(Action<aabb2> onCellFn)
		{
			var cells = Cells.ToList();
			observeBeforeUpdate?.OnNext(cells);

			if (onCellFn != null)
			{
				foreach (var cell in cells)
				{
					onCellFn(cell);
				}
			}
		}
	}
}
