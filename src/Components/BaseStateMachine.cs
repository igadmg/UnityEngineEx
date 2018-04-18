using System;
using System.Collections.Generic;
using UnityEngine;



namespace UnityEngineEx
{
	public class BaseStateMachine : MonoBehaviour
	{
		protected Dictionary<int, string> m_StateNames = new Dictionary<int, string>();
		protected BaseState m_CurrentState;
		protected BaseState m_LastTransitionState;
		AnimatorTransitionInfo? m_CurrentTransition = null;
		protected float m_TransitionStartTime;



		public BaseState currentState { get { return m_CurrentState; } }
		public BaseState lastState { get { return m_LastTransitionState; } }
		public float weight
		{
			get {
				if (!m_CurrentTransition.HasValue)
				{
					return 1.0f;
				}

				return (Time.time - m_TransitionStartTime) / m_CurrentTransition.Value.duration;
			}
		}

		internal void StartTransition(BaseState targetState, Animator animator)
		{
			m_TransitionStartTime = Time.time;

			if (m_CurrentState == null)
			{
				m_CurrentState = targetState;
				m_CurrentTransition = null;
			}
			else
			{
				if (animator.IsInTransition(0))
				{
					m_CurrentTransition = animator.GetAnimatorTransitionInfo(0);
				}
				else
				{
					m_CurrentTransition = null;
				}

				m_LastTransitionState = m_CurrentState;
				m_CurrentState = targetState;
			}
		}



		public Action<BaseState> onStateSwitch = (state) => { };



		internal float GetStateWeight(BaseState state, Animator animator)
		{
			if (!animator.IsInTransition(0))
			{
				m_CurrentTransition = null;
			}

			float w = weight;
			return state == m_CurrentState ? w : state == m_LastTransitionState ? 1.0f - w : 0.0f;
		}

		internal string GetStateName(int nameHash)
		{
			return m_StateNames[nameHash];
		}

		public void AddStateNames(params string[] names)
		{
			foreach (string name in names)
			{
				m_StateNames.Add(Animator.StringToHash(name), name);
			}
		}
	}
}
