using System;
using System.Runtime.CompilerServices;
using SystemEx;
using UnityEngine.Profiling;

namespace UnityEngineEx
{
	public static class ProfilerEx
	{
#if ENABLE_PROFILER
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IDisposable BeginSample(string name)
		{
			Profiler.BeginSample(name);
			return DisposableLock.Lock(() => Profiler.EndSample());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IDisposable BeginSample()
		{
			var method = TypeEx.GetCallingMethod();
			Profiler.BeginSample($"{method.DeclaringType.Name}.{method.Name}");
			return DisposableLock.Lock(() => Profiler.EndSample());
		}
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IDisposable BeginSample(string name)
			=> DisposableLock.empty;
#endif
	}
}
