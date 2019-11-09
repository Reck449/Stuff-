using Microsoft.Xna.Framework;
using System;
using TomShane.Neoforce.Controls;

namespace TCCL
{
	internal class AdvModControl : Control
	{
		private Game game;

		private Pack p;

		private TomShane.Neoforce.Controls.Manager manager;

		private Label lblModName;

		private CheckBox chkLoadPngs;

		private CheckBox chkLoadFonts;

		private CheckBox chkLoadSounds;

		private CheckBox chkLoadMusic;

		private Button btnUp;

		private Button btnDown;

		public AdvModControl(TomShane.Neoforce.Controls.Manager manager) : base(manager)
		{
			this.manager = manager;
		}

		public AdvModControl(TomShane.Neoforce.Controls.Manager manager, Game game, Pack p) : base(manager)
		{
			this.p = p;
			this.manager = manager;
			this.game = game;
		}

		private void btn_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
			if (sender == this.btnUp)
			{
				Packs.MoveUp(this.p.sName);
				Helper.updateList = true;
				return;
			}
			if (sender == this.btnDown)
			{
				Packs.MoveDown(this.p.sName);
				Helper.updateList = true;
			}
		}

		private void chk_CheckedChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
			if (sender == this.chkLoadPngs)
			{
				this.p.bLoadPngs = this.chkLoadPngs.Checked;
				Helper.updateList = true;
			}
			if (sender == this.chkLoadFonts)
			{
				this.p.bLoadFonts = this.chkLoadFonts.Checked;
				Helper.updateList = true;
			}
			if (sender == this.chkLoadSounds)
			{
				this.p.bLoadSounds = this.chkLoadSounds.Checked;
				Helper.updateList = true;
			}
			if (sender == this.chkLoadMusic)
			{
				this.p.bLoadMusic = this.chkLoadMusic.Checked;
				Helper.updateList = true;
			}
		}

		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			this.btnUp.Focused = false;
			this.btnDown.Focused = false;
			this.chkLoadPngs.Focused = false;
			this.chkLoadFonts.Focused = false;
			this.chkLoadMusic.Focused = false;
			this.chkLoadSounds.Focused = false;
			base.DrawControl(renderer, rect, gameTime);
		}

		public override void Init()
		{
			base.Init();
		}

		protected override void InitSkin()
		{
			base.InitSkin();
		}

		public void Load()
		{
			this.chkLoadPngs = new CheckBox(this.manager);
			this.chkLoadPngs.SetSize(20, 20);
			this.chkLoadPngs.SetPosition(this.Width - 128, 10);
			this.chkLoadPngs.Text = "";
			this.chkLoadPngs.Checked = this.p.bLoadPngs;
			this.chkLoadPngs.Parent = this;
			this.chkLoadPngs.CheckedChanged += new TomShane.Neoforce.Controls.EventHandler(this.chk_CheckedChanged);
			this.chkLoadPngs.ToolTip.Text = "Load Pngs Y/N";
			this.chkLoadPngs.ToolTip.TextColor = Microsoft.Xna.Framework.Color.White;
			this.chkLoadFonts = new CheckBox(this.manager);
			this.chkLoadFonts.SetSize(20, 20);
			this.chkLoadFonts.SetPosition(this.Width - 108, 10);
			this.chkLoadFonts.Text = "";
			this.chkLoadFonts.Checked = this.p.bLoadFonts;
			this.chkLoadFonts.Parent = this;
			this.chkLoadFonts.CheckedChanged += new TomShane.Neoforce.Controls.EventHandler(this.chk_CheckedChanged);
			this.chkLoadFonts.ToolTip.Text = "Load Fonts Y/N";
			this.chkLoadFonts.ToolTip.TextColor = Microsoft.Xna.Framework.Color.White;
			this.chkLoadSounds = new CheckBox(this.manager);
			this.chkLoadSounds.SetSize(20, 20);
			this.chkLoadSounds.SetPosition(this.Width - 88, 10);
			this.chkLoadSounds.Text = "";
			this.chkLoadSounds.Checked = this.p.bLoadSounds;
			this.chkLoadSounds.Parent = this;
			this.chkLoadSounds.CheckedChanged += new TomShane.Neoforce.Controls.EventHandler(this.chk_CheckedChanged);
			this.chkLoadSounds.ToolTip.Text = "Load Sounds Y/N";
			this.chkLoadSounds.ToolTip.TextColor = Microsoft.Xna.Framework.Color.White;
			this.chkLoadMusic = new CheckBox(this.manager);
			this.chkLoadMusic.SetSize(20, 20);
			this.chkLoadMusic.SetPosition(this.Width - 68, 10);
			this.chkLoadMusic.Text = "";
			this.chkLoadMusic.Checked = this.p.bLoadMusic;
			this.chkLoadMusic.Parent = this;
			this.chkLoadMusic.CheckedChanged += new TomShane.Neoforce.Controls.EventHandler(this.chk_CheckedChanged);
			this.chkLoadMusic.ToolTip.Text = "Load Music Y/N";
			this.chkLoadMusic.ToolTip.TextColor = Microsoft.Xna.Framework.Color.White;
			this.lblModName = new Label(this.manager);
			this.lblModName.SetPosition(8, 0);
			this.lblModName.SetSize(150, 32);
			this.lblModName.Text = string.Concat(this.p.priority.ToString(), " - ", this.p.sName);
			this.lblModName.TextColor = Microsoft.Xna.Framework.Color.White;
			this.lblModName.Parent = this;
			TomShane.Neoforce.Controls.ToolTip toolTip = this.lblModName.ToolTip;
			string[] strArrays = new string[] { "Author: ", this.p.sAuthor, "\nDescription: ", this.p.sDescription, "\nHomepage: ", this.p.sHomepage };
			toolTip.Text = string.Concat(strArrays);
			this.lblModName.ToolTip.TextColor = Microsoft.Xna.Framework.Color.White;
			this.btnUp = new Button(this.manager);
			this.btnUp.SetSize(16, 14);
			this.btnUp.SetPosition(this.Width - 6 - 22, 2);
			this.btnUp.Text = "+";
			this.btnUp.Parent = this;
			this.btnUp.Click += new TomShane.Neoforce.Controls.EventHandler(this.btn_Click);
			this.btnUp.ToolTip.Text = "Higher Priority";
			this.btnUp.ToolTip.TextColor = Microsoft.Xna.Framework.Color.White;
			this.btnDown = new Button(this.manager);
			this.btnDown.SetSize(16, 14);
			this.btnDown.SetPosition(this.Width - 6 - 22, 16);
			this.btnDown.Text = "-";
			this.btnDown.Parent = this;
			this.btnDown.Click += new TomShane.Neoforce.Controls.EventHandler(this.btn_Click);
			this.btnDown.ToolTip.Text = "Lower Priority";
			this.btnDown.ToolTip.TextColor = Microsoft.Xna.Framework.Color.White;
		}
	}
}