using MathEx;
using UnityEngine;

namespace UnityEngineEx
{
	public class TerrainHeightSampler : MonoBehaviour, IHeightSampler
	{
		public Terrain terrain;

		public Vector3 SampleHeight(Vector2 position)
			=> position.xzy(terrain.SampleHeight(position.xzy(0)));


#if UNITY_EDITOR
		public void OnValidate()
		{
			if (terrain == null)
				terrain = GetComponent<Terrain>();
		}
#endif
	}
}
