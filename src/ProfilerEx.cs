using System;
using SystemEx;
using UnityEngine.Profiling;

namespace UnityEngineEx
{
	public static class ProfilerEx
	{
#if ENABLE_PROFILER
		public static IDisposable BeginSample(string name)
		{
			Profiler.BeginSample(name);
			return DisposableLock.Lock(() => Profiler.EndSample());
		}
#else
		public static IDisposable BeginSample(string name)
			=> DisposableLock.empty;
#endif
	}
}
