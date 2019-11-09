using System;

namespace TCCL
{
	public sealed class FontTypes
	{
		public readonly static string combat_crit;

		public readonly static string combat;

		public readonly static string death;

		public readonly static string item;

		public readonly static string mouse;

		static FontTypes()
		{
			FontTypes.combat_crit = "combat_crit.xnb";
			FontTypes.combat = "Combat_Text.xnb";
			FontTypes.death = "Death_Text.xnb";
			FontTypes.item = "Item_Stack.xnb";
			FontTypes.mouse = "Mouse_Text.xnb";
		}

		private FontTypes()
		{
		}
	}
}