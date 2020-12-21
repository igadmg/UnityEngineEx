using UnityEngine;

namespace UnityEngineEx
{
	public interface IHeightSampler
	{
		Vector3 SampleHeight(Vector2 position);
	}
}
