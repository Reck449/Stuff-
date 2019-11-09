using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TCCL
{
	public class Splash : Form
	{
		private string statusInfo;

		private int progress;

		private IContainer components;

		private Timer timer1;

		private Label label1;

		private NewProgressBar newProgressBar1;

		private Label label2;

		public int ProgressInfo
		{
			get
			{
				return this.progress;
			}
			set
			{
				this.progress = value;
				this.ChangeProgressValue();
			}
		}

		public string StatusInfo
		{
			get
			{
				return this.statusInfo;
			}
			set
			{
				this.statusInfo = value;
				this.ChangeStatusText();
			}
		}

		public Splash()
		{
			this.InitializeComponent();
			this.label2.Text = "";
		}

		public void ChangeProgressMinMaxValue(int min, int max)
		{
			try
			{
				if (!base.InvokeRequired)
				{
					this.newProgressBar1.Minimum = min;
					this.newProgressBar1.Maximum = max;
				}
				else
				{
					base.Invoke(new MethodInvoker(() => this.ChangeProgressMinMaxValue(min, max)));
				}
			}
			catch (Exception exception)
			{
			}
		}

		public void ChangeProgressValue()
		{
			try
			{
				if (!base.InvokeRequired)
				{
					this.newProgressBar1.Value = this.progress;
				}
				else
				{
					base.Invoke(new MethodInvoker(this.ChangeProgressValue));
				}
			}
			catch (Exception exception)
			{
			}
		}

		public void ChangeStatusText()
		{
			try
			{
				if (!base.InvokeRequired)
				{
					this.label1.Text = this.statusInfo;
				}
				else
				{
					base.Invoke(new MethodInvoker(this.ChangeStatusText));
				}
			}
			catch (Exception exception)
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new Timer(this.components);
			this.label1 = new Label();
			this.label2 = new Label();
			this.newProgressBar1 = new NewProgressBar();
			base.SuspendLayout();
			this.timer1.Enabled = true;
			this.timer1.Interval = 32;
			this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.label1.BackColor = Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label1.ForeColor = Color.White;
			this.label1.Location = new Point(12, 85);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(360, 37);
			this.label1.TabIndex = 1;
			this.label1.Text = "Please wait...";
			this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.label2.BackColor = Color.Transparent;
			this.label2.Location = new Point(295, 13);
			this.label2.Margin = new System.Windows.Forms.Padding(4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "version";
			this.label2.TextAlign = ContentAlignment.MiddleRight;
			this.newProgressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.newProgressBar1.BackColor = Color.FromArgb(100, 80, 90);
			this.newProgressBar1.Location = new Point(12, 125);
			this.newProgressBar1.Maximum = 100;
			this.newProgressBar1.Minimum = 0;
			this.newProgressBar1.Name = "newProgressBar1";
			this.newProgressBar1.ProgressBarColor = Color.FromArgb(148, 62, 86);
			this.newProgressBar1.Size = new System.Drawing.Size(360, 23);
			this.newProgressBar1.TabIndex = 2;
			this.newProgressBar1.Value = 0;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = Color.Magenta;
			this.BackgroundImage = Resource1.tccl;
			this.BackgroundImageLayout = ImageLayout.Center;
			base.ClientSize = new System.Drawing.Size(384, 160);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.newProgressBar1);
			base.Controls.Add(this.label1);
			this.DoubleBuffered = true;
			this.ForeColor = Color.Black;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MinimumSize = new System.Drawing.Size(384, 160);
			base.Name = "Splash";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Splash";
			base.TransparencyKey = Color.Magenta;
			base.ResumeLayout(false);
		}

		private void newProgressBar1_Load(object sender, EventArgs e)
		{
		}
	}
}