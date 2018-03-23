namespace UnityEditorEx
{
	public class Editor<T> : UnityEditor.Editor
		where T : UnityEngine.Object
	{
		private static int s_activeEditor = 0;

		public bool isActive { get { return GetInstanceID() == s_activeEditor; } set { s_activeEditor = value ? GetInstanceID() : 0; } }
		new public T target { get { return (T)base.target; } }

		protected virtual void OnEnable()
		{
			if (s_activeEditor == 0)
			{
				s_activeEditor = GetInstanceID();
			}
		}

		protected virtual void OnDisable()
		{
			if (s_activeEditor == GetInstanceID())
			{
				s_activeEditor = 0;
			}
		}
	}

	public class Editor<T, TStyles> : Editor<T>
		where T : UnityEngine.Object
		where TStyles : new()
	{
		protected TStyles m_Styles = default(TStyles);

		public TStyles styles { get { if (m_Styles == null) m_Styles = new TStyles(); return m_Styles; } }
	}
}
