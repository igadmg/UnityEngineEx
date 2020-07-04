using SystemEx;
using UnityEditor.EditorTools;
using UnityEngine;

namespace UnityEngineEx
{
	public class EditorTool<T> : EditorTool
	{
		GUIContent m_IconContent;

		public override GUIContent toolbarIcon => m_IconContent;
		public virtual Texture2D Image => null;
		public virtual string Text => GetType().GetAttribute<EditorToolAttribute>()?.displayName;
		public virtual string Tooltip => null;

		protected virtual void OnEnable()
		{
			m_IconContent = new GUIContent() {
				image = Image,
				text = Text,
				tooltip = Tooltip
			};
		}
	}
}
