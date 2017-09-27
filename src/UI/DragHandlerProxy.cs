using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



namespace UnityEngineEx
{
	class DragHandlerProxy : MonoBehaviour, IDragHandler
	{
		[Serializable]
		public class TriggerEvent : UnityEvent<PointerEventData>
		{ }

		[SerializeField]
		protected TriggerEvent m_Delegate;



		public void OnDrag(PointerEventData eventData)
		{
			if (m_Delegate != null)
			{
				m_Delegate.Invoke(eventData);
			}
		}
	}
}