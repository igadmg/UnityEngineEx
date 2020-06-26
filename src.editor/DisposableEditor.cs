using System;

namespace UnityEditorEx
{
	public class DisposableEditor<T> : IDisposable
		where T : UnityEngine.Object
	{
		T item;
		UnityEditor.Editor editor;

		public UnityEditor.Editor GetEditor(T item)
		{
			if (item != this.item)
			{
				Dispose();
				editor = UnityEditor.Editor.CreateEditor(item);
				this.item = item;
			}

			return editor;
		}

		public void Dispose()
		{
			UnityEngine.Object.DestroyImmediate(editor);
		}
	}
}
