using UnityEngine;

namespace UnityEngineEx {
	public static class ComputeShaderEx {
		public static (uint x, uint y, uint z) GetThreadGroupSizes(this ComputeShader compute, int kernelIndex = 0) {
			compute.GetKernelThreadGroupSizes(kernelIndex, out uint x, out uint y, out uint z);
			return (x, y, z);
		}

		public static ComputeBuffer CreateBuffer<T>(this ComputeShader shader, string name, int count) {
			var buffer = ComputeBufferEx.Create<T>(count);
			shader.SetBuffer(0, name, buffer);

			return buffer;
		}

		public static ComputeBuffer CreateBufferWithLength<T>(this ComputeShader shader, string name, T[] data) {
			if (data.Length == 0) {
				var buffer = ComputeBufferEx.Create<T>(1);
				shader.SetBuffer(0, name, buffer);
				shader.SetInt(name + "_length", 0);
				return buffer;
			}
			else {
				var buffer = ComputeBufferEx.Create<T>(data.Length);
				buffer.SetData(data);
				shader.SetBuffer(0, name, buffer);
				shader.SetInt(name + "_length", data.Length);

				return buffer;
			}
		}

		public static void Run(this ComputeShader shader, int numIterationsX, int numIterationsY = 1, int numIterationsZ = 1, int kernelIndex = 0) {
			var threadGroupSize = shader.GetThreadGroupSizes(kernelIndex);
			int numGroupsX = Mathf.CeilToInt(numIterationsX / (float)threadGroupSize.x);
			int numGroupsY = Mathf.CeilToInt(numIterationsY / (float)threadGroupSize.y);
			int numGroupsZ = Mathf.CeilToInt(numIterationsZ / (float)threadGroupSize.y);

			shader.Dispatch(kernelIndex, numGroupsX, numGroupsY, numGroupsZ);
		}
	}
}
