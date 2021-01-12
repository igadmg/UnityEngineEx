using MathEx;
using SystemEx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityEngineEx
{
	public class UnityRandomGenerator : IRandomGenerator
		, IRandomGenerator<int>
		, IRandomGenerator<float>
		, IRandomGenerator<vec2>
		, IRandomGenerator<Vector2>
		, IRandomGenerator<vec3>
		, IRandomGenerator<Vector3>
	{
		Random.State state;

		public UnityRandomGenerator()
		{
			state = Random.state;
		}

		public IRandomGenerator<T> Cast<T>()
		{
			return (IRandomGenerator<T>)this;
		}

		int IRandomGenerator<int>.Next(int min, int max)
		{
			using (RandomEx.State(state, s => state = s))
			{
				if (min == default && max == default)
				{
					return Random.value.Lerp(int.MinValue, int.MaxValue);
				}

				return Random.value.Lerp(min, max);
			}
		}

		float IRandomGenerator<float>.Next(float min, float max)
		{
			using (RandomEx.State(state, s => state = s))
			{
				if (min == default && max == default)
				{
					return Random.value;
				}

				return Random.value.Lerp(min, max);
			}
		}

		vec2 IRandomGenerator<vec2>.Next(vec2 min, vec2 max)
			=> ((IRandomGenerator<Vector2>)this).Next(min, max);

		Vector2 IRandomGenerator<Vector2>.Next(Vector2 min, Vector2 max)
		{
			using (RandomEx.State(state, s => state = s))
			{
				if (min == default && max == default)
				{
					return new Vector2(Random.value, Random.value);
				}

				return new Vector2(Random.value.Lerp(min.x, max.x), Random.value.Lerp(min.y, max.y));
			}
		}

		vec3 IRandomGenerator<vec3>.Next(vec3 min, vec3 max)
			=> ((IRandomGenerator<Vector3>)this).Next(min, max);

		Vector3 IRandomGenerator<Vector3>.Next(Vector3 min, Vector3 max)
		{
			using (RandomEx.State(state, s => state = s))
			{
				if (min == default && max == default)
				{
					return new Vector3(Random.value, Random.value, Random.value);
				}

				return new Vector3(Random.value.Lerp(min.x, max.x), Random.value.Lerp(min.y, max.y), Random.value.Lerp(min.z, max.z));
			}
		}
	}
}
