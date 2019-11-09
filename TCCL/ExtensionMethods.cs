using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace TCCL
{
	public static class ExtensionMethods
	{
		public static Texture2D ConvertToPreMultipliedAlpha(this Texture2D texture)
		{
			Color[] color = new Color[texture.Width * texture.Height];
			texture.GetData<Color>(color, 0, (int)color.Length);
			for (int i = 0; i < (int)color.Length; i++)
			{
				color[i] = new Color(new Vector4(color[i].ToVector3() * ((float)color[i].A / 255f), (float)color[i].A / 255f));
			}
			texture.SetData<Color>(color, 0, (int)color.Length);
			return texture;
		}
	}
}