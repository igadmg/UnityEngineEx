using System;
using System.Collections.Generic;
using UnityEngine;



namespace UnityEngineEx
{
    public class BaseStateMachine : MonoBehaviour
    {
        protected Dictionary<int, string> m_StateNames = new Dictionary<int, string>();
        protected BaseState m_CurrentState;



        public BaseState currentState { get { return m_CurrentState; } internal set { m_CurrentState = value; } }



        public Action<BaseState> onStateSwitch = (state) => { };



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
