using System;
using System.Collections;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	[ExecuteInEditMode]
	class CoroutineHelperBehaviour : MonoBehaviour {
		static CoroutineHelperBehaviour _instance;
		public static CoroutineHelperBehaviour instance {
			get {
				_instance = _instance.Or(() => {
					var chb = FindAnyObjectByType<CoroutineHelperBehaviour>();
					if (chb == null) {
						chb = new GameObject("Coroutine Helper").AddComponent<CoroutineHelperBehaviour>();
						chb.gameObject.hideFlags = HideFlags.HideAndDontSave;
					}
					return chb;
				});
				return _instance;
			}
		}
	}

	public static class CorotineEx {
		public static void StartCoroutine(IEnumerator coroutine)
		{
			CoroutineHelperBehaviour.instance.StartCoroutine(coroutine);
		}

		public static void RunAtEndOfFrameEditor(Action fn) {
			IEnumerator RunAtEnfOfFrameFn() {
				yield return null;
				fn();
			}
			StartCoroutine(RunAtEnfOfFrameFn());
		}

		public static void RunAtEndOfFrame(this MonoBehaviour mbh, Action fn) {
			IEnumerator RunAtEnfOfFrameFn() {
				yield return new WaitForEndOfFrame();
				fn();
			}
			mbh.StartCoroutine(RunAtEnfOfFrameFn());
		}
	}
}

