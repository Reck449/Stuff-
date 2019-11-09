using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TCCL
{
	public class NewProgressBar : UserControl
	{
		private int min;

		private int max = 100;

		private int val;

		private Color BarColor = Color.Blue;

		private IContainer components;

		public int Maximum
		{
			get
			{
				return this.max;
			}
			set
			{
				if (value < this.min)
				{
					this.min = value;
				}
				this.max = value;
				if (this.val > this.max)
				{
					this.val = this.max;
				}
				base.Invalidate();
			}
		}

		public int Minimum
		{
			get
			{
				return this.min;
			}
			set
			{
				if (value < 0)
				{
					this.min = 0;
				}
				if (value > this.max)
				{
					this.min = value;
					this.min = value;
				}
				if (this.val < this.min)
				{
					this.val = this.min;
				}
				base.Invalidate();
			}
		}

		public Color ProgressBarColor
		{
			get
			{
				return this.BarColor;
			}
			set
			{
				this.BarColor = value;
				base.Invalidate();
			}
		}

		public int Value
		{
			get
			{
				return this.val;
			}
			set
			{
				int num = this.val;
				if (value < this.min)
				{
					this.val = this.min;
				}
				else if (value <= this.max)
				{
					this.val = value;
				}
				else
				{
					this.val = this.max;
				}
				Rectangle clientRectangle = base.ClientRectangle;
				Rectangle width = base.ClientRectangle;
				float single = (float)(this.val - this.min) / (float)(this.max - this.min);
				clientRectangle.Width = (int)((float)clientRectangle.Width * single);
				single = (float)(num - this.min) / (float)(this.max - this.min);
				width.Width = (int)((float)width.Width * single);
				Rectangle height = new Rectangle();
				if (clientRectangle.Width <= width.Width)
				{
					height.X = clientRectangle.Size.Width;
					height.Width = width.Width - clientRectangle.Width;
				}
				else
				{
					height.X = width.Size.Width;
					height.Width = clientRectangle.Width - width.Width;
				}
				height.Height = base.Height;
				base.Invalidate(height);
			}
		}

		public NewProgressBar()
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void Draw3DBorder(Graphics g)
		{
			Pen pen = new Pen(Color.FromArgb(20, 10, 10), 4f);
			g.DrawRectangle(pen, base.ClientRectangle);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "NewProgressBar";
			base.Size = new System.Drawing.Size(150, 34);
			base.ResumeLayout(false);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			SolidBrush solidBrush = new SolidBrush(this.BarColor);
			float single = (float)(this.val - this.min) / (float)(this.max - this.min);
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Width = (int)((float)clientRectangle.Width * single);
			graphics.FillRectangle(solidBrush, clientRectangle);
			this.Draw3DBorder(graphics);
			solidBrush.Dispose();
			graphics.Dispose();
		}

		protected override void OnResize(EventArgs e)
		{
			base.Invalidate();
		}
	}
}