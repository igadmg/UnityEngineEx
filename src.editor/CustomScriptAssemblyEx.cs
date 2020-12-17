#if !NETSTANDARD
using PeanutButter.DuckTyping.Extensions;
#endif
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace UnityEditorEx
{
	public interface ICustomScriptAssemblyPlatform
	{
		string Name { get; }
		string DisplayName { get; }
		BuildTarget BuildTarget { get; }
	}

	public static class CustomScriptAssemblyEx
	{
		private static Lazy<Type> m_CustomScriptAssemblyType
			= new Lazy<Type>(() => Type.GetType("UnityEditor.Scripting.ScriptCompilation.CustomScriptAssembly, UnityEditor"));
		private static Lazy<PropertyInfo> m_CustomScriptAssemblyTypePlatforms
			= new Lazy<PropertyInfo>(() => m_CustomScriptAssemblyType.Value.GetProperty("Platforms", BindingFlags.Static | BindingFlags.Public));

#if !NETSTANDARD
		public static ICustomScriptAssemblyPlatform[] Platforms
			=> ((IEnumerable)m_CustomScriptAssemblyTypePlatforms.Value.GetValue(null))
			.OfType<object>()
			.Select(o => o.DuckAs<ICustomScriptAssemblyPlatform>()).ToArray();
#endif
	}
}
