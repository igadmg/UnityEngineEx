using System;
using UnityEngine;



namespace UnityEngineEx
{
    public abstract class AsyncMonoBehaviour : MonoBehaviour, IAsyncResult
    {
        public object AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public abstract bool IsCompleted { get; }
    }
}