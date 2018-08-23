using MathEx;
using System.Linq;
using SystemEx;
using UnityEditor;
using UnityEditorEx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineEx;



namespace UnityEditorEx.Components
{
    [CustomEditor(typeof(PrefabPanel))]
    class Inspector_PrefabPanel : Editor<PrefabPanel>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PrefabPanel panel = target;

            if (GUILayout.Button("Rebuild"))
            {
                RebuildPrefabPanel(panel);
            }
            if (GUILayout.Button("Apply"))
            {
                ApplyPrefabPanel(panel);
            }
        }

        public static void RebuildPrefabPanel(PrefabPanel panel)
        {
            panel.transform.ClearImmediate();

            Vector3 position = Vector3.zero;
            int i = 0;
            foreach (GameObject prefab in panel.prefabs)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                GameObject item = new GameObject("Item{0} ({1})".format(i, go.name));
                if (panel.isUIPrefabs)
                {
                    Canvas canvas = item.AddComponent<Canvas>();
                    CanvasScaler canvasScaler = item.AddComponent<CanvasScaler>();
                    GraphicRaycaster graphicRaycaster = item.AddComponent<GraphicRaycaster>();

                    canvas.renderMode = RenderMode.WorldSpace;
                    RectTransform rectTransform = canvas.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = panel.uiCanvasSize;
                }
				else
				{
					if (go.GetComponentInChildren<Renderer>() == null)
					{
						item.AddComponent<MeshRenderer>();
						TextMesh text = item.AddComponent<TextMesh>();
						text.characterSize = 0.1f;
						text.fontSize = 30;
						text.anchor = TextAnchor.MiddleCenter;
						text.alignment = TextAlignment.Center;
						text.text = go.name;
					}
				}
                go.transform.SetParent(item.transform, false);
                panel.Add(item);

                Bounds itemb = item.GetBounds();
                if (panel.isUIPrefabs)
                {
                    itemb = new Bounds(Vector3.zero, panel.uiCanvasSize.xyz(0));
                }

                float shift = itemb.IsEmpty() ? 0.0f : (Vector3.right.Mul(itemb.size).magnitude / 2.0f);

                if (i > 0)
                {
                    position += Vector3.right * shift;
                    position += Vector3.right;
                }

                item.transform.localPosition = position;

                position += Vector3.right * shift;

                i++;
            }
        }

        public static void ApplyPrefabPanel(PrefabPanel panel)
        {
            EditorProgressBar.ShowProgressBar("Applying prefabs ...", 0);

            int i = 0;
            foreach (GameObject item in panel.gameObject.GetEnumerator())
            {
                GameObject prefab = item.GetEnumerator().First();
                PrefabUtility.ReplacePrefab(prefab, PrefabUtility.GetCorrespondingObjectFromSource(prefab));

                i++;
                EditorProgressBar.ShowProgressBar("Applying prefabs ...", i / (i + 1.0f));
            }

            EditorProgressBar.ShowProgressBar("Applying prefabs ...", 1.0f);
            EditorProgressBar.ClearProgressBar();
        }
    }
}
