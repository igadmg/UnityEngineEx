using UnityEngine;



namespace UnityEngineEx
{
	public class BaseState : StateMachineBehaviour
	{
		protected Animator m_Animator;
		protected BaseStateMachine m_StateMachine;
		protected AnimatorStateInfo m_StateInfo;

		public BaseStateMachine stateMachine { get { return m_StateMachine; } }
		public AnimatorStateInfo stateInfo { get { return m_StateInfo; } }
		public float weight { get; protected set; }

		public string stateName { get { return m_StateMachine.GetStateName(m_StateInfo.shortNameHash); } }
	}

	public class BaseState<TStateMachine, TController> : BaseState
		where TStateMachine : BaseStateMachine
	{
		protected TController m_Controller;


		public new TStateMachine stateMachine { get { return m_StateMachine as TStateMachine; } }
		public TController controller { get { return m_Controller; } }


		protected virtual void OnEnter() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnExit() { }



		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			m_Animator = animator;
			m_StateMachine = m_Animator.gameObject.GetComponent<TStateMachine>();
			m_StateInfo = stateInfo;
			m_Controller = m_Animator.gameObject.GetComponent<TController>();

			m_StateMachine.StartTransition(this, m_Animator);
			if (m_StateMachine.onStateSwitch != null)
			{
				m_StateMachine.onStateSwitch(this);
			}

			weight = m_StateMachine.GetStateWeight(this, m_Animator);
			OnEnter();
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			weight = m_StateMachine.GetStateWeight(this, m_Animator);
			OnUpdate();
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			weight = m_StateMachine.GetStateWeight(this, m_Animator);
			OnExit();
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
