using System.Reflection;
using UnityEditor;

namespace UnityEditorEx
{
	public static class SerializedPropertyEx
	{
		public static object GetValue(this SerializedProperty property)
		{
			object obj = property.serializedObject.targetObject;

			FieldInfo field = null;
			foreach (var path in property.propertyPath.Split('.'))
			{
				var type = obj.GetType();
				field = type.GetField(path);
				obj = field.GetValue(obj);
			}

			return obj;
		}
	}
}
