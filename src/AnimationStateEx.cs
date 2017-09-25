using UnityEngine;



namespace UnityEngineEx
{
	public static class AnimationStateEx
	{
		public static AnimationState Forward(this AnimationState animState)
		{
			animState.speed = 1;
			animState.time = 0;
			return animState;
		}

		public static AnimationState Reverse(this AnimationState animState)
		{
			animState.speed = -1;
			animState.time = animState.length;
			return animState;
		}
	}
}
