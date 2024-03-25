using System;
using SystemEx;
using Random = UnityEngine.Random;

namespace UnityEngineEx {
	public static class RandomEx {
		public static float RandomDiff(this float v) {
			return v + Random.Range(-0.1f * v, 0.1f * v);
		}

		public static IDisposable State(Random.State state, Action<Random.State> updateStateFn) {
			var oldState = Random.state;
			Random.state = state;
			return DisposableLock.Lock(() => {
				updateStateFn(Random.state);
				Random.state = oldState;
			});
		}
	}
}
