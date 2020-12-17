using UnityEngine;



namespace UnityEngineEx
{
	public interface IState
	{
		void OnRestore();
		void OnEnter(float duration);
		void OnUpdate(float weight);
		void OnExit(float duration);
	}

	public class BaseState : StateMachineBehaviour, IState
	{
		protected Animator m_Animator;
		protected BaseStateMachine m_StateMachine;
		protected AnimatorStateInfo m_StateInfo;

		public BaseStateMachine stateMachine { get { return m_StateMachine; } }
		public AnimatorStateInfo stateInfo { get { return m_StateInfo; } }
		public float weight { get; protected set; }

		public int stateNameHash { get { return m_StateInfo.shortNameHash; } }
		public string stateName { get { return m_StateMachine.GetStateName(m_StateInfo.shortNameHash); } }

		[SerializeField]
		protected GameObject m_ControllerPrefab;
		protected GameObject m_Controller;

		public virtual void OnRestore() { }
		public virtual void OnEnter(float duration) { }
		public virtual void OnUpdate(float weight) { }
		public virtual void OnExit(float duration) { }


		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			m_Animator = animator;
			m_StateMachine = m_Animator.gameObject.GetComponent<BaseStateMachine>();
			m_StateInfo = stateInfo;
			m_Controller = GameObject.Instantiate(m_ControllerPrefab);
			m_Controller.transform.SetParent(m_StateMachine.transform);

#if UNITY_EDITOR
			try
			{
				Debug.LogFormat("{0}: OnStateEnter {1}", m_StateMachine.name, stateName);
			}
			catch
			{
				Debug.LogWarningFormat("{0}: OnStateEnter {1}:{2}", m_StateMachine.name, "<failed>", stateNameHash);
			}
#endif

			m_StateMachine.StartTransition(this, m_Animator);
			weight = m_StateMachine.GetStateWeight(this, m_Animator);

#if UNITY_EDITOR
			if (m_StateMachine.restoreState)
			{
				if (!string.IsNullOrEmpty(m_StateMachine.restoreStateData))
				{
					JsonUtility.FromJsonOverwrite(m_StateMachine.restoreStateData, this);
				}

				OnRestore();
			}
			else
#endif
			{
				m_StateMachine.onStateSwitch?.Invoke(this);
				OnEnter(0);
			}
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			weight = m_StateMachine.GetStateWeight(this, m_Animator);
			OnUpdate(weight);
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
#if UNITY_EDITOR
			if (m_StateMachine == null || m_Animator == null)
			{
				return;
			}
#endif

			weight = m_StateMachine.GetStateWeight(this, m_Animator);
			OnExit(0);

#if UNITY_EDITOR
			try
			{
				Debug.LogFormat("{0}: OnStateExit {1}", m_StateMachine.name, stateName);
			}
			catch
			{
				Debug.LogWarningFormat("{0}: OnStateExit {1}:{2}", m_StateMachine.name, "<failed>", stateNameHash);
			}
#endif
		}

		// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
		//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		//{
		//
		//}

		// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
		//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		//{
		//
		//}
	}
}
