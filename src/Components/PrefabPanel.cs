using System.Collections.Generic;
using UnityEngine;



namespace UnityEngineEx
{
    public class PrefabPanel : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> m_Prefabs = new List<GameObject>();

        [SerializeField]
        bool m_isUIPrefabs = false;

        [SerializeField]
        Vector2 m_UICanvasSize = new Vector2(640, 480);



        public List<GameObject> prefabs { get { return m_Prefabs; } }
        public bool isUIPrefabs { get { return m_isUIPrefabs; } }
        public Vector2 uiCanvasSize { get { return m_UICanvasSize; } }
    }
}