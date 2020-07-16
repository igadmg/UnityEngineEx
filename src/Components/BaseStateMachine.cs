using System;
using System.Collections.Generic;
using SystemEx;
using UnityEngine;



namespace UnityEngineEx
{
	public abstract class BaseStateMachine : MonoBehaviour
	{
		protected Animator m_Animator;
		protected Dictionary<int, string> m_StateNames = new Dictionary<int, string>();
		protected BaseState m_CurrentState;
		protected BaseState m_LastTransitionState;
		AnimatorTransitionInfo? m_CurrentTransition = null;
		protected float m_TransitionStartTime;

		protected object parameters = null;
		public T GetParameters<T>() => (T)parameters;


		public BaseState currentState { get { return m_CurrentState; } }
		public BaseState lastState { get { return m_LastTransitionState; } }
		public float weight {
			get {
				if (!m_CurrentTransition.HasValue)
				{
					return 1.0f;
				}

				//return (Time.time - m_TransitionStartTime) / m_CurrentTransition.Value.duration;
				return 0.0f;
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



		public Action<BaseState> onStateSwitch = null;



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

		protected void AddStateNames(params string[] names)
		{
			foreach (string name in names)
			{
				m_StateNames.Add(Animator.StringToHash(name), name);
			}
		}



		protected abstract void RegisterStateNames();



		protected virtual void Awake()
		{
			m_Animator = GetComponent<Animator>();
			RegisterStateNames();
		}

#if UNITY_EDITOR
		private string currentStateHashKey { get { return "StateMachines.{0}.currentStateHash".format(gameObject.name); } }
		private string currentStateDataKey { get { return "StateMachines.{0}.currentStateData".format(gameObject.name); } }

		private void OnEnable()
		{
			UnityEditor.AssemblyReloadEvents.afterAssemblyReload += LoadStateInformation;
		}

		private void OnDisable()
		{
			AnimatorStateInfo currentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
			UnityEditor.EditorPrefs.SetInt(currentStateHashKey, currentStateInfo.fullPathHash);

			if (m_CurrentState != null)
			{
				UnityEditor.EditorPrefs.SetString(currentStateDataKey, JsonUtility.ToJson(m_CurrentState));
				Debug.Log(JsonUtility.ToJson(m_CurrentState));
			}
			else
			{
				UnityEditor.EditorPrefs.DeleteKey(currentStateDataKey);
			}
		}

		public bool restoreState = false;
		public string restoreStateData;
		protected void LateUpdate()
		{
			if (restoreState)
			{
				restoreState = false;
				Debug.LogFormat("{0}: Restoring state done.", gameObject.name);
			}
		}

		private void LoadStateInformation()
		{
			if (!UnityEditor.EditorApplication.isPlaying)
				return;

			m_Animator = GetComponent<Animator>();
			m_StateNames.Clear();
			RegisterStateNames();

			int savedStateHash = UnityEditor.EditorPrefs.GetInt(currentStateHashKey);
			if (savedStateHash != 0)
			{
				Debug.LogFormat("{0}: Restoring state {1}", gameObject.name, savedStateHash);
				restoreState = true;
				restoreStateData = UnityEditor.EditorPrefs.GetString(currentStateDataKey);
				m_Animator.Play(savedStateHash);
			}
			UnityEditor.EditorPrefs.DeleteKey(currentStateHashKey);
			UnityEditor.EditorPrefs.DeleteKey(currentStateDataKey);
		}
#endif
	}
}
