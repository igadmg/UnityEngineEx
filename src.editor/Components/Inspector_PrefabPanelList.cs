using MathEx;
using UnityEditor;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx.Components
{
	[CustomEditor(typeof(PrefabPanelList))]
	class Inspector_PrefabPanelList : Editor<PrefabPanelList>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			PrefabPanelList panelList = target;

			if (GUILayout.Button("Rebuild"))
			{
				Vector3 position = Vector3.zero;
				Vector3 prevPosition = Vector3.zero;
				int i = 0;
				foreach (PrefabPanel panel in panelList.GetComponentsInChildren<PrefabPanel>())
				{
					Inspector_PrefabPanel.RebuildPrefabPanel(panel);

					Bounds panelBounds = panel.gameObject.GetBounds();
					float shift = panelBounds.IsEmpty() ? 0.0f : (Vector3.forward.Mul(panelBounds.size).magnitude / 2.0f);

					if (i > 0)
					{
						position += Vector3.forward * shift;
						position += 5.0f * Vector3.forward;

						Vector3 deltav = (position - prevPosition);
						float delta0 = deltav.magnitude;
						float delta = delta0.Clamp(10.0f, float.MaxValue);
						if (delta0 != delta)
						{
							position = prevPosition + deltav / delta0 * delta;
						}
					}

					panel.transform.localPosition = position;
					prevPosition = position;

					position += Vector3.forward * shift;
					i++;
				}
			}
			if (GUILayout.Button("Apply"))
			{
				foreach (PrefabPanel panel in panelList.GetComponentsInChildren<PrefabPanel>())
				{
					Inspector_PrefabPanel.ApplyPrefabPanel(panel);
				}
			}
		}
	}
}
