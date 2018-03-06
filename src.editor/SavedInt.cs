using UnityEditor;

namespace UnityEditorEx
{
	public class SavedInt
	{
		private int m_Value;
		private string m_Name;
		private bool m_Loaded;

		public SavedInt(string name, int value)
		{
			m_Name = name;
			m_Loaded = false;
			m_Value = value;
		}

		private void Load()
		{
			if (m_Loaded)
				return;
			m_Loaded = true;
			m_Value = EditorPrefs.GetInt(this.m_Name, this.m_Value);
		}

		public int value {
			get {
				Load();
				return m_Value;
			}
			set {
				Load();
				if (m_Value == value)
					return;
				m_Value = value;
				EditorPrefs.SetInt(m_Name, value);
			}
		}

		public static implicit operator int(SavedInt s)
		{
			return s.value;
		}
	}
}
