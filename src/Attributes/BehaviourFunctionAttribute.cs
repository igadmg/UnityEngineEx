using System;

namespace UnityEngineEx
{
	public class BehaviourFunctionAttribute : Attribute
	{
		public string name;

		public BehaviourFunctionAttribute(string name)
		{
			this.name = name;
		}
	}
}
