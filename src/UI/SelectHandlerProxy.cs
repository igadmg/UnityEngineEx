using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



namespace UnityEngineEx
{
	class SelectHandlerProxy : MonoBehaviour, ISelectHandler
	{
		[Serializable]
		public class TriggerEvent : UnityEvent<BaseEventData>
		{ }

		[SerializeField]
		protected TriggerEvent m_Delegate;



		public void OnSelect(BaseEventData eventData)
		{
			if (m_Delegate != null)
			{
				m_Delegate.Invoke(eventData);
			}
		}
	}
}
