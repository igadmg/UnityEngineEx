using System;



namespace UnityEditorEx
{
	public class SpyglassAttribute : Attribute
	{
		public Type inspectedType { get; protected set; }

		public SpyglassAttribute(Type inspectedType)
		{
			this.inspectedType = inspectedType;
		}

		public SpyglassAttribute(Type inspectedType, bool editorForChildClasses)
		{
			this.inspectedType = inspectedType;
		}
	}
}
