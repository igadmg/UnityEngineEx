using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class ComputeBufferEx
	{
		public static ComputeBuffer Create<T>(int count)
			=> new ComputeBuffer(count, MarshalEx.SizeOf<T>());

		public static ComputeBuffer Create<T>(T[] data)
			=> new ComputeBuffer(data.Length, MarshalEx.SizeOf<T>());

		public static T[] GetData<T>(this ComputeBuffer buffer)
		{
			var result = new T[buffer.count];
			buffer.GetData(result);
			return result;
		}
	}
}
