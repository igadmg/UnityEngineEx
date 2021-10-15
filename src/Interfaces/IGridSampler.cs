using UnityEngine;

namespace UnityEngineEx
{
	public interface IGridSampler
	{
		Vector3 SampleGrid(Vector2 position);
	}
}
