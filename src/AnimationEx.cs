using UnityEngine;

namespace UnityEngineEx
{
	public static class AnimationEx
	{
		/// <summary>
		/// Get AnimationState of Animation by index.
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static AnimationState GetState(this Animation animation, int index)
		{
			int i = 0;
			foreach (AnimationState state in animation) {
				if (i == index)
					return state;
				i++;
			}

			return null;
		}

		/// <summary>
		/// Sample one frame from ainmation clip.
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="clip"></param>
		/// <param name="normalizedTime"></param>
		/// <returns></returns>
		public static Animation SampleFrame(this Animation animation, AnimationClip clip, float normalizedTime)
		{
			AnimationState state = animation[clip.name];
			state.enabled = true;
			state.normalizedTime = normalizedTime;
			state.weight = 1.0f;
			animation.Sample();
			state.enabled = false;

			return animation;
		}

		/// <summary>
		/// Play clip in with desired speed.
		/// If same clip is already played than it's speed is adjusted.
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="clip"></param>
		/// <param name="speed"></param>
		/// <returns></returns>
		public static Animation PlayDirection(this Animation animation, AnimationClip clip, float speed)
		{
			AnimationState state = animation[clip.name];

			state.speed = speed;
			if (!state.enabled)
			{
				if (speed > 0)
					state.Forward();
				else
					state.Reverse();

				animation.Play(clip.name, PlayMode.StopSameLayer);
			}

			return animation;
		}
	}
}
