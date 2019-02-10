using UnityEngine;

namespace UnityEngineEx
{
	public struct AnimationFrames
	{
		public int columns;
		public int rows;
		public int count;

		public bool isAnimation { get { return columns != 0 && rows != 0 && count > 0; } }
	}

	public static class SpriteEx
	{
		public static Sprite Create(Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
		}

		public static Sprite Create(Texture2D texture, Vector4 border)
		{
			return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
		}

		public static Sprite[] Create(Texture2D texture, AnimationFrames frames, Vector4 border)
		{
			Sprite[] sprites = new Sprite[frames.count];

			int dc = texture.width / frames.columns;
			int dr = texture.height / frames.rows;

			int i = 0;
			for (int ci = 0; ci < frames.columns; ci++)
			{
				for (int ri = 0; ri < frames.rows; ri++)
				{
					Rect r = new Rect(ci * dc, ri * dr, dc, dr);
					sprites[i++] = Sprite.Create(texture, r, new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
				}
			}

			return sprites;
		}
	}
}
