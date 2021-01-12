using System;
using UnityEngine;

namespace UnityEngineEx
{
	// Thanks to https://www.patrykgalach.com/2020/01/27/assigning-interface-in-unity-inspector/
	public class RequireInterfaceAttribute : PropertyAttribute
	{
		// Interface type.
		public System.Type requiredType { get; private set; }
		/// <summary>
		/// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
		/// </summary>
		/// <param name="type">Interface type.</param>
		public RequireInterfaceAttribute(Type type)
		{
			requiredType = type;
		}
	}
}
