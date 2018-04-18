using System.Reflection;
using UnityEditor.Animations;
using UnityEngine;



namespace UnityEditorEx
{
	public static class AnimatorControllerEx
	{
		private static MethodInfo m_GetEffectiveAnimatorController = typeof(AnimatorController).GetMethod("GetEffectiveAnimatorController", BindingFlags.Static | BindingFlags.NonPublic);

		public static AnimatorController GetEffectiveAnimatorController(Animator animator)
		{
			return (AnimatorController)m_GetEffectiveAnimatorController.Invoke(null, new object[] { animator });
		}
	}
}
