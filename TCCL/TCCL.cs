using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nomad.Archive.SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Terraria;
using TomShane.Neoforce.Controls;

namespace TCCL
{
	public class TCCL : Main
	{
		private ContentManager customContent;

		public SpriteBatch batch;

		private bool convert;

		private string version = "";

		private string name = "";

		private bool enableScroll;

		private KeyboardState oldKeyState;

		private Manager manager;

		private TomShane.Neoforce.Controls.Window window;

		private ImageBox lb;

		private TomShane.Neoforce.Controls.CheckBox chkSkipDialog;

		private TomShane.Neoforce.Controls.CheckBox chkSkipSplash;

		private TomShane.Neoforce.Controls.ScrollBar vert;

		private List<int> offset = new List<int>();

		private List<AdvModControl> advModControl = new List<AdvModControl>();

		private Color cursorBorderColor = Color.Black;

		private Texture2D[] cursorTexture2 = new Texture2D[4];

		private byte cursorType;

		private bool cursorBorder;

		private float cursorSize = 1f;

		private int savedValue;

		public TCCL(string arg)
		{
			AssemblyName name = Assembly.GetExecutingAssembly().GetName();
			this.version = name.Version.ToString();
			this.name = name.Name;
			if (!Directory.Exists(string.Concat(Helper.StartUpPath, "\\Custom Content")))
			{
				try
				{
					Directory.CreateDirectory(string.Concat(Helper.StartUpPath, "\\Custom Content"));
					TCCL.Log.Write("Creating Directory \"Custom Centent\"");
				}
				catch (Exception exception)
				{
					TCCL.Log.Write(exception);
				}
			}
			base.Content.RootDirectory = "Content";
			this.customContent = new ContentManager(base.Services)
			{
				RootDirectory = "Custom Content"
			};
			if (File.Exists(Config.config))
			{
				Config.Load();
				this.cursorBorderColor = Config.colCursorBorderColor;
				this.cursorType = Config.iCursorType;
				this.cursorBorder = Config.bCursorBorder;
				this.cursorSize = Config.fCursorScale;
			}
			else if (File.Exists(Settings.config))
			{
				Settings.Read();
				Config.bShowGui = Settings.ShowGui;
				Config.bSkipSplash = Settings.SkipSplash;
				Config.Save();
				try
				{
					File.Delete(Settings.config);
				}
				catch
				{
				}
			}
			if (arg.ToLower().Contains("content") && !arg.ToLower().Contains("custom content"))
			{
				this.convert = true;
			}
			if (arg.ToLower().Contains(".zip") || arg.ToLower().Contains(".rar") || arg.ToLower().Contains(".7z"))
			{
				using (SevenZipFormat sevenZipFormat = new SevenZipFormat(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "7z.dll")))
				{
					IInArchive inArchive = null;
					if (arg.ToLower().Contains(".zip"))
					{
						inArchive = sevenZipFormat.CreateInArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.Zip));
					}
					if (arg.ToLower().Contains(".rar"))
					{
						inArchive = sevenZipFormat.CreateInArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.Rar));
					}
					if (arg.ToLower().Contains(".7z"))
					{
						inArchive = sevenZipFormat.CreateInArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.SevenZip));
					}
					if (inArchive != null)
					{
						try
						{
							try
							{
								using (InStreamWrapper inStreamWrapper = new InStreamWrapper(File.OpenRead(arg)))
								{
									ulong num = (ulong)32768;
									inArchive.Open(inStreamWrapper, ref num, null);
									uint numberOfItems = 0;
									PropVariant propVariant = new PropVariant();
									numberOfItems = inArchive.GetNumberOfItems();
									for (uint i = 0; i < numberOfItems; i++)
									{
										inArchive.GetProperty(i, ItemPropId.kpidPath, ref propVariant);
										FileInfo fileInfo = new FileInfo(arg);
										string str = "";
										str = propVariant.GetObject().ToString();
										try
										{
											if (str.Contains("\\"))
											{
												str = str.Remove(0, str.LastIndexOf("\\"));
											}
											if (str.Contains("\\"))
											{
												str = str.Remove(0, str.LastIndexOf("\\"));
											}
											StringBuilder stringBuilder = new StringBuilder();
											stringBuilder.Append(Helper.StartUpPath);
											stringBuilder.Append("\\Custom Content\\");
											stringBuilder.Append(fileInfo.Name.Replace(fileInfo.Extension, ""));
											if (!Directory.Exists(stringBuilder.ToString()))
											{
												Directory.CreateDirectory(stringBuilder.ToString());
											}
											if (str.Contains(FileTypes.png))
											{
												stringBuilder.Append("\\images\\");
												if (!Directory.Exists(stringBuilder.ToString()))
												{
													Directory.CreateDirectory(stringBuilder.ToString());
												}
												stringBuilder.Append(str);
												uint[] numArray = new uint[] { i };
												inArchive.Extract(numArray, 1, 0, new ArchiveCallback(i, stringBuilder.ToString()));
											}
											else if (str.Contains(FileTypes.wav))
											{
												stringBuilder.Append("\\sounds\\");
												if (!Directory.Exists(stringBuilder.ToString()))
												{
													Directory.CreateDirectory(stringBuilder.ToString());
												}
												stringBuilder.Append(str);
												uint[] numArray1 = new uint[] { i };
												inArchive.Extract(numArray1, 1, 0, new ArchiveCallback(i, stringBuilder.ToString()));
											}
											else if (str.Contains(FileTypes.cs))
											{
												stringBuilder.Append("\\");
												stringBuilder.Append(str);
												uint[] numArray2 = new uint[] { i };
												inArchive.Extract(numArray2, 1, 0, new ArchiveCallback(i, stringBuilder.ToString()));
											}
											else if (str.Contains(FileTypes.txt))
											{
												stringBuilder.Append("\\");
												stringBuilder.Append(str);
												uint[] numArray3 = new uint[] { i };
												inArchive.Extract(numArray3, 1, 0, new ArchiveCallback(i, stringBuilder.ToString()));
											}
											else if (str.Contains(FileTypes.xwb))
											{
												stringBuilder.Append("\\");
												stringBuilder.Append(str);
												uint[] numArray4 = new uint[] { i };
												inArchive.Extract(numArray4, 1, 0, new ArchiveCallback(i, stringBuilder.ToString()));
											}
											else if (str.Contains(FileTypes.xnb) && (str.Contains(FontTypes.combat) || str.Contains(FontTypes.death) || str.Contains(FontTypes.item) || str.Contains(FontTypes.mouse) || str.Contains(FontTypes.combat_crit)))
											{
												stringBuilder.Append("\\fonts\\");
												if (!Directory.Exists(stringBuilder.ToString()))
												{
													Directory.CreateDirectory(stringBuilder.ToString());
												}
												stringBuilder.Append(str);
												uint[] numArray5 = new uint[] { i };
												inArchive.Extract(numArray5, 1, 0, new ArchiveCallback(i, stringBuilder.ToString()));
											}
										}
										catch (Exception exception1)
										{
											TCCL.Log.Write(exception1);
										}
									}
								}
							}
							catch (Exception exception2)
							{
								TCCL.Log.Write(exception2);
							}
						}
						finally
						{
							Marshal.ReleaseComObject(inArchive);
						}
					}
					else
					{
						return;
					}
				}
			}
			this.LoadPacks();
		}

		private void AddTextureToDictionary(ref Texture2D texture, string uniqueName)
		{
			if (this.PngExists(uniqueName))
			{
				Splasher.Progress = Splasher.Progress + 1;
				return;
			}
			try
			{
				if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", string.Concat(uniqueName, ".xnb"))))
				{
					Splasher.Status = string.Concat("Loading: Default\n", uniqueName);
					texture = base.Content.Load<Texture2D>(string.Concat("Images", Path.DirectorySeparatorChar, uniqueName));
					Splasher.Progress = Splasher.Progress + 1;
				}
			}
			catch
			{
			}
		}

		private void btnApply_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
			Packs.Write();
			Helper.Redraw = true;
			Helper.what = 0;
			(sender as TomShane.Neoforce.Controls.Button).Focused = false;
		}

		private void btnCancel_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
			this.window.Hide();
			this.LoadPacks();
			(sender as TomShane.Neoforce.Controls.Button).Focused = false;
		}

		private void btnClose_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
			this.window.Hide();
			(sender as TomShane.Neoforce.Controls.Button).Focused = false;
		}

		private void Button(Vector2 position, string text, Color color, bool booleanButton, ref bool booleanObject)
		{
			Vector2 vector2 = new Vector2();
			Vector2 vector21 = position;
			float single = 1.25f;
			for (int i = 0; i < 5; i++)
			{
				Color black = Color.Black;
				if (i == 4)
				{
					black = color;
				}
				int num = 0;
				int num1 = 0;
				switch (i)
				{
					case 0:
					{
						num = -2;
						break;
					}
					case 1:
					{
						num = 2;
						break;
					}
					case 2:
					{
						num1 = -2;
						break;
					}
					case 3:
					{
						num1 = 2;
						break;
					}
				}
				string str = text;
				vector2 = Main.fontMouseText.MeasureString(str);
				vector2.X *= 0.5f;
				vector2.Y *= 0.5f;
				this.batch.DrawString(Main.fontMouseText, str, new Vector2(vector21.X + (float)num, vector21.Y + (float)num1), black, 0f, vector2, single, SpriteEffects.None, 0f);
			}
			if (Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Main.oldMouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Rectangle rectangle = new Rectangle((int)(vector21.X - vector2.X * single), (int)(vector21.Y - vector2.Y * single), (int)(vector2.X * single * 2f), (int)(vector2.Y * single * 2f));
				Rectangle rectangle1 = new Rectangle(Main.mouseState.X, Main.mouseState.Y, 1, 1);
				if (rectangle.Intersects(rectangle1))
				{
					if (booleanButton)
					{
						booleanObject = !booleanObject;
						Config.bCursorBorder = booleanObject;
					}
					Main.PlaySound(12, -1, -1, 1);
				}
			}
		}

		private void chk_CheckedChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
			if (sender == this.chkSkipDialog)
			{
				Config.bShowGui = !this.chkSkipDialog.Checked;
			}
			if (sender == this.chkSkipSplash)
			{
				Config.bSkipSplash = this.chkSkipSplash.Checked;
			}
		}

		public static void CleanUp()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			TCCL.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
		}

		public void Convert()
		{
			this.batch = new SpriteBatch(base.GraphicsDevice);
			string str = string.Concat(Helper.StartUpPath, "\\Content\\PngImages\\");
			string[] files = Directory.GetFiles(string.Concat(Helper.StartUpPath, "\\Content\\Images"), "*.xnb");
			if (!Directory.Exists(str))
			{
				Directory.CreateDirectory(str);
			}
			string fileNameWithoutExtension = "";
			Splasher.SetMinMaxValues(0, (int)files.Length);
			string[] strArrays = files;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				fileNameWithoutExtension = Path.GetFileNameWithoutExtension(strArrays[i]);
				Splasher.Status = string.Concat("Converting: ", fileNameWithoutExtension);
				if (File.Exists(string.Concat(str, fileNameWithoutExtension, ".png")))
				{
					File.Delete(string.Concat(str, fileNameWithoutExtension, ".png"));
				}
				FileStream fileStream = new FileStream(string.Concat(str, fileNameWithoutExtension, ".png"), FileMode.CreateNew, FileAccess.Write);
				Texture2D texture2D = base.Content.Load<Texture2D>(string.Concat("Images\\", fileNameWithoutExtension.Replace(".xnb", "")));
				texture2D.SaveAsPng(fileStream, texture2D.Width, texture2D.Height);
				fileStream.Flush();
				fileStream.Close();
				Splasher.Progress = Splasher.Progress + 1;
			}
			Splasher.Status = "Images succesfully converted";
		}

		public void CursorBorderColor()
		{
			this.cursorBorderColor.A = 255;
		}

		protected override void Draw(GameTime gameTime)
		{
			if (Helper.isLoading)
			{
				Splasher.Close();
				Form form = (Form)System.Windows.Forms.Control.FromHandle(base.Window.Handle);
				form.Activate();
				form.TopMost = true;
				Helper.isLoading = false;
				form.TopMost = false;
			}
			if (!Main.showSplash && this.window.Visible)
			{
				this.manager.BeginDraw(gameTime);
			}
			base.Draw(gameTime);
			if (Config.bSkipSplash)
			{
				Main.showSplash = false;
			}
			if (Main.gameMenu && !Main.showSplash)
			{
				this.batch.Begin();
				byte r = (byte)((255 + Main.tileColor.R * 2) / 3);
				Color color = new Color((int)r, (int)r, (int)r, 255);
				for (int i = 0; i < 5; i++)
				{
					Color black = Color.Black;
					if (i == 4)
					{
						black = color;
						black.R = (byte)((255 + black.R) / 2);
						black.G = (byte)((255 + black.R) / 2);
						black.B = (byte)((255 + black.R) / 2);
					}
					black.A = (byte)((float)black.A * 0.3f);
					int num = 0;
					int num1 = 0;
					switch (i)
					{
						case 0:
						{
							num = -2;
							break;
						}
						case 1:
						{
							num = 2;
							break;
						}
						case 2:
						{
							num1 = -2;
							break;
						}
						case 3:
						{
							num1 = 2;
							break;
						}
					}
					string str = string.Concat(this.name, "\n ", this.version);
					Vector2 vector2 = Main.fontMouseText.MeasureString(str);
					vector2.X *= 0.5f;
					vector2.Y *= 0.5f;
					this.batch.DrawString(Main.fontMouseText, str, new Vector2(vector2.X + (float)num + 10f, vector2.Y + (float)num1 - 2f), black, 0f, vector2, 1f, SpriteEffects.None, 0f);
				}
				if (Main.menuMode == 25 && Main.menuMode != 111)
				{
					this.DrawExtendedcursorMenu();
				}
				this.batch.End();
			}
			this.DrawCustomCursor();
			if (!Main.showSplash && this.window.Visible)
			{
				this.manager.EndDraw();
			}
		}

		private void DrawColorButton(Vector2 position, byte colorType)
		{
			Vector2 vector2 = new Vector2();
			Vector2 vector21 = new Vector2();
			Vector2 x = new Vector2();
			Vector2 y = new Vector2();
			Vector2 vector22 = position;
			float single = 1.25f;
			for (int i = 0; i < 5; i++)
			{
				Color black = Color.Black;
				if (i == 4)
				{
					black = Color.White;
					black.R = (byte)((255 + black.R) / 2);
					black.G = (byte)((255 + black.R) / 2);
					black.B = (byte)((255 + black.R) / 2);
				}
				int num = 0;
				int num1 = 0;
				switch (i)
				{
					case 0:
					{
						num = -2;
						break;
					}
					case 1:
					{
						num = 2;
						break;
					}
					case 2:
					{
						num1 = -2;
						break;
					}
					case 3:
					{
						num1 = 2;
						break;
					}
				}
				if (colorType < 3)
				{
					string str = "Red";
					switch (colorType)
					{
						case 0:
						{
							str = string.Concat("Red ", this.cursorBorderColor.R);
							break;
						}
						case 1:
						{
							str = string.Concat("Green ", this.cursorBorderColor.G);
							break;
						}
						case 2:
						{
							str = string.Concat("Blue ", this.cursorBorderColor.B);
							break;
						}
					}
					string str1 = str;
					vector2 = Main.fontMouseText.MeasureString(str1);
					vector2.X *= 0.5f;
					vector2.Y *= 0.5f;
					this.batch.DrawString(Main.fontMouseText, str1, new Vector2(vector22.X + (float)num, vector22.Y + (float)num1), black, 0f, vector2, single, SpriteEffects.None, 0f);
					int x1 = (int)vector22.X - 64;
					str1 = "<";
					vector21 = Main.fontMouseText.MeasureString(str1);
					vector21.X *= 0.5f;
					vector21.Y *= 0.5f;
					this.batch.DrawString(Main.fontMouseText, str1, new Vector2((float)(x1 + num), vector22.Y + (float)num1), black, 0f, vector21, single, SpriteEffects.None, 0f);
					if (Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
					{
						Rectangle rectangle = new Rectangle((int)((float)x1 - vector21.X * single), (int)(vector22.Y - vector21.Y * single), (int)(vector21.X * single * 2f), (int)(vector21.Y * single * 2f));
						Rectangle rectangle1 = new Rectangle(Main.mouseState.X, Main.mouseState.Y, 1, 1);
						if (rectangle.Intersects(rectangle1))
						{
							switch (colorType)
							{
								case 0:
								{
									if (this.cursorBorderColor.R <= 0)
									{
										break;
									}
									ref Color r = ref this.cursorBorderColor;
									r.R = (byte)(r.R - 1);
									break;
								}
								case 1:
								{
									if (this.cursorBorderColor.G <= 0)
									{
										break;
									}
									ref Color g = ref this.cursorBorderColor;
									g.G = (byte)(g.G - 1);
									break;
								}
								case 2:
								{
									if (this.cursorBorderColor.B <= 0)
									{
										break;
									}
									ref Color b = ref this.cursorBorderColor;
									b.B = (byte)(b.B - 1);
									break;
								}
							}
							Config.colCursorBorderColor = this.cursorBorderColor;
							Main.PlaySound(12, -1, -1, 1);
						}
					}
					x1 = (int)vector22.X + 64;
					str1 = ">";
					vector21 = Main.fontMouseText.MeasureString(str1);
					vector21.X *= 0.5f;
					vector21.Y *= 0.5f;
					this.batch.DrawString(Main.fontMouseText, str1, new Vector2((float)(x1 + num), vector22.Y + (float)num1), black, 0f, vector21, single, SpriteEffects.None, 0f);
					if (Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
					{
						Rectangle rectangle2 = new Rectangle((int)((float)x1 - vector21.X * single), (int)(vector22.Y - vector21.Y * single), (int)(vector21.X * single * 2f), (int)(vector21.Y * single * 2f));
						Rectangle rectangle3 = new Rectangle(Main.mouseState.X, Main.mouseState.Y, 1, 1);
						if (rectangle2.Intersects(rectangle3))
						{
							switch (colorType)
							{
								case 0:
								{
									if (this.cursorBorderColor.R >= 255)
									{
										break;
									}
									ref Color colorPointer = ref this.cursorBorderColor;
									colorPointer.R = (byte)(colorPointer.R + 1);
									break;
								}
								case 1:
								{
									if (this.cursorBorderColor.G >= 255)
									{
										break;
									}
									ref Color g1 = ref this.cursorBorderColor;
									g1.G = (byte)(g1.G + 1);
									break;
								}
								case 2:
								{
									if (this.cursorBorderColor.B >= 255)
									{
										break;
									}
									ref Color b1 = ref this.cursorBorderColor;
									b1.B = (byte)(b1.B + 1);
									break;
								}
							}
							Config.colCursorBorderColor = this.cursorBorderColor;
							Main.PlaySound(12, -1, -1, 1);
						}
					}
				}
				else if (colorType == 3)
				{
					string str2 = "Cursor Border Color";
					vector2 = Main.fontDeathText.MeasureString(str2);
					vector2.X *= 0.5f;
					vector2.Y *= 0.5f;
					this.batch.DrawString(Main.fontDeathText, str2, new Vector2(vector22.X + (float)num, vector22.Y + (float)num1), black, 0f, vector2, 0.8f, SpriteEffects.None, 0f);
				}
				else if (colorType == 5)
				{
					string str3 = "Cursor";
					vector2 = Main.fontDeathText.MeasureString(str3);
					vector2.X *= 0.5f;
					vector2.Y *= 0.5f;
					this.batch.DrawString(Main.fontDeathText, str3, new Vector2(vector22.X + (float)num, vector22.Y + (float)num1), black, 0f, vector2, 0.8f, SpriteEffects.None, 0f);
				}
				else if (colorType == 4)
				{
					x.X = (float)((int)vector22.X - 64);
					x.Y = vector22.Y;
					string str4 = "<";
					vector21 = Main.fontMouseText.MeasureString(str4);
					vector21.X *= 0.5f;
					vector21.Y *= 0.5f;
					this.batch.DrawString(Main.fontMouseText, str4, new Vector2(x.X + (float)num, x.Y + (float)num1), black, 0f, vector21, single, SpriteEffects.None, 0f);
					y.X = (float)((int)vector22.X + 64);
					y.Y = vector22.Y;
					str4 = ">";
					vector21 = Main.fontMouseText.MeasureString(str4);
					vector21.X *= 0.5f;
					vector21.Y *= 0.5f;
					this.batch.DrawString(Main.fontMouseText, str4, new Vector2(y.X + (float)num, vector22.Y + (float)num1), black, 0f, vector21, single, SpriteEffects.None, 0f);
				}
			}
			if (colorType != 4)
			{
				return;
			}
			Vector2 vector23 = new Vector2()
			{
				X = (float)(this.cursorTexture2[this.cursorType].Width / 2),
				Y = (float)(this.cursorTexture2[this.cursorType].Height / 2)
			};
			this.batch.Draw(this.cursorTexture2[this.cursorType], new Vector2(vector22.X - vector23.X, vector22.Y - vector23.Y), Color.White);
			if (Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Main.oldMouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Rectangle rectangle4 = new Rectangle((int)(x.X - vector21.X * single), (int)(x.Y - vector21.Y * single), (int)(vector21.X * single * 2f), (int)(vector21.Y * single * 2f));
				Rectangle rectangle5 = new Rectangle(Main.mouseState.X, Main.mouseState.Y, 1, 1);
				if (rectangle4.Intersects(rectangle5))
				{
					if (this.cursorType >= 1)
					{
						TCCL tCCL = this;
						tCCL.cursorType = (byte)(tCCL.cursorType - 1);
					}
					else
					{
						this.cursorType = 0;
					}
					Config.iCursorType = this.cursorType;
					Main.PlaySound(12, -1, -1, 1);
				}
			}
			if (Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Main.oldMouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Rectangle rectangle6 = new Rectangle((int)(y.X - vector21.X * single), (int)(y.Y - vector21.Y * single), (int)(vector21.X * single * 2f), (int)(vector21.Y * single * 2f));
				Rectangle rectangle7 = new Rectangle(Main.mouseState.X, Main.mouseState.Y, 1, 1);
				if (rectangle6.Intersects(rectangle7))
				{
					if (this.cursorType < (int)this.cursorTexture2.Length - 1)
					{
						TCCL tCCL1 = this;
						tCCL1.cursorType = (byte)(tCCL1.cursorType + 1);
					}
					else
					{
						this.cursorType = (byte)((int)this.cursorTexture2.Length - 1);
					}
					Config.iCursorType = this.cursorType;
					Main.PlaySound(12, -1, -1, 1);
				}
			}
		}

		private void DrawCustomCursor()
		{
			this.batch.Begin();
			this.CursorBorderColor();
			Vector2 vector2 = new Vector2();
			Rectangle rectangle = new Rectangle(Main.mouseX + (int)Main.screenPosition.X, Main.mouseY + (int)Main.screenPosition.Y, 1, 1);
			if (!this.cursorBorder)
			{
				vector2 = new Vector2();
				this.batch.Draw(this.cursorTexture2[this.cursorType], new Vector2((float)(Main.mouseState.X + 1), (float)(Main.mouseState.Y + 1)), new Rectangle?(new Rectangle(0, 0, this.cursorTexture2[this.cursorType].Width, this.cursorTexture2[this.cursorType].Height)), new Color((int)((float)Main.cursorColor.R * 0.2f), (int)((float)Main.cursorColor.G * 0.2f), (int)((float)Main.cursorColor.B * 0.2f), (int)((float)Main.cursorColor.A * 0.5f)), 0f, vector2, (float)(Main.cursorScale * 1.1f * this.cursorSize), SpriteEffects.None, 0.9f);
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					int num = 0;
					int num1 = 0;
					switch (i)
					{
						case 0:
						{
							num = -2;
							break;
						}
						case 1:
						{
							num = 2;
							break;
						}
						case 2:
						{
							num1 = -2;
							break;
						}
						case 3:
						{
							num1 = 2;
							break;
						}
					}
					vector2 = new Vector2();
					this.batch.Draw(this.cursorTexture2[this.cursorType], new Vector2((float)Main.mouseState.X + (float)num, (float)Main.mouseState.Y + (float)num1), new Rectangle?(new Rectangle(0, 0, this.cursorTexture2[this.cursorType].Width, this.cursorTexture2[this.cursorType].Height)), this.cursorBorderColor, 0f, vector2, Main.cursorScale * this.cursorSize, SpriteEffects.None, 0.9f);
				}
			}
			vector2 = new Vector2();
			this.batch.Draw(this.cursorTexture2[this.cursorType], new Vector2((float)Main.mouseState.X, (float)Main.mouseState.Y), new Rectangle?(new Rectangle(0, 0, this.cursorTexture2[this.cursorType].Width, this.cursorTexture2[this.cursorType].Height)), Main.cursorColor, 0f, vector2, Main.cursorScale * this.cursorSize, SpriteEffects.None, 0.9f);
			this.batch.End();
		}

		private void DrawExtendedcursorMenu()
		{
			Vector2 vector2 = new Vector2()
			{
				X = (float)(Main.screenWidth / 2),
				Y = 170f
			};
			this.DrawColorButton(vector2, 3);
			vector2.Y = 200f;
			this.Button(vector2, string.Concat("Cursor Border ", (this.cursorBorder ? "On" : "Off")), this.cursorBorderColor, true, ref this.cursorBorder);
			vector2.Y = 230f;
			this.DrawColorButton(vector2, 0);
			vector2.Y = 260f;
			this.DrawColorButton(vector2, 1);
			vector2.Y = 290f;
			this.DrawColorButton(vector2, 2);
			vector2.Y = 470f;
			this.DrawColorButton(vector2, 5);
			vector2.Y = 500f;
			this.DrawColorButton(vector2, 4);
		}

		private void gameForm_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!this.enableScroll)
			{
				return;
			}
			float delta = (float)(e.Delta / 120);
			if (delta != 0f)
			{
				TomShane.Neoforce.Controls.ScrollBar value = this.vert;
				value.Value = value.Value - (int)(delta * 32f);
			}
		}

		private string GetContentPath(string modName, string FileName, string Extension, string subdir)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Helper.StartUpPath);
			stringBuilder.Append("\\Custom Content");
			stringBuilder.Append("\\");
			stringBuilder.Append(modName);
			stringBuilder.Append("\\");
			if (subdir != "")
			{
				stringBuilder.Append(subdir);
				stringBuilder.Append("\\");
			}
			stringBuilder.Append(FileName);
			stringBuilder.Append(Extension);
			return stringBuilder.ToString();
		}

		private int GetFileCount(string packName, string subdir)
		{
			int length = 0;
			if (subdir == "Music")
			{
				length = 3;
			}
			else if (subdir != "All")
			{
				try
				{
					length = (int)Directory.GetFiles(Path.Combine(Helper.StartUpPath, "Custom Content", packName, subdir), "*.*").Length;
				}
				catch
				{
					length = 0;
				}
			}
			else
			{
				try
				{
					length = (int)Directory.GetFiles(Path.Combine(Helper.StartUpPath, "Custom Content", packName), "*.*", SearchOption.AllDirectories).Length - 1;
				}
				catch
				{
					length = 0;
				}
			}
			return length;
		}

		private string GetFontsPath(string modName, string Filename)
		{
			return this.GetContentPath(modName, Filename, ".xnb", "fonts");
		}

		private string GetPngPath(string modName, string Filename)
		{
			return this.GetContentPath(modName, Filename, ".png", "images");
		}

		private string GetWavPath(string modName, string Filename)
		{
			return this.GetContentPath(modName, Filename, ".wav", "sounds");
		}

		protected override void Initialize()
		{
			GraphicsDeviceManager service = (GraphicsDeviceManager)base.Services.GetService(typeof(IGraphicsDeviceManager));
			this.manager = new Manager(this)
			{
				SkinDirectory = string.Concat(Helper.StartUpPath, "\\Content\\Skins")
			};
			this.manager.SetSkin("terraria");
			this.manager.UpdateOrder = 0;
			this.manager.Initialize();
			Form form = (Form)System.Windows.Forms.Control.FromHandle(base.Window.Handle);
			form.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gameForm_MouseWheel);
			this.window = new TomShane.Neoforce.Controls.Window(this.manager);
			this.window.Init();
			this.window.Text = this.name;
			this.window.Top = 32;
			this.window.Left = 32;
			this.window.Width = 350;
			this.window.Height = 450;
			this.window.Visible = true;
			this.window.Alpha = 240;
			this.window.DragAlpha = 245;
			this.window.Resizable = false;
			this.window.CanFocus = false;
			this.window.MouseOver += new TomShane.Neoforce.Controls.MouseEventHandler(this.window_MouseOver);
			this.window.ResizeEnd += new TomShane.Neoforce.Controls.EventHandler(this.window_ResizeEnd);
			this.vert = new TomShane.Neoforce.Controls.ScrollBar(this.manager, TomShane.Neoforce.Controls.Orientation.Vertical);
			this.vert.Init();
			this.lb = new ImageBox(this.manager)
			{
				Height = 288,
				Width = this.window.Width - 32
			};
			this.lb.SetPosition(10, 10);
			this.lb.Parent = this.window;
			this.lb.Margins = new Margins(0, 0, 0, 0);
			this.vert.Parent = this.lb;
			this.vert.Top = 0;
			this.vert.Left = this.lb.ClientWidth - this.vert.Width;
			this.vert.Height = this.lb.ClientHeight;
			this.vert.Value = 0;
			this.vert.Anchor = Anchors.Top | Anchors.Right | Anchors.Bottom | Anchors.Vertical;
			this.vert.ValueChanged += new TomShane.Neoforce.Controls.EventHandler(this.ValueChanged);
			TomShane.Neoforce.Controls.Button button = new TomShane.Neoforce.Controls.Button(this.manager);
			button.Init();
			button.Text = "Cancel";
			button.Width = 96;
			button.Height = 32;
			button.Left = 8;
			button.Top = this.window.ClientHeight - button.Height - 8;
			button.Anchor = Anchors.Bottom;
			button.Parent = this.window;
			button.ToolTip.Text = "Cancel all Settings and close the Window";
			button.ToolTip.TextColor = Color.White;
			button.Click += new TomShane.Neoforce.Controls.EventHandler(this.btnCancel_Click);
			TomShane.Neoforce.Controls.Button clientWidth = new TomShane.Neoforce.Controls.Button(this.manager);
			clientWidth.Init();
			clientWidth.Text = "Apply";
			clientWidth.Width = 96;
			clientWidth.Height = 32;
			clientWidth.Left = this.window.ClientWidth / 2 - clientWidth.Width / 2;
			clientWidth.Top = this.window.ClientHeight - clientWidth.Height - 8;
			clientWidth.Anchor = Anchors.Bottom;
			clientWidth.Parent = this.window;
			clientWidth.ToolTip.Text = "Apply the Settings and load the selected Contentpack(s)";
			clientWidth.ToolTip.TextColor = Color.White;
			clientWidth.Click += new TomShane.Neoforce.Controls.EventHandler(this.btnApply_Click);
			TomShane.Neoforce.Controls.Button clientHeight = new TomShane.Neoforce.Controls.Button(this.manager);
			clientHeight.Init();
			clientHeight.Text = "Close";
			clientHeight.Width = 96;
			clientHeight.Height = 32;
			clientHeight.Left = this.window.ClientWidth - clientHeight.Width - 8;
			clientHeight.Top = this.window.ClientHeight - clientHeight.Height - 8;
			clientHeight.Anchor = Anchors.Bottom;
			clientHeight.Parent = this.window;
			clientHeight.ToolTip.Text = "Just close the Window";
			clientHeight.ToolTip.TextColor = Color.White;
			clientHeight.Click += new TomShane.Neoforce.Controls.EventHandler(this.btnClose_Click);
			this.chkSkipDialog = new TomShane.Neoforce.Controls.CheckBox(this.manager);
			this.chkSkipDialog.Init();
			this.chkSkipDialog.Text = "Hide Dialog on next Start";
			this.chkSkipDialog.Width = 200;
			this.chkSkipDialog.Left = 10;
			this.chkSkipDialog.Top = this.lb.Height + this.lb.Top + 24 + 16;
			this.chkSkipDialog.Parent = this.window;
			this.chkSkipDialog.CheckedChanged += new TomShane.Neoforce.Controls.EventHandler(this.chk_CheckedChanged);
			this.chkSkipDialog.Checked = !Config.bShowGui;
			this.chkSkipSplash = new TomShane.Neoforce.Controls.CheckBox(this.manager);
			this.chkSkipSplash.Init();
			this.chkSkipSplash.Text = "Skip Splashscreen";
			this.chkSkipSplash.Width = 200;
			this.chkSkipSplash.Left = 10;
			this.chkSkipSplash.Top = this.lb.Height + this.lb.Top + 16;
			this.chkSkipSplash.Parent = this.window;
			this.chkSkipSplash.CheckedChanged += new TomShane.Neoforce.Controls.EventHandler(this.chk_CheckedChanged);
			this.chkSkipSplash.Checked = Config.bSkipSplash;
			Packs.GetPackArray().Sort();
			int num = 0;
			for (int i = 0; i < Packs.Count; i++)
			{
				AdvModControl advModControl = new AdvModControl(this.manager, this, Packs.GetPack(i));
				advModControl.SetSize(this.lb.Width - this.vert.Width, 32);
				advModControl.SetPosition(0, num);
				advModControl.Load();
				advModControl.Parent = this.lb;
				advModControl.CanFocus = false;
				if (i % 2 != 0)
				{
					advModControl.Color = new Color(74, 105, 120);
				}
				else
				{
					advModControl.Color = new Color(84, 115, 130);
				}
				this.lb.Add(advModControl);
				num += 32;
				this.advModControl.Add(advModControl);
				this.offset.Add(advModControl.Top);
			}
			this.vert.Range = num;
			this.vert.PageSize = this.lb.Height;
			this.manager.DrawOrder = 3;
			this.manager.Add(this.window);
			this.window.Visible = Config.bShowGui;
			base.Initialize();
			if (this.convert)
			{
				this.Convert();
			}
		}

		private void LoadAcc(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 0; i < (int)Main.accBackTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.accBackTexture[i], string.Concat("Acc_Back_", i));
					Main.accBackLoaded[i] = true;
				}
				for (int j = 0; j < (int)Main.accBalloonTexture.Length; j++)
				{
					this.AddTextureToDictionary(ref Main.accBalloonTexture[j], string.Concat("Acc_Balloon_", j));
					Main.accballoonLoaded[j] = true;
				}
				for (int k = 0; k < (int)Main.accFaceTexture.Length; k++)
				{
					this.AddTextureToDictionary(ref Main.accFaceTexture[k], string.Concat("Acc_Face_", k));
					Main.accFaceLoaded[k] = true;
				}
				for (int l = 0; l < (int)Main.accFrontTexture.Length; l++)
				{
					this.AddTextureToDictionary(ref Main.accFrontTexture[l], string.Concat("Acc_Front_", l));
					Main.accFrontLoaded[l] = true;
				}
				for (int m = 0; m < (int)Main.accHandsOffTexture.Length; m++)
				{
					this.AddTextureToDictionary(ref Main.accHandsOffTexture[m], string.Concat("Acc_HandsOff_", m));
					Main.accHandsOffLoaded[m] = true;
				}
				for (int n = 0; n < (int)Main.accHandsOnTexture.Length; n++)
				{
					this.AddTextureToDictionary(ref Main.accHandsOnTexture[n], string.Concat("Acc_HandsOn_", n));
					Main.accHandsOnLoaded[n] = true;
				}
				for (int o = 0; o < (int)Main.accNeckTexture.Length; o++)
				{
					this.AddTextureToDictionary(ref Main.accNeckTexture[o], string.Concat("Acc_Neck_", o));
					Main.accNeckLoaded[o] = true;
				}
				for (int p = 0; p < (int)Main.accShieldTexture.Length; p++)
				{
					this.AddTextureToDictionary(ref Main.accShieldTexture[p], string.Concat("Acc_Shield_", p));
					Main.accShieldLoaded[p] = true;
				}
				for (int q = 0; q < (int)Main.accShoesTexture.Length; q++)
				{
					this.AddTextureToDictionary(ref Main.accShoesTexture[q], string.Concat("Acc_Shoes_", q));
					Main.accShoesLoaded[q] = true;
				}
				for (int r = 0; r < (int)Main.accWaistTexture.Length; r++)
				{
					this.AddTextureToDictionary(ref Main.accWaistTexture[r], string.Concat("Acc_Waist_", r));
					Main.accWaistLoaded[r] = true;
				}
				return;
			}
			for (int s = 0; s < (int)Main.accBackTexture.Length; s++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Back_", s))))
				{
					Main.accBackTexture[s] = this.LoadPng(name, string.Concat("Acc_Back_", s));
				}
				Main.accBackLoaded[s] = true;
			}
			for (int t = 0; t < (int)Main.accBalloonTexture.Length; t++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Balloon_", t))))
				{
					Main.accBalloonTexture[t] = this.LoadPng(name, string.Concat("Acc_Balloon_", t));
				}
				Main.accballoonLoaded[t] = true;
			}
			for (int u = 0; u < (int)Main.accFaceTexture.Length; u++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Face_", u))))
				{
					Main.accFaceTexture[u] = this.LoadPng(name, string.Concat("Acc_Face_", u));
				}
				Main.accFaceLoaded[u] = true;
			}
			for (int v = 0; v < (int)Main.accFrontTexture.Length; v++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Front_", v))))
				{
					Main.accFrontTexture[v] = this.LoadPng(name, string.Concat("Acc_Front_", v));
				}
				Main.accFrontLoaded[v] = true;
			}
			for (int w = 0; w < (int)Main.accHandsOffTexture.Length; w++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_HandsOff_", w))))
				{
					Main.accHandsOffTexture[w] = this.LoadPng(name, string.Concat("Acc_HandsOff_", w));
				}
				Main.accHandsOffLoaded[w] = true;
			}
			for (int x = 0; x < (int)Main.accHandsOnTexture.Length; x++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_HandsOn_", x))))
				{
					Main.accHandsOnTexture[x] = this.LoadPng(name, string.Concat("Acc_HandsOn_", x));
				}
				Main.accHandsOnLoaded[x] = true;
			}
			for (int y = 0; y < (int)Main.accNeckTexture.Length; y++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Neck_", y))))
				{
					Main.accNeckTexture[y] = this.LoadPng(name, string.Concat("Acc_Neck_", y));
				}
				Main.accNeckLoaded[y] = true;
			}
			for (int a = 0; a < (int)Main.accShieldTexture.Length; a++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Shield_", a))))
				{
					Main.accShieldTexture[a] = this.LoadPng(name, string.Concat("Acc_Shield_", a));
				}
				Main.accShieldLoaded[a] = true;
			}
			for (int b = 0; b < (int)Main.accShoesTexture.Length; b++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Shoes_", b))))
				{
					Main.accShoesTexture[b] = this.LoadPng(name, string.Concat("Acc_Shoes_", b));
				}
				Main.accShoesLoaded[b] = true;
			}
			for (int c = 0; c < (int)Main.accWaistTexture.Length; c++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Acc_Waist_", c))))
				{
					Main.accWaistTexture[c] = this.LoadPng(name, string.Concat("Acc_Waist_", c));
				}
				Main.accWaistLoaded[c] = true;
			}
		}

		private void LoadArmor(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 1; i < (int)Main.femaleBodyTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.femaleBodyTexture[i], string.Concat("Female_Body_", i));
					Main.armorBodyLoaded[i] = true;
				}
				for (int j = 1; j < (int)Main.armorBodyTexture.Length; j++)
				{
					this.AddTextureToDictionary(ref Main.armorBodyTexture[j], string.Concat("Armor_Body_", j));
					Main.armorBodyLoaded[j] = true;
				}
				for (int k = 1; k < (int)Main.armorArmTexture.Length; k++)
				{
					this.AddTextureToDictionary(ref Main.armorArmTexture[k], string.Concat("Armor_Arm_", k));
					Main.armorBodyLoaded[k] = true;
				}
				for (int l = 1; l < (int)Main.armorHeadTexture.Length; l++)
				{
					this.AddTextureToDictionary(ref Main.armorHeadTexture[l], string.Concat("Armor_Head_", l));
					Main.armorHeadLoaded[l] = true;
				}
				for (int m = 1; m < (int)Main.armorLegTexture.Length; m++)
				{
					this.AddTextureToDictionary(ref Main.armorLegTexture[m], string.Concat("Armor_Legs_", m));
					Main.armorLegsLoaded[m] = true;
				}
				return;
			}
			for (int n = 1; n < (int)Main.armorBodyTexture.Length; n++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Armor_Body_", n))))
				{
					Main.armorBodyTexture[n] = this.LoadPng(name, string.Concat("Armor_Body_", n));
				}
				if (File.Exists(this.GetPngPath(name, string.Concat("female_Body_", n))))
				{
					Main.femaleBodyTexture[n] = this.LoadPng(name, string.Concat("female_Body_", n));
				}
				if (File.Exists(this.GetPngPath(name, string.Concat("Armor_Arm_", n))))
				{
					Main.armorArmTexture[n] = this.LoadPng(name, string.Concat("Armor_Arm_", n));
				}
				Main.armorBodyLoaded[n] = true;
			}
			for (int o = 1; o < (int)Main.armorHeadTexture.Length; o++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Armor_Head_", o))))
				{
					Main.armorHeadTexture[o] = this.LoadPng(name, string.Concat("Armor_Head_", o));
				}
				Main.armorHeadLoaded[o] = true;
			}
			for (int p = 1; p < (int)Main.armorLegTexture.Length; p++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Armor_Legs_", p))))
				{
					Main.armorLegTexture[p] = this.LoadPng(name, string.Concat("Armor_Legs_", p));
				}
				Main.armorLegsLoaded[p] = true;
			}
		}

		private void LoadBackground(string name = "", bool png = false)
		{
			if (png)
			{
				for (int i = 0; i < (int)Main.backgroundTexture.Length; i++)
				{
					if (File.Exists(this.GetPngPath(name, string.Concat("Background_", i))))
					{
						Main.backgroundTexture[i] = this.LoadPng(name, string.Concat("Background_", i));
						Main.backgroundWidth[i] = Main.backgroundTexture[i].Width;
						Main.backgroundHeight[i] = Main.backgroundTexture[i].Height;
						Main.backgroundLoaded[i] = true;
					}
				}
				return;
			}
			for (int j = 0; j < (int)Main.backgroundTexture.Length; j++)
			{
				this.AddTextureToDictionary(ref Main.backgroundTexture[j], string.Concat("Background_", j));
				try
				{
					Main.backgroundWidth[j] = Main.backgroundTexture[j].Width;
					Main.backgroundHeight[j] = Main.backgroundTexture[j].Height;
				}
				catch
				{
					Main.backgroundWidth[j] = 0;
					Main.backgroundHeight[j] = 0;
				}
				Main.backgroundLoaded[j] = true;
			}
		}

		private void LoadBank()
		{
			if (File.Exists(string.Concat(Helper.StartUpPath, "\\Content\\TerrariaMusic.xgs")) && Main.engine == null)
			{
				Main.engine = new AudioEngine(string.Concat(Helper.StartUpPath, "\\Content\\TerrariaMusic.xgs"));
			}
			if (File.Exists(string.Concat(Helper.StartUpPath, "\\Content\\Sound Bank.xsb")) && Main.soundBank == null)
			{
				Main.soundBank = new SoundBank(Main.engine, string.Concat(Helper.StartUpPath, "\\Content\\Sound Bank.xsb"));
			}
			if (File.Exists(string.Concat(Helper.StartUpPath, "\\Content\\Wave Bank.xwb")))
			{
				if (Main.waveBank != null)
				{
					Main.waveBank.Dispose();
					Main.waveBank = null;
				}
				Main.waveBank = new WaveBank(Main.engine, string.Concat(Helper.StartUpPath, "\\Content\\Wave Bank.xwb"));
			}
			for (int i = 1; i < (int)Main.music.Length; i++)
			{
				if (Main.music[i] != null)
				{
					Main.music[i].Dispose();
					try
					{
						Main.music[i] = Main.soundBank.GetCue(string.Concat("Music_", i));
					}
					catch (Exception exception)
					{
						TCCL.Log.Write(exception.Message);
					}
				}
			}
			Main.engine.Update();
		}

		protected override void LoadContent()
		{
			this.batch = new SpriteBatch(base.GraphicsDevice);
			Splasher.Status = "Loading default Content...";
			base.LoadContent();
			Thread thread = new Thread(new ParameterizedThreadStart(this.Reload));
			thread.Start(0);
			thread.Join();
		}

		private void LoadCustomFonts(bool load, string name)
		{
			if (load)
			{
				Splasher.Status = string.Concat("Loading Fonts: ", name);
				if (File.Exists(this.GetFontsPath(name, "Death_Text")))
				{
					Main.fontDeathText = this.customContent.Load<SpriteFont>(string.Concat(name, "\\Fonts\\Death_Text"));
				}
				Splasher.Progress = Splasher.Progress + 1;
				if (File.Exists(this.GetFontsPath(name, "Item_Stack")))
				{
					Main.fontItemStack = this.customContent.Load<SpriteFont>(string.Concat(name, "\\Fonts\\Item_Stack"));
				}
				Splasher.Progress = Splasher.Progress + 1;
				if (File.Exists(this.GetFontsPath(name, "Mouse_Text")))
				{
					Main.fontMouseText = this.customContent.Load<SpriteFont>(string.Concat(name, "\\Fonts\\Mouse_Text"));
				}
				Splasher.Progress = Splasher.Progress + 1;
				if (File.Exists(this.GetFontsPath(name, "Combat_Text")))
				{
					Main.fontCombatText[0] = this.customContent.Load<SpriteFont>(string.Concat(name, "\\Fonts\\Combat_Text"));
				}
				Splasher.Progress = Splasher.Progress + 1;
				if (File.Exists(this.GetFontsPath(name, "Combat_Crit")))
				{
					Main.fontCombatText[1] = this.customContent.Load<SpriteFont>(string.Concat(name, "\\Fonts\\Combat_Crit"));
				}
				Splasher.Progress = Splasher.Progress + 1;
			}
		}

		private void LoadCustomMusic(bool load, string name)
		{
			if (load)
			{
				Splasher.Status = string.Concat("Loading Music: ", name);
				try
				{
					if (File.Exists(string.Concat(Helper.StartUpPath, "\\Custom Content\\", name, "\\Wave Bank.xwb")))
					{
						if (Main.waveBank != null)
						{
							Main.waveBank.Dispose();
							Main.waveBank = null;
						}
						Main.waveBank = new WaveBank(Main.engine, string.Concat(Helper.StartUpPath, "\\Custom Content\\", name, "\\Wave Bank.xwb"));
					}
					for (int i = 1; i < (int)Main.music.Length; i++)
					{
						if (Main.music[i] != null)
						{
							Main.music[i].Dispose();
							try
							{
								Main.music[i] = Main.soundBank.GetCue(string.Concat("Music_", i));
							}
							catch (Exception exception)
							{
								TCCL.Log.Write(exception.Message);
							}
						}
					}
					Main.engine.Update();
				}
				catch (Exception exception1)
				{
					TCCL.Log.Write(exception1.Message);
				}
			}
		}

		private void LoadCustomPNGs(bool load, string name)
		{
			if (load)
			{
				try
				{
					for (int i = 0; i < (int)this.cursorTexture2.Length; i++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Cursor_", i))))
						{
							this.cursorTexture2[i] = this.LoadPng(name, string.Concat("Cursor_", i));
						}
					}
					if (File.Exists(this.GetPngPath(name, "Frozen")))
					{
						Main.frozenTexture = this.LoadPng(name, "Frozen");
					}
					if (File.Exists(this.GetPngPath(name, "CraftButton")))
					{
						Main.craftButtonTexture = this.LoadPng(name, "CraftButton");
					}
					if (File.Exists(this.GetPngPath(name, "RecUp")))
					{
						Main.craftUpButtonTexture = this.LoadPng(name, "RecUp");
					}
					if (File.Exists(this.GetPngPath(name, "RecDown")))
					{
						Main.craftDownButtonTexture = this.LoadPng(name, "RecDown");
					}
					if (File.Exists(this.GetPngPath(name, "RecLeft")))
					{
						Main.scrollLeftButtonTexture = this.LoadPng(name, "RecLeft");
					}
					if (File.Exists(this.GetPngPath(name, "RecRight")))
					{
						Main.scrollRightButtonTexture = this.LoadPng(name, "RecRight");
					}
					if (File.Exists(this.GetPngPath(name, "PlayerPulley")))
					{
						Main.pulleyTexture = this.LoadPng(name, "PlayerPulley");
					}
					if (File.Exists(this.GetPngPath(name, "Reforge")))
					{
						Main.reforgeTexture = this.LoadPng(name, "Reforge");
					}
					if (File.Exists(this.GetPngPath(name, "Timer")))
					{
						Main.timerTexture = this.LoadPng(name, "Timer");
					}
					if (File.Exists(this.GetPngPath(name, "WallOfFlesh")))
					{
						Main.wofTexture = this.LoadPng(name, "WallOfFlesh");
					}
					if (File.Exists(this.GetPngPath(name, "Wall_Outline")))
					{
						Main.wallOutlineTexture = this.LoadPng(name, "Wall_Outline");
					}
					if (File.Exists(this.GetPngPath(name, "fade-out")))
					{
						Main.fadeTexture = this.LoadPng(name, "fade-out");
					}
					if (File.Exists(this.GetPngPath(name, "Ghost")))
					{
						Main.ghostTexture = this.LoadPng(name, "Ghost");
					}
					if (File.Exists(this.GetPngPath(name, "Evil_Cactus")))
					{
						Main.evilCactusTexture = this.LoadPng(name, "Evil_Cactus");
					}
					if (File.Exists(this.GetPngPath(name, "Good_Cactus")))
					{
						Main.goodCactusTexture = this.LoadPng(name, "Good_Cactus");
					}
					if (File.Exists(this.GetPngPath(name, "Crimson_Cactus")))
					{
						Main.crimsonCactusTexture = this.LoadPng(name, "Crimson_Cactus");
					}
					if (File.Exists(this.GetPngPath(name, "Wraith_Eyes")))
					{
						Main.wraithEyeTexture = this.LoadPng(name, "Wraith_Eyes");
					}
					if (File.Exists(this.GetPngPath(name, "Reaper_Eyes")))
					{
						Main.reaperEyeTexture = this.LoadPng(name, "Reaper_Eyes");
					}
					for (int j = 0; j < (int)Main.rainTexture.Length; j++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Rain_", j))))
						{
							Main.rainTexture[j] = this.LoadPng(name, string.Concat("Rain_", j));
						}
					}
					if (File.Exists(this.GetPngPath(name, "MagicPixel")))
					{
						Main.magicPixel = this.LoadPng(name, "MagicPixel");
					}
					if (File.Exists(this.GetPngPath(name, "MiniMapFrame")))
					{
						Main.miniMapFrameTexture = this.LoadPng(name, "MiniMapFrame");
					}
					if (File.Exists(this.GetPngPath(name, "MiniMapFrame2")))
					{
						Main.miniMapFrame2Texture = this.LoadPng(name, "MiniMapFrame2");
					}
					for (int k = 0; k < (int)Main.FlameTexture.Length; k++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Flame_", k))))
						{
							Main.FlameTexture[k] = this.LoadPng(name, string.Concat("Flame_", k));
						}
					}
					for (int l = 0; l < (int)Main.miniMapButtonTexture.Length; l++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("MiniMapButton_", l))))
						{
							Main.miniMapButtonTexture[l] = this.LoadPng(name, string.Concat("MiniMapButton_", l));
						}
					}
					for (int m = 0; m < (int)Main.destTexture.Length; m++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Dest", m))))
						{
							Main.destTexture[m] = this.LoadPng(name, string.Concat("Dest", m));
						}
					}
					if (File.Exists(this.GetPngPath(name, "Actuator")))
					{
						Main.actuatorTexture = this.LoadPng(name, "Actuator");
					}
					if (File.Exists(this.GetPngPath(name, "Wires")))
					{
						Main.wireTexture = this.LoadPng(name, "Wires");
					}
					if (File.Exists(this.GetPngPath(name, "Wires2")))
					{
						Main.wire2Texture = this.LoadPng(name, "Wires2");
					}
					if (File.Exists(this.GetPngPath(name, "Wires3")))
					{
						Main.wire3Texture = this.LoadPng(name, "Wires3");
					}
					if (File.Exists(this.GetPngPath(name, "FlyingCarpet")))
					{
						Main.flyingCarpetTexture = this.LoadPng(name, "FlyingCarpet");
					}
					if (File.Exists(this.GetPngPath(name, "HealthBar1")))
					{
						Main.hbTexture1 = this.LoadPng(name, "HealthBar1");
					}
					if (File.Exists(this.GetPngPath(name, "HealthBar2")))
					{
						Main.hbTexture2 = this.LoadPng(name, "HealthBar2");
					}
					int num = 1;
					if (Main.rand != null)
					{
						num = Main.rand.Next(1, 9);
					}
					if (File.Exists(this.GetPngPath(name, string.Concat("logo_", num))))
					{
						Main.loTexture = this.LoadPng(name, string.Concat("logo_", num));
					}
					for (int n = 1; n < 2; n++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("House_Banner_", n))))
						{
							Main.bannerTexture[n] = this.LoadPng(name, string.Concat("House_Banner_", n));
						}
					}
					for (int o = 0; o < (int)Main.npcHeadTexture.Length; o++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("NPC_Head_", o))))
						{
							Main.npcHeadTexture[o] = this.LoadPng(name, string.Concat("NPC_Head_", o));
						}
					}
					for (int p = 1; p < (int)Main.BackPackTexture.Length; p++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("BackPack_", p))))
						{
							Main.BackPackTexture[p] = this.LoadPng(name, string.Concat("BackPack_", p));
						}
					}
					for (int q = 1; q < (int)Main.buffTexture.Length; q++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Buff_", q))))
						{
							Main.buffTexture[q] = this.LoadPng(name, string.Concat("Buff_", q));
						}
					}
					for (int r = 0; r < (int)Main.itemTexture.Length; r++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Item_", r))))
						{
							Main.itemTexture[r] = this.LoadPng(name, string.Concat("Item_", r));
						}
					}
					for (int s = 0; s < (int)Main.gemTexture.Length; s++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Gem_", s))))
						{
							Main.gemTexture[s] = this.LoadPng(name, string.Concat("Gem_", s));
						}
					}
					for (int t = 0; t < (int)Main.cloudTexture.Length; t++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Cloud_", t))))
						{
							Main.cloudTexture[t] = this.LoadPng(name, string.Concat("Cloud_", t));
						}
					}
					for (int u = 0; u < (int)Main.starTexture.Length; u++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Star_", u))))
						{
							Main.starTexture[u] = this.LoadPng(name, string.Concat("Star_", u));
						}
					}
					for (int v = 0; v < (int)Main.liquidTexture.Length; v++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Liquid_", v))))
						{
							Main.liquidTexture[v] = this.LoadPng(name, string.Concat("Liquid_", v));
						}
					}
					for (int w = 0; w < (int)this.waterfallManager.waterfallTexture.Length; w++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Waterfall_", w))))
						{
							this.waterfallManager.waterfallTexture[w] = this.LoadPng(name, string.Concat("Waterfall_", w));
						}
					}
					for (int x = 0; x < (int)Main.npcToggleTexture.Length; x++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("House_", x))))
						{
							Main.npcToggleTexture[x] = this.LoadPng(name, string.Concat("House_", x));
						}
					}
					for (int y = 0; y < (int)Main.HBLockTexture.Length; y++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Lock_", y))))
						{
							Main.HBLockTexture[y] = this.LoadPng(name, string.Concat("Lock_", y));
						}
					}
					if (File.Exists(this.GetPngPath(name, "Grid")))
					{
						Main.gridTexture = this.LoadPng(name, "Grid");
					}
					if (File.Exists(this.GetPngPath(name, "Trash")))
					{
						Main.trashTexture = this.LoadPng(name, "Trash");
					}
					if (File.Exists(this.GetPngPath(name, "CoolDown")))
					{
						Main.cdTexture = this.LoadPng(name, "CoolDown");
					}
					if (File.Exists(this.GetPngPath(name, "Logo")))
					{
						Main.logoTexture = this.LoadPng(name, "Logo");
					}
					if (File.Exists(this.GetPngPath(name, "Logo2")))
					{
						Main.logo2Texture = this.LoadPng(name, "Logo2");
					}
					if (File.Exists(this.GetPngPath(name, "Dust")))
					{
						Main.dustTexture = this.LoadPng(name, "Dust");
					}
					if (File.Exists(this.GetPngPath(name, "Sun")))
					{
						Main.sunTexture = this.LoadPng(name, "Sun");
					}
					if (File.Exists(this.GetPngPath(name, "Sun2")))
					{
						Main.sun2Texture = this.LoadPng(name, "Sun2");
					}
					if (File.Exists(this.GetPngPath(name, "Sun3")))
					{
						Main.sun3Texture = this.LoadPng(name, "Sun3");
					}
					if (File.Exists(this.GetPngPath(name, "Black_Tile")))
					{
						Main.blackTileTexture = this.LoadPng(name, "Black_Tile");
					}
					if (File.Exists(this.GetPngPath(name, "Heart")))
					{
						Main.heartTexture = this.LoadPng(name, "Heart");
					}
					if (File.Exists(this.GetPngPath(name, "Heart2")))
					{
						Main.heart2Texture = this.LoadPng(name, "Heart2");
					}
					if (File.Exists(this.GetPngPath(name, "Bubble")))
					{
						Main.bubbleTexture = this.LoadPng(name, "Bubble");
					}
					if (File.Exists(this.GetPngPath(name, "Flame")))
					{
						Main.flameTexture = this.LoadPng(name, "Flame");
					}
					if (File.Exists(this.GetPngPath(name, "Mana")))
					{
						Main.manaTexture = this.LoadPng(name, "Mana");
					}
					if (File.Exists(this.GetPngPath(name, "Cursor")))
					{
						Main.cursorTexture = this.LoadPng(name, "Cursor");
					}
					if (File.Exists(this.GetPngPath(name, "Cursor2")))
					{
						Main.cursor2Texture = this.LoadPng(name, "Cursor2");
					}
					if (File.Exists(this.GetPngPath(name, "Ninja")))
					{
						Main.ninjaTexture = this.LoadPng(name, "Ninja");
					}
					if (File.Exists(this.GetPngPath(name, "AntlionBody")))
					{
						Main.antLionTexture = this.LoadPng(name, "AntlionBody");
					}
					if (File.Exists(this.GetPngPath(name, "Spike_Base")))
					{
						Main.spikeBaseTexture = this.LoadPng(name, "Spike_Base");
					}
					for (int a = 0; a < (int)Main.woodTexture.Length; a++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Tiles_5_", a))))
						{
							Main.woodTexture[a] = this.LoadPng(name, string.Concat("Tiles_5_", a));
						}
					}
					for (int b = 0; b < (int)Main.moonTexture.Length; b++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Moon_", b))))
						{
							Main.moonTexture[b] = this.LoadPng(name, string.Concat("Moon_", b));
						}
					}
					for (int c = 0; c < (int)Main.treeTopTexture.Length; c++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Tree_Tops_", c))))
						{
							Main.treeTopTexture[c] = this.LoadPng(name, string.Concat("Tree_Tops_", c));
						}
					}
					for (int d = 0; d < (int)Main.treeTopTexture.Length; d++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("Tree_Branches_", d))))
						{
							Main.treeBranchTexture[d] = this.LoadPng(name, string.Concat("Tree_Branches_", d));
						}
					}
					if (File.Exists(this.GetPngPath(name, "Shroom_Tops")))
					{
						Main.shroomCapTexture = this.LoadPng(name, "Shroom_Tops");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back")))
					{
						Main.inventoryBackTexture = this.LoadPng(name, "Inventory_Back");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back2")))
					{
						Main.inventoryBack2Texture = this.LoadPng(name, "Inventory_Back2");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back3")))
					{
						Main.inventoryBack3Texture = this.LoadPng(name, "Inventory_Back3");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back4")))
					{
						Main.inventoryBack4Texture = this.LoadPng(name, "Inventory_Back4");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back5")))
					{
						Main.inventoryBack5Texture = this.LoadPng(name, "Inventory_Back5");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back6")))
					{
						Main.inventoryBack6Texture = this.LoadPng(name, "Inventory_Back6");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back7")))
					{
						Main.inventoryBack7Texture = this.LoadPng(name, "Inventory_Back7");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back8")))
					{
						Main.inventoryBack8Texture = this.LoadPng(name, "Inventory_Back8");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back9")))
					{
						Main.inventoryBack9Texture = this.LoadPng(name, "Inventory_Back9");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back10")))
					{
						Main.inventoryBack10Texture = this.LoadPng(name, "Inventory_Back10");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back11")))
					{
						Main.inventoryBack11Texture = this.LoadPng(name, "Inventory_Back11");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back12")))
					{
						Main.inventoryBack12Texture = this.LoadPng(name, "Inventory_Back12");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back13")))
					{
						Main.inventoryBack13Texture = this.LoadPng(name, "Inventory_Back13");
					}
					if (File.Exists(this.GetPngPath(name, "Inventory_Back14")))
					{
						Main.inventoryBack14Texture = this.LoadPng(name, "Inventory_Back14");
					}
					if (File.Exists(this.GetPngPath(name, "Text_Back")))
					{
						Main.textBackTexture = this.LoadPng(name, "Text_Back");
					}
					if (File.Exists(this.GetPngPath(name, "Chat")))
					{
						Main.chatTexture = this.LoadPng(name, "Chat");
					}
					if (File.Exists(this.GetPngPath(name, "Chat2")))
					{
						Main.chat2Texture = this.LoadPng(name, "Chat2");
					}
					if (File.Exists(this.GetPngPath(name, "Chat_Back")))
					{
						Main.chatBackTexture = this.LoadPng(name, "Chat_Back");
					}
					if (File.Exists(this.GetPngPath(name, "Team")))
					{
						Main.teamTexture = this.LoadPng(name, "Team");
					}
					if (File.Exists(this.GetPngPath(name, "Skin_Arm")))
					{
						Main.skinArmTexture = this.LoadPng(name, "Skin_Arm");
					}
					if (File.Exists(this.GetPngPath(name, "Skin_Body")))
					{
						Main.skinBodyTexture = this.LoadPng(name, "Skin_Body");
					}
					if (File.Exists(this.GetPngPath(name, "Skin_Legs")))
					{
						Main.skinLegsTexture = this.LoadPng(name, "Skin_Legs");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Eye_Whites")))
					{
						Main.playerEyeWhitesTexture = this.LoadPng(name, "Player_Eye_Whites");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Eyes")))
					{
						Main.playerEyesTexture = this.LoadPng(name, "Player_Eyes");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Hands")))
					{
						Main.playerHandsTexture = this.LoadPng(name, "Player_Hands");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Hands2")))
					{
						Main.playerHands2Texture = this.LoadPng(name, "Player_Hands2");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Head")))
					{
						Main.playerHeadTexture = this.LoadPng(name, "Player_Head");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Pants")))
					{
						Main.playerPantsTexture = this.LoadPng(name, "Player_Pants");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Shirt")))
					{
						Main.playerShirtTexture = this.LoadPng(name, "Player_Shirt");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Shoes")))
					{
						Main.playerShoesTexture = this.LoadPng(name, "Player_Shoes");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Undershirt")))
					{
						Main.playerUnderShirtTexture = this.LoadPng(name, "Player_Undershirt");
					}
					if (File.Exists(this.GetPngPath(name, "Player_Undershirt2")))
					{
						Main.playerUnderShirt2Texture = this.LoadPng(name, "Player_Undershirt2");
					}
					if (File.Exists(this.GetPngPath(name, "Female_Pants")))
					{
						Main.femalePantsTexture = this.LoadPng(name, "Female_Pants");
					}
					if (File.Exists(this.GetPngPath(name, "Female_Shirt")))
					{
						Main.femaleShirtTexture = this.LoadPng(name, "Female_Shirt");
					}
					if (File.Exists(this.GetPngPath(name, "Female_Shoes")))
					{
						Main.femaleShoesTexture = this.LoadPng(name, "Female_Shoes");
					}
					if (File.Exists(this.GetPngPath(name, "Female_Undershirt")))
					{
						Main.femaleUnderShirtTexture = this.LoadPng(name, "Female_Undershirt");
					}
					if (File.Exists(this.GetPngPath(name, "Female_Undershirt2")))
					{
						Main.femaleUnderShirt2Texture = this.LoadPng(name, "Female_Undershirt2");
					}
					if (File.Exists(this.GetPngPath(name, "Female_Shirt2")))
					{
						Main.femaleShirt2Texture = this.LoadPng(name, "Female_Shirt2");
					}
					if (File.Exists(this.GetPngPath(name, "Chaos")))
					{
						Main.chaosTexture = this.LoadPng(name, "Chaos");
					}
					if (File.Exists(this.GetPngPath(name, "Eye_Laser")))
					{
						Main.EyeLaserTexture = this.LoadPng(name, "Eye_Laser");
					}
					if (File.Exists(this.GetPngPath(name, "Bone_eyes")))
					{
						Main.BoneEyesTexture = this.LoadPng(name, "Bone_eyes");
					}
					if (File.Exists(this.GetPngPath(name, "Bone_Laser")))
					{
						Main.BoneLaserTexture = this.LoadPng(name, "Bone_Laser");
					}
					if (File.Exists(this.GetPngPath(name, "Light_Disc")))
					{
						Main.lightDiscTexture = this.LoadPng(name, "Light_Disc");
					}
					if (File.Exists(this.GetPngPath(name, "Confuse")))
					{
						Main.confuseTexture = this.LoadPng(name, "Confuse");
					}
					if (File.Exists(this.GetPngPath(name, "Probe")))
					{
						Main.probeTexture = this.LoadPng(name, "Probe");
					}
					if (File.Exists(this.GetPngPath(name, "SunOrb")))
					{
						Main.sunOrbTexture = this.LoadPng(name, "SunOrb");
					}
					if (File.Exists(this.GetPngPath(name, "SunAltar")))
					{
						Main.sunAltarTexture = this.LoadPng(name, "SunAltar");
					}
					if (File.Exists(this.GetPngPath(name, "Chain")))
					{
						Main.chainTexture = this.LoadPng(name, "Chain");
					}
					if (File.Exists(this.GetPngPath(name, "Chain2")))
					{
						Main.chain2Texture = this.LoadPng(name, "Chain2");
					}
					if (File.Exists(this.GetPngPath(name, "Chain3")))
					{
						Main.chain3Texture = this.LoadPng(name, "Chain3");
					}
					if (File.Exists(this.GetPngPath(name, "Chain4")))
					{
						Main.chain4Texture = this.LoadPng(name, "Chain4");
					}
					if (File.Exists(this.GetPngPath(name, "Chain5")))
					{
						Main.chain5Texture = this.LoadPng(name, "Chain5");
					}
					if (File.Exists(this.GetPngPath(name, "Chain6")))
					{
						Main.chain6Texture = this.LoadPng(name, "Chain6");
					}
					if (File.Exists(this.GetPngPath(name, "Chain7")))
					{
						Main.chain7Texture = this.LoadPng(name, "Chain7");
					}
					if (File.Exists(this.GetPngPath(name, "Chain8")))
					{
						Main.chain8Texture = this.LoadPng(name, "Chain8");
					}
					if (File.Exists(this.GetPngPath(name, "Chain9")))
					{
						Main.chain9Texture = this.LoadPng(name, "Chain9");
					}
					if (File.Exists(this.GetPngPath(name, "Chain10")))
					{
						Main.chain10Texture = this.LoadPng(name, "Chain10");
					}
					if (File.Exists(this.GetPngPath(name, "Chain11")))
					{
						Main.chain11Texture = this.LoadPng(name, "Chain11");
					}
					if (File.Exists(this.GetPngPath(name, "Chain12")))
					{
						Main.chain12Texture = this.LoadPng(name, "Chain12");
					}
					if (File.Exists(this.GetPngPath(name, "Chain13")))
					{
						Main.chain13Texture = this.LoadPng(name, "Chain13");
					}
					if (File.Exists(this.GetPngPath(name, "Chain14")))
					{
						Main.chain14Texture = this.LoadPng(name, "Chain14");
					}
					if (File.Exists(this.GetPngPath(name, "Chain15")))
					{
						Main.chain15Texture = this.LoadPng(name, "Chain15");
					}
					if (File.Exists(this.GetPngPath(name, "Chain16")))
					{
						Main.chain16Texture = this.LoadPng(name, "Chain16");
					}
					if (File.Exists(this.GetPngPath(name, "Chain17")))
					{
						Main.chain17Texture = this.LoadPng(name, "Chain17");
					}
					if (File.Exists(this.GetPngPath(name, "Chain18")))
					{
						Main.chain18Texture = this.LoadPng(name, "Chain18");
					}
					if (File.Exists(this.GetPngPath(name, "Chain19")))
					{
						Main.chain19Texture = this.LoadPng(name, "Chain19");
					}
					if (File.Exists(this.GetPngPath(name, "Chain20")))
					{
						Main.chain20Texture = this.LoadPng(name, "Chain20");
					}
					if (File.Exists(this.GetPngPath(name, "Chain21")))
					{
						Main.chain21Texture = this.LoadPng(name, "Chain21");
					}
					if (File.Exists(this.GetPngPath(name, "Chain22")))
					{
						Main.chain22Texture = this.LoadPng(name, "Chain22");
					}
					if (File.Exists(this.GetPngPath(name, "Chain23")))
					{
						Main.chain23Texture = this.LoadPng(name, "Chain23");
					}
					if (File.Exists(this.GetPngPath(name, "Chain24")))
					{
						Main.chain24Texture = this.LoadPng(name, "Chain24");
					}
					if (File.Exists(this.GetPngPath(name, "Chain25")))
					{
						Main.chain25Texture = this.LoadPng(name, "Chain25");
					}
					if (File.Exists(this.GetPngPath(name, "Chain26")))
					{
						Main.chain26Texture = this.LoadPng(name, "Chain26");
					}
					if (File.Exists(this.GetPngPath(name, "Chain27")))
					{
						Main.chain27Texture = this.LoadPng(name, "Chain27");
					}
					if (File.Exists(this.GetPngPath(name, "Chain28")))
					{
						Main.chain28Texture = this.LoadPng(name, "Chain28");
					}
					if (File.Exists(this.GetPngPath(name, "Chain29")))
					{
						Main.chain29Texture = this.LoadPng(name, "Chain29");
					}
					if (File.Exists(this.GetPngPath(name, "Chain30")))
					{
						Main.chain30Texture = this.LoadPng(name, "Chain30");
					}
					if (File.Exists(this.GetPngPath(name, "Chain31")))
					{
						Main.chain31Texture = this.LoadPng(name, "Chain31");
					}
					if (File.Exists(this.GetPngPath(name, "Chain32")))
					{
						Main.chain32Texture = this.LoadPng(name, "Chain32");
					}
					if (File.Exists(this.GetPngPath(name, "Chain33")))
					{
						Main.chain33Texture = this.LoadPng(name, "Chain33");
					}
					if (File.Exists(this.GetPngPath(name, "Chain34")))
					{
						Main.chain34Texture = this.LoadPng(name, "Chain34");
					}
					if (File.Exists(this.GetPngPath(name, "Chain35")))
					{
						Main.chain35Texture = this.LoadPng(name, "Chain35");
					}
					if (File.Exists(this.GetPngPath(name, "Chain36")))
					{
						Main.chain36Texture = this.LoadPng(name, "Chain36");
					}
					if (File.Exists(this.GetPngPath(name, "Chain37")))
					{
						Main.chain37Texture = this.LoadPng(name, "Chain37");
					}
					if (File.Exists(this.GetPngPath(name, "Arm_Bone")))
					{
						Main.boneArmTexture = this.LoadPng(name, "Arm_Bone");
					}
					if (File.Exists(this.GetPngPath(name, "Arm_Bone_2")))
					{
						Main.boneArm2Texture = this.LoadPng(name, "Arm_Bone_2");
					}
					for (int e = 1; e < (int)Main.gemChainTexture.Length; e++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("GemChain_", e))))
						{
							Main.gemChainTexture[e] = this.LoadPng(name, string.Concat("GemChain_", e));
						}
					}
					for (int f = 1; f < (int)Main.golemTexture.Length; f++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("GolemLights", f))))
						{
							Main.golemTexture[f] = this.LoadPng(name, string.Concat("GolemLights", f));
						}
					}
					this.LoadArmor(name, true);
					this.LoadBackground(name, true);
					this.LoadGore(name, true);
					this.LoadHair(name, true);
					this.LoadItemFlames(name, true);
					this.LoadNPC(name, true);
					this.LoadProjectile(name, true);
					this.LoadTiles(name, true);
					this.LoadWalls(name, true);
					this.LoadWings(name, true);
					this.LoadAcc(name, true);
					if (File.Exists(this.GetPngPath(name, "SmartDig")))
					{
						Main.smartDigTexture = this.LoadPng(name, "SmartDig");
					}
					if (File.Exists(this.GetPngPath(name, "JackHat")))
					{
						Main.jackHatTexture = this.LoadPng(name, "JackHat");
					}
					if (File.Exists(this.GetPngPath(name, "TreeFace")))
					{
						Main.treeFaceTexture = this.LoadPng(name, "TreeFace");
					}
					if (File.Exists(this.GetPngPath(name, "PumpkingCloak")))
					{
						Main.pumpkingCloakTexture = this.LoadPng(name, "PumpkingCloak");
					}
					if (File.Exists(this.GetPngPath(name, "PumpkingArm")))
					{
						Main.pumpkingArmTexture = this.LoadPng(name, "PumpkingArm");
					}
					if (File.Exists(this.GetPngPath(name, "PumpkingFace")))
					{
						Main.pumpkingFaceTexture = this.LoadPng(name, "PumpkingFace");
					}
					if (File.Exists(this.GetPngPath(name, "Moon_Pumpkin")))
					{
						Main.pumpkinMoonTexture = this.LoadPng(name, "Moon_Pumpkin");
					}
					if (File.Exists(this.GetPngPath(name, "Moon_Snow")))
					{
						Main.snowMoonTexture = this.LoadPng(name, "Moon_Snow");
					}
					if (File.Exists(this.GetPngPath(name, "SantaTank")))
					{
						Main.santaTankTexture = this.LoadPng(name, "SantaTank");
					}
					if (File.Exists(this.GetPngPath(name, "XmasLight")))
					{
						Main.xmasLightTexture = this.LoadPng(name, "XmasLight");
					}
					if (File.Exists(this.GetPngPath(name, "IceQueen")))
					{
						Main.iceQueenTexture = this.LoadPng(name, "IceQueen");
					}
					if (File.Exists(this.GetPngPath(name, "GlowSnail")))
					{
						Main.glowSnailTexture = this.LoadPng(name, "GlowSnail");
					}
					if (File.Exists(this.GetPngPath(name, "DukeFishron")))
					{
						Main.dukeFishronTexture = this.LoadPng(name, "DukeFishron");
					}
					if (File.Exists(this.GetPngPath(name, "miniMinotaur")))
					{
						Main.miniMinotaurTexture = this.LoadPng(name, "miniMinotaur");
					}
					if (File.Exists(this.GetPngPath(name, "Firefly")))
					{
						Main.fireflyTexture = this.LoadPng(name, "Firefly");
					}
					if (File.Exists(this.GetPngPath(name, "FireflyJar")))
					{
						Main.fireflyJarTexture = this.LoadPng(name, "FireflyJar");
					}
					if (File.Exists(this.GetPngPath(name, "LightningBug")))
					{
						Main.lightningbugTexture = this.LoadPng(name, "LightningBug");
					}
					if (File.Exists(this.GetPngPath(name, "LightningBugJar")))
					{
						Main.lightningbugJarTexture = this.LoadPng(name, "LightningBugJar");
					}
					for (int g = 0; g < (int)Main.jellyfishBowlTexture.Length; g++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("jellyfishBowl", g + 1))))
						{
							Main.jellyfishBowlTexture[g] = this.LoadPng(name, string.Concat("jellyfishBowl", g + 1));
						}
					}
					for (int h = 0; h < (int)Main.xmasTree.Length; h++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("xmas_", h))))
						{
							Main.xmasTree[h] = this.LoadPng(name, string.Concat("xmas_", h));
						}
					}
					for (int i1 = 0; i1 < (int)Main.rudolphMountTexture.Length; i1++)
					{
						if (File.Exists(this.GetPngPath(name, string.Concat("rudolph_", i1))))
						{
							Main.rudolphMountTexture[i1] = this.LoadPng(name, string.Concat("rudolph_", i1));
						}
					}
					if (File.Exists(this.GetPngPath(name, "Mount_Bunny")))
					{
						Main.bunnyMountTexture = this.LoadPng(name, "Mount_Bunny");
					}
					if (File.Exists(this.GetPngPath(name, "Mount_Minecart")))
					{
						Main.minecartMountTexture = this.LoadPng(name, "Mount_Minecart");
					}
					if (File.Exists(this.GetPngPath(name, "Mount_Pigron")))
					{
						Main.pigronMountTexture = this.LoadPng(name, "Mount_Pigron");
					}
					if (File.Exists(this.GetPngPath(name, "Mount_Turtle")))
					{
						Main.turtleMountTexture = this.LoadPng(name, "Mount_Turtle");
					}
					if (File.Exists(this.GetPngPath(name, "Mount_Slime")))
					{
						Main.slimeMountTexture = this.LoadPng(name, "Mount_Slime");
					}
					if (File.Exists(this.GetPngPath(name, "Mount_Bee")))
					{
						Main.beeMountTexture[0] = this.LoadPng(name, "Mount_Bee");
					}
					if (File.Exists(this.GetPngPath(name, "Mount_BeeWings")))
					{
						Main.beeMountTexture[1] = this.LoadPng(name, "Mount_BeeWings");
					}
				}
				catch (Exception exception)
				{
					TCCL.Log.Write(exception.Message);
				}
			}
		}

		private void LoadCustomSounds(bool load, string name)
		{
			if (load)
			{
				if (File.Exists(this.GetWavPath(name, "Grab")))
				{
					Main.soundGrab = this.LoadWav(name, "Grab");
					Main.soundInstanceGrab = Main.soundGrab.CreateInstance();
				}
				if (File.Exists(this.GetWavPath(name, "Mech_0")))
				{
					Main.soundMech[0] = this.LoadWav(name, "Mech_0");
					Main.soundInstanceMech[0] = Main.soundMech[0].CreateInstance();
				}
				if (File.Exists(this.GetWavPath(name, "Pixie")))
				{
					Main.soundPixie = this.LoadWav(name, "Pixie");
					Main.soundInstancePixie = Main.soundPixie.CreateInstance();
				}
				if (File.Exists(this.GetWavPath(name, "Dig_0")))
				{
					try
					{
						Main.soundDig[0] = this.LoadWav(name, "Dig_0");
						Main.soundInstanceDig[0] = Main.soundDig[0].CreateInstance();
					}
					catch (Exception exception)
					{
						TCCL.Log.Write(exception);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Dig_1")))
				{
					try
					{
						Main.soundDig[1] = this.LoadWav(name, "Dig_1");
						Main.soundInstanceDig[1] = Main.soundDig[1].CreateInstance();
					}
					catch (Exception exception1)
					{
						TCCL.Log.Write(exception1);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Dig_2")))
				{
					try
					{
						Main.soundDig[2] = this.LoadWav(name, "Dig_2");
						Main.soundInstanceDig[2] = Main.soundDig[2].CreateInstance();
					}
					catch (Exception exception2)
					{
						TCCL.Log.Write(exception2);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Tink_0")))
				{
					try
					{
						Main.soundTink[0] = this.LoadWav(name, "Tink_0");
						Main.soundInstanceTink[0] = Main.soundTink[0].CreateInstance();
					}
					catch (Exception exception3)
					{
						TCCL.Log.Write(exception3);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Tink_1")))
				{
					try
					{
						Main.soundTink[1] = this.LoadWav(name, "Tink_1");
						Main.soundInstanceTink[1] = Main.soundTink[1].CreateInstance();
					}
					catch (Exception exception4)
					{
						TCCL.Log.Write(exception4);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Tink_2")))
				{
					try
					{
						Main.soundTink[2] = this.LoadWav(name, "Tink_2");
						Main.soundInstanceTink[2] = Main.soundTink[2].CreateInstance();
					}
					catch (Exception exception5)
					{
						TCCL.Log.Write(exception5);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Player_Hit_0")))
				{
					try
					{
						Main.soundPlayerHit[0] = this.LoadWav(name, "Player_Hit_0");
						Main.soundInstancePlayerHit[0] = Main.soundPlayerHit[0].CreateInstance();
					}
					catch (Exception exception6)
					{
						TCCL.Log.Write(exception6);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Player_Hit_1")))
				{
					try
					{
						Main.soundPlayerHit[1] = this.LoadWav(name, "Player_Hit_1");
						Main.soundInstancePlayerHit[1] = Main.soundPlayerHit[1].CreateInstance();
					}
					catch (Exception exception7)
					{
						TCCL.Log.Write(exception7);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Player_Hit_2")))
				{
					try
					{
						Main.soundPlayerHit[2] = this.LoadWav(name, "Player_Hit_2");
						Main.soundInstancePlayerHit[2] = Main.soundPlayerHit[2].CreateInstance();
					}
					catch (Exception exception8)
					{
						TCCL.Log.Write(exception8);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Female_Hit_0")))
				{
					try
					{
						Main.soundFemaleHit[0] = this.LoadWav(name, "Female_Hit_0");
						Main.soundInstanceFemaleHit[0] = Main.soundFemaleHit[0].CreateInstance();
					}
					catch (Exception exception9)
					{
						TCCL.Log.Write(exception9);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Female_Hit_1")))
				{
					try
					{
						Main.soundFemaleHit[1] = this.LoadWav(name, "Female_Hit_1");
						Main.soundInstanceFemaleHit[1] = Main.soundFemaleHit[1].CreateInstance();
					}
					catch (Exception exception10)
					{
						TCCL.Log.Write(exception10);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Female_Hit_2")))
				{
					try
					{
						Main.soundFemaleHit[2] = this.LoadWav(name, "Female_Hit_2");
						Main.soundInstanceFemaleHit[2] = Main.soundFemaleHit[2].CreateInstance();
					}
					catch (Exception exception11)
					{
						TCCL.Log.Write(exception11);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Player_Killed")))
				{
					try
					{
						Main.soundPlayerKilled = this.LoadWav(name, "Player_Killed");
						Main.soundInstancePlayerKilled = Main.soundPlayerKilled.CreateInstance();
					}
					catch (Exception exception12)
					{
						TCCL.Log.Write(exception12);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Chat")))
				{
					try
					{
						Main.soundChat = this.LoadWav(name, "Chat");
						Main.soundInstanceChat = Main.soundChat.CreateInstance();
					}
					catch (Exception exception13)
					{
						TCCL.Log.Write(exception13);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Grass")))
				{
					try
					{
						Main.soundGrass = this.LoadWav(name, "Grass");
						Main.soundInstanceGrass = Main.soundGrass.CreateInstance();
					}
					catch (Exception exception14)
					{
						TCCL.Log.Write(exception14);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Door_Opened")))
				{
					try
					{
						Main.soundDoorOpen = this.LoadWav(name, "Door_Opened");
						Main.soundInstanceDoorOpen = Main.soundDoorOpen.CreateInstance();
					}
					catch (Exception exception15)
					{
						TCCL.Log.Write(exception15);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Door_Closed")))
				{
					try
					{
						Main.soundDoorClosed = this.LoadWav(name, "Door_Closed");
						Main.soundInstanceDoorClosed = Main.soundDoorClosed.CreateInstance();
					}
					catch (Exception exception16)
					{
						TCCL.Log.Write(exception16);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Menu_Tick")))
				{
					try
					{
						Main.soundMenuTick = this.LoadWav(name, "Menu_Tick");
						Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
					}
					catch (Exception exception17)
					{
						TCCL.Log.Write(exception17);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Menu_Open")))
				{
					try
					{
						Main.soundMenuOpen = this.LoadWav(name, "Menu_Open");
						Main.soundInstanceMenuOpen = Main.soundMenuOpen.CreateInstance();
					}
					catch (Exception exception18)
					{
						TCCL.Log.Write(exception18);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Menu_Close")))
				{
					try
					{
						Main.soundMenuClose = this.LoadWav(name, "Menu_Close");
						Main.soundInstanceMenuClose = Main.soundMenuClose.CreateInstance();
					}
					catch (Exception exception19)
					{
						TCCL.Log.Write(exception19);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Shatter")))
				{
					try
					{
						Main.soundShatter = this.LoadWav(name, "Shatter");
						Main.soundInstanceShatter = Main.soundShatter.CreateInstance();
					}
					catch (Exception exception20)
					{
						TCCL.Log.Write(exception20);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Zombie_0")))
				{
					try
					{
						Main.soundZombie[0] = this.LoadWav(name, "Zombie_0");
						Main.soundInstanceZombie[0] = Main.soundZombie[0].CreateInstance();
					}
					catch (Exception exception21)
					{
						TCCL.Log.Write(exception21);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Zombie_1")))
				{
					try
					{
						Main.soundZombie[1] = this.LoadWav(name, "Zombie_1");
						Main.soundInstanceZombie[1] = Main.soundZombie[1].CreateInstance();
					}
					catch (Exception exception22)
					{
						TCCL.Log.Write(exception22);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Zombie_2")))
				{
					try
					{
						Main.soundZombie[2] = this.LoadWav(name, "Zombie_2");
						Main.soundInstanceZombie[2] = Main.soundZombie[2].CreateInstance();
					}
					catch (Exception exception23)
					{
						TCCL.Log.Write(exception23);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Zombie_3")))
				{
					try
					{
						Main.soundZombie[3] = this.LoadWav(name, "Zombie_3");
						Main.soundInstanceZombie[3] = Main.soundZombie[3].CreateInstance();
					}
					catch (Exception exception24)
					{
						TCCL.Log.Write(exception24);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Zombie_4")))
				{
					try
					{
						Main.soundZombie[4] = this.LoadWav(name, "Zombie_4");
						Main.soundInstanceZombie[4] = Main.soundZombie[4].CreateInstance();
					}
					catch (Exception exception25)
					{
						TCCL.Log.Write(exception25);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Roar_0")))
				{
					try
					{
						Main.soundRoar[0] = this.LoadWav(name, "Roar_0");
						Main.soundInstanceRoar[0] = Main.soundRoar[0].CreateInstance();
					}
					catch (Exception exception26)
					{
						TCCL.Log.Write(exception26);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Roar_1")))
				{
					try
					{
						Main.soundRoar[1] = this.LoadWav(name, "Roar_1");
						Main.soundInstanceRoar[1] = Main.soundRoar[1].CreateInstance();
					}
					catch (Exception exception27)
					{
						TCCL.Log.Write(exception27);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Splash_0")))
				{
					try
					{
						Main.soundSplash[0] = this.LoadWav(name, "Splash_0");
						Main.soundInstanceSplash[0] = Main.soundSplash[0].CreateInstance();
					}
					catch (Exception exception28)
					{
						TCCL.Log.Write(exception28);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Splash_1")))
				{
					try
					{
						Main.soundSplash[1] = this.LoadWav(name, "Splash_1");
						Main.soundInstanceSplash[1] = Main.soundSplash[1].CreateInstance();
					}
					catch (Exception exception29)
					{
						TCCL.Log.Write(exception29);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Double_Jump")))
				{
					try
					{
						Main.soundDoubleJump = this.LoadWav(name, "Double_Jump");
						Main.soundInstanceDoubleJump = Main.soundDoubleJump.CreateInstance();
					}
					catch (Exception exception30)
					{
						TCCL.Log.Write(exception30);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Run")))
				{
					try
					{
						Main.soundRun = this.LoadWav(name, "Run");
						Main.soundInstanceRun = Main.soundRun.CreateInstance();
					}
					catch (Exception exception31)
					{
						TCCL.Log.Write(exception31);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Coins")))
				{
					try
					{
						Main.soundCoins = this.LoadWav(name, "Coins");
						Main.soundInstanceCoins = Main.soundCoins.CreateInstance();
					}
					catch (Exception exception32)
					{
						TCCL.Log.Write(exception32);
					}
				}
				if (File.Exists(this.GetWavPath(name, "MaxMana")))
				{
					try
					{
						Main.soundMaxMana = this.LoadWav(name, "MaxMana");
						Main.soundInstanceMaxMana = Main.soundMenuTick.CreateInstance();
					}
					catch (Exception exception33)
					{
						TCCL.Log.Write(exception33);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Unlock")))
				{
					try
					{
						Main.soundUnlock = this.LoadWav(name, "Unlock");
						Main.soundInstanceUnlock = Main.soundMenuTick.CreateInstance();
					}
					catch (Exception exception34)
					{
						TCCL.Log.Write(exception34);
					}
				}
				if (File.Exists(this.GetWavPath(name, "Drown")))
				{
					try
					{
						Main.soundDrown = this.LoadWav(name, "Drown");
						Main.soundInstanceDrown = Main.soundDrown.CreateInstance();
					}
					catch (Exception exception35)
					{
						TCCL.Log.Write(exception35);
					}
				}
				for (int i = 1; i < (int)Main.soundItem.Length; i++)
				{
					if (File.Exists(this.GetWavPath(name, string.Concat("Item_", i))))
					{
						try
						{
							Main.soundItem[i] = this.LoadWav(name, string.Concat("Item_", i));
							Main.soundInstanceItem[i] = Main.soundItem[i].CreateInstance();
						}
						catch (Exception exception36)
						{
							TCCL.Log.Write(exception36);
						}
					}
				}
				for (int j = 1; j < (int)Main.soundNPCHit.Length; j++)
				{
					if (File.Exists(this.GetWavPath(name, string.Concat("NPC_Hit_", j))))
					{
						try
						{
							Main.soundNPCHit[j] = this.LoadWav(name, string.Concat("NPC_Hit_", j));
							Main.soundInstanceNPCHit[j] = Main.soundNPCHit[j].CreateInstance();
						}
						catch (Exception exception37)
						{
							TCCL.Log.Write(exception37);
						}
					}
				}
				for (int k = 1; k < (int)Main.soundNPCKilled.Length; k++)
				{
					if (File.Exists(this.GetWavPath(name, string.Concat("NPC_Killed_", k))))
					{
						try
						{
							Main.soundNPCKilled[k] = this.LoadWav(name, string.Concat("NPC_Killed_", k));
							Main.soundInstanceNPCKilled[k] = Main.soundNPCKilled[k].CreateInstance();
						}
						catch (Exception exception38)
						{
							TCCL.Log.Write(exception38);
						}
					}
				}
			}
		}

		private void LoadFonts()
		{
			Main.fontDeathText = base.Content.Load<SpriteFont>("Fonts\\Death_Text");
			Main.fontItemStack = base.Content.Load<SpriteFont>("Fonts\\Item_Stack");
			Main.fontMouseText = base.Content.Load<SpriteFont>("Fonts\\Mouse_Text");
			Main.fontCombatText[0] = base.Content.Load<SpriteFont>("Fonts\\Combat_Text");
			Main.fontCombatText[1] = base.Content.Load<SpriteFont>("Fonts\\Combat_Crit");
		}

		private void LoadGore(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 1; i < (int)Main.goreTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.goreTexture[i], string.Concat("Gore_", i));
					Main.goreLoaded[i] = true;
				}
				return;
			}
			for (int j = 1; j < (int)Main.goreTexture.Length; j++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Gore_", j))))
				{
					Main.goreTexture[j] = this.LoadPng(name, string.Concat("Gore_", j));
					Main.goreLoaded[j] = true;
				}
			}
		}

		private void LoadHair(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 0; i < (int)Main.playerHairTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.playerHairTexture[i], string.Concat("Player_Hair_", i + 1));
					Main.hairLoaded[i] = true;
				}
				for (int j = 0; j < (int)Main.playerHairTexture.Length; j++)
				{
					this.AddTextureToDictionary(ref Main.playerHairAltTexture[j], string.Concat("Player_HairAlt_", j + 1));
					Main.hairLoaded[j] = true;
				}
				return;
			}
			for (int k = 0; k < (int)Main.playerHairTexture.Length; k++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Player_Hair_", k + 1))))
				{
					Main.playerHairTexture[k] = this.LoadPng(name, string.Concat("Player_Hair_", k + 1));
				}
				if (File.Exists(this.GetPngPath(name, string.Concat("Player_HairAlt_", k + 1))))
				{
					Main.playerHairAltTexture[k] = this.LoadPng(name, string.Concat("Player_HairAlt_", k + 1));
				}
				Main.hairLoaded[k] = true;
			}
		}

		private void LoadImages()
		{
			Splasher.SetMinMaxValues(0, (int)Directory.GetFiles(string.Concat(Helper.StartUpPath, "\\Content\\Images"), "*.xnb", SearchOption.AllDirectories).Length);
			this.LoadArmor("", false);
			this.LoadBackground("", false);
			this.LoadGore("", false);
			this.LoadHair("", false);
			this.LoadItemFlames("", false);
			this.LoadNPC("", false);
			this.LoadProjectile("", false);
			this.LoadTiles("", false);
			this.LoadWalls("", false);
			this.LoadWings("", false);
			this.LoadAcc("", false);
			for (int i = 0; i < (int)this.cursorTexture2.Length; i++)
			{
				this.AddTextureToDictionary(ref this.cursorTexture2[i], string.Concat("Cursor_", i));
			}
			this.AddTextureToDictionary(ref Main.smartDigTexture, "SmartDig");
			this.AddTextureToDictionary(ref Main.frozenTexture, "Frozen");
			this.AddTextureToDictionary(ref Main.craftButtonTexture, "CraftButton");
			this.AddTextureToDictionary(ref Main.craftUpButtonTexture, "RecUp");
			this.AddTextureToDictionary(ref Main.craftDownButtonTexture, "RecDown");
			this.AddTextureToDictionary(ref Main.scrollLeftButtonTexture, "RecLeft");
			this.AddTextureToDictionary(ref Main.scrollRightButtonTexture, "RecRight");
			this.AddTextureToDictionary(ref Main.pulleyTexture, "PlayerPulley");
			this.AddTextureToDictionary(ref Main.reforgeTexture, "Reforge");
			this.AddTextureToDictionary(ref Main.timerTexture, "Timer");
			this.AddTextureToDictionary(ref Main.wofTexture, "WallOfFlesh");
			this.AddTextureToDictionary(ref Main.wallOutlineTexture, "Wall_Outline");
			this.AddTextureToDictionary(ref Main.fadeTexture, "fade-out");
			this.AddTextureToDictionary(ref Main.ghostTexture, "Ghost");
			this.AddTextureToDictionary(ref Main.evilCactusTexture, "Evil_Cactus");
			this.AddTextureToDictionary(ref Main.goodCactusTexture, "Good_Cactus");
			this.AddTextureToDictionary(ref Main.crimsonCactusTexture, "Crimson_Cactus");
			this.AddTextureToDictionary(ref Main.wraithEyeTexture, "Wraith_Eyes");
			this.AddTextureToDictionary(ref Main.reaperEyeTexture, "Reaper_Eyes");
			this.AddTextureToDictionary(ref Main.fireflyTexture, "FireFly");
			this.AddTextureToDictionary(ref Main.fireflyJarTexture, "FireFlyJar");
			this.AddTextureToDictionary(ref Main.lightningbugTexture, "LightningBug");
			this.AddTextureToDictionary(ref Main.lightningbugJarTexture, "LightningBugJar");
			for (int j = 0; j < (int)Main.jellyfishBowlTexture.Length; j++)
			{
				this.AddTextureToDictionary(ref Main.jellyfishBowlTexture[j], string.Concat("jellyfishBowl", j + 1));
			}
			this.AddTextureToDictionary(ref Main.rainTexture[0], "Rain_0");
			this.AddTextureToDictionary(ref Main.rainTexture[1], "Rain_1");
			this.AddTextureToDictionary(ref Main.rainTexture[2], "Rain_2");
			this.AddTextureToDictionary(ref Main.rainTexture[3], "Rain_3");
			this.AddTextureToDictionary(ref Main.rainTexture[4], "Rain_4");
			this.AddTextureToDictionary(ref Main.rainTexture[5], "Rain_5");
			this.AddTextureToDictionary(ref Main.magicPixel, "MagicPixel");
			this.AddTextureToDictionary(ref Main.miniMapFrameTexture, "MiniMapFrame");
			this.AddTextureToDictionary(ref Main.miniMapFrame2Texture, "MiniMapFrame2");
			for (int k = 0; k < (int)Main.FlameTexture.Length; k++)
			{
				this.AddTextureToDictionary(ref Main.FlameTexture[k], string.Concat("Flame_", k));
			}
			for (int l = 0; l < 3; l++)
			{
				this.AddTextureToDictionary(ref Main.miniMapButtonTexture[l], string.Concat("MiniMapButton_", l));
			}
			this.AddTextureToDictionary(ref Main.destTexture[0], "Dest1");
			this.AddTextureToDictionary(ref Main.destTexture[1], "Dest2");
			this.AddTextureToDictionary(ref Main.destTexture[2], "Dest3");
			this.AddTextureToDictionary(ref Main.flyingCarpetTexture, "FlyingCarpet");
			this.AddTextureToDictionary(ref Main.hbTexture1, "HealthBar1");
			this.AddTextureToDictionary(ref Main.hbTexture2, "HealthBar2");
			this.AddTextureToDictionary(ref Main.actuatorTexture, "Actuator");
			this.AddTextureToDictionary(ref Main.wireTexture, "Wires");
			this.AddTextureToDictionary(ref Main.wire2Texture, "Wires2");
			this.AddTextureToDictionary(ref Main.wire3Texture, "Wires3");
			int num = 1;
			if (Main.rand != null)
			{
				num = Main.rand.Next(1, 9);
			}
			this.AddTextureToDictionary(ref Main.loTexture, string.Concat("logo_", num));
			for (int m = 1; m < 2; m++)
			{
				this.AddTextureToDictionary(ref Main.bannerTexture[m], string.Concat("House_Banner_", m));
			}
			for (int n = 0; n < (int)Main.npcHeadTexture.Length; n++)
			{
				this.AddTextureToDictionary(ref Main.npcHeadTexture[n], string.Concat("NPC_Head_", n));
			}
			for (int o = 1; o < (int)Main.BackPackTexture.Length; o++)
			{
				this.AddTextureToDictionary(ref Main.BackPackTexture[o], string.Concat("BackPack_", o));
			}
			for (int p = 1; p < (int)Main.buffTexture.Length; p++)
			{
				this.AddTextureToDictionary(ref Main.buffTexture[p], string.Concat("Buff_", p));
			}
			for (int q = 0; q < (int)Main.itemTexture.Length; q++)
			{
				this.AddTextureToDictionary(ref Main.itemTexture[q], string.Concat("Item_", q));
			}
			for (int r = 0; r < (int)Main.gemTexture.Length; r++)
			{
				this.AddTextureToDictionary(ref Main.gemTexture[r], string.Concat("Gem_", r));
			}
			for (int s = 0; s < (int)Main.cloudTexture.Length; s++)
			{
				this.AddTextureToDictionary(ref Main.cloudTexture[s], string.Concat("Cloud_", s));
			}
			for (int t = 0; t < (int)Main.starTexture.Length; t++)
			{
				this.AddTextureToDictionary(ref Main.starTexture[t], string.Concat("Star_", t));
			}
			for (int u = 0; u < (int)Main.liquidTexture.Length; u++)
			{
				this.AddTextureToDictionary(ref Main.liquidTexture[u], string.Concat("Liquid_", u));
			}
			for (int v = 0; v < (int)this.waterfallManager.waterfallTexture.Length; v++)
			{
				this.AddTextureToDictionary(ref this.waterfallManager.waterfallTexture[v], string.Concat("Waterfall_", v));
			}
			this.AddTextureToDictionary(ref Main.npcToggleTexture[0], "House_1");
			this.AddTextureToDictionary(ref Main.npcToggleTexture[1], "House_2");
			this.AddTextureToDictionary(ref Main.HBLockTexture[0], "Lock_0");
			this.AddTextureToDictionary(ref Main.HBLockTexture[1], "Lock_1");
			this.AddTextureToDictionary(ref Main.gridTexture, "Grid");
			this.AddTextureToDictionary(ref Main.trashTexture, "Trash");
			this.AddTextureToDictionary(ref Main.cdTexture, "CoolDown");
			this.AddTextureToDictionary(ref Main.logoTexture, "Logo");
			this.AddTextureToDictionary(ref Main.logo2Texture, "Logo2");
			this.AddTextureToDictionary(ref Main.dustTexture, "Dust");
			this.AddTextureToDictionary(ref Main.sunTexture, "Sun");
			this.AddTextureToDictionary(ref Main.sun2Texture, "Sun2");
			this.AddTextureToDictionary(ref Main.sun3Texture, "Sun3");
			this.AddTextureToDictionary(ref Main.blackTileTexture, "Black_Tile");
			this.AddTextureToDictionary(ref Main.heartTexture, "Heart");
			this.AddTextureToDictionary(ref Main.heart2Texture, "Heart2");
			this.AddTextureToDictionary(ref Main.bubbleTexture, "Bubble");
			this.AddTextureToDictionary(ref Main.flameTexture, "Flame");
			this.AddTextureToDictionary(ref Main.manaTexture, "Mana");
			this.AddTextureToDictionary(ref Main.cursorTexture, "Cursor");
			this.AddTextureToDictionary(ref Main.cursor2Texture, "Cursor2");
			this.AddTextureToDictionary(ref Main.ninjaTexture, "Ninja");
			this.AddTextureToDictionary(ref Main.antLionTexture, "AntlionBody");
			this.AddTextureToDictionary(ref Main.spikeBaseTexture, "Spike_Base");
			for (int w = 0; w < (int)Main.woodTexture.Length; w++)
			{
				this.AddTextureToDictionary(ref Main.woodTexture[w], string.Concat("Tiles_5_", w));
			}
			for (int x = 0; x < (int)Main.moonTexture.Length; x++)
			{
				this.AddTextureToDictionary(ref Main.moonTexture[x], string.Concat("Moon_", x));
			}
			for (int y = 0; y < (int)Main.treeTopTexture.Length; y++)
			{
				this.AddTextureToDictionary(ref Main.treeTopTexture[y], string.Concat("Tree_Tops_", y));
			}
			for (int a = 0; a < (int)Main.treeBranchTexture.Length; a++)
			{
				this.AddTextureToDictionary(ref Main.treeBranchTexture[a], string.Concat("Tree_Branches_", a));
			}
			this.AddTextureToDictionary(ref Main.shroomCapTexture, "Shroom_Tops");
			this.AddTextureToDictionary(ref Main.inventoryBackTexture, "Inventory_Back");
			this.AddTextureToDictionary(ref Main.inventoryBack2Texture, "Inventory_Back2");
			this.AddTextureToDictionary(ref Main.inventoryBack3Texture, "Inventory_Back3");
			this.AddTextureToDictionary(ref Main.inventoryBack4Texture, "Inventory_Back4");
			this.AddTextureToDictionary(ref Main.inventoryBack5Texture, "Inventory_Back5");
			this.AddTextureToDictionary(ref Main.inventoryBack6Texture, "Inventory_Back6");
			this.AddTextureToDictionary(ref Main.inventoryBack7Texture, "Inventory_Back7");
			this.AddTextureToDictionary(ref Main.inventoryBack8Texture, "Inventory_Back8");
			this.AddTextureToDictionary(ref Main.inventoryBack9Texture, "Inventory_Back9");
			this.AddTextureToDictionary(ref Main.inventoryBack10Texture, "Inventory_Back10");
			this.AddTextureToDictionary(ref Main.inventoryBack11Texture, "Inventory_Back11");
			this.AddTextureToDictionary(ref Main.inventoryBack12Texture, "Inventory_Back12");
			this.AddTextureToDictionary(ref Main.inventoryBack13Texture, "Inventory_Back13");
			this.AddTextureToDictionary(ref Main.inventoryBack14Texture, "Inventory_Back14");
			this.AddTextureToDictionary(ref Main.hairStyleBackTexture, "HairStyleBack");
			this.AddTextureToDictionary(ref Main.clothesStyleBackTexture, "ClothesStyleBack");
			this.AddTextureToDictionary(ref Main.inventoryTickOffTexture, "Inventory_Tick_Off");
			this.AddTextureToDictionary(ref Main.inventoryTickOnTexture, "Inventory_Tick_On");
			this.AddTextureToDictionary(ref Main.textBackTexture, "Text_Back");
			this.AddTextureToDictionary(ref Main.chatTexture, "Chat");
			this.AddTextureToDictionary(ref Main.chat2Texture, "Chat2");
			this.AddTextureToDictionary(ref Main.chatBackTexture, "Chat_Back");
			this.AddTextureToDictionary(ref Main.teamTexture, "Team");
			this.AddTextureToDictionary(ref Main.skinArmTexture, "Skin_Arm");
			this.AddTextureToDictionary(ref Main.skinBodyTexture, "Skin_Body");
			this.AddTextureToDictionary(ref Main.skinLegsTexture, "Skin_Legs");
			this.AddTextureToDictionary(ref Main.playerEyeWhitesTexture, "Player_Eye_Whites");
			this.AddTextureToDictionary(ref Main.playerEyesTexture, "Player_Eyes");
			this.AddTextureToDictionary(ref Main.playerHandsTexture, "Player_Hands");
			this.AddTextureToDictionary(ref Main.playerHands2Texture, "Player_Hands2");
			this.AddTextureToDictionary(ref Main.playerHeadTexture, "Player_Head");
			this.AddTextureToDictionary(ref Main.playerPantsTexture, "Player_Pants");
			this.AddTextureToDictionary(ref Main.playerShirtTexture, "Player_Shirt");
			this.AddTextureToDictionary(ref Main.playerShoesTexture, "Player_Shoes");
			this.AddTextureToDictionary(ref Main.playerUnderShirtTexture, "Player_Undershirt");
			this.AddTextureToDictionary(ref Main.playerUnderShirt2Texture, "Player_Undershirt2");
			this.AddTextureToDictionary(ref Main.femalePantsTexture, "Female_Pants");
			this.AddTextureToDictionary(ref Main.femaleShirtTexture, "Female_Shirt");
			this.AddTextureToDictionary(ref Main.femaleShoesTexture, "Female_Shoes");
			this.AddTextureToDictionary(ref Main.femaleUnderShirtTexture, "Female_Undershirt");
			this.AddTextureToDictionary(ref Main.femaleUnderShirt2Texture, "Female_Undershirt2");
			this.AddTextureToDictionary(ref Main.femaleShirt2Texture, "Female_Shirt2");
			this.AddTextureToDictionary(ref Main.chaosTexture, "Chaos");
			this.AddTextureToDictionary(ref Main.EyeLaserTexture, "Eye_Laser");
			this.AddTextureToDictionary(ref Main.BoneEyesTexture, "Bone_eyes");
			this.AddTextureToDictionary(ref Main.BoneLaserTexture, "Bone_Laser");
			this.AddTextureToDictionary(ref Main.lightDiscTexture, "Light_Disc");
			this.AddTextureToDictionary(ref Main.confuseTexture, "Confuse");
			this.AddTextureToDictionary(ref Main.probeTexture, "Probe");
			this.AddTextureToDictionary(ref Main.sunOrbTexture, "SunOrb");
			this.AddTextureToDictionary(ref Main.beetleTexture, "BeetleOrb");
			this.AddTextureToDictionary(ref Main.sunAltarTexture, "SunAltar");
			this.AddTextureToDictionary(ref Main.fishingLineTexture, "FishingLine");
			this.AddTextureToDictionary(ref Main.chainTexture, "Chain");
			this.AddTextureToDictionary(ref Main.chain2Texture, "Chain2");
			this.AddTextureToDictionary(ref Main.chain3Texture, "Chain3");
			this.AddTextureToDictionary(ref Main.chain4Texture, "Chain4");
			this.AddTextureToDictionary(ref Main.chain5Texture, "Chain5");
			this.AddTextureToDictionary(ref Main.chain6Texture, "Chain6");
			this.AddTextureToDictionary(ref Main.chain7Texture, "Chain7");
			this.AddTextureToDictionary(ref Main.chain8Texture, "Chain8");
			this.AddTextureToDictionary(ref Main.chain9Texture, "Chain9");
			this.AddTextureToDictionary(ref Main.chain10Texture, "Chain10");
			this.AddTextureToDictionary(ref Main.chain11Texture, "Chain11");
			this.AddTextureToDictionary(ref Main.chain12Texture, "Chain12");
			this.AddTextureToDictionary(ref Main.chain13Texture, "Chain13");
			this.AddTextureToDictionary(ref Main.chain14Texture, "Chain14");
			this.AddTextureToDictionary(ref Main.chain15Texture, "Chain15");
			this.AddTextureToDictionary(ref Main.chain16Texture, "Chain16");
			this.AddTextureToDictionary(ref Main.chain17Texture, "Chain17");
			this.AddTextureToDictionary(ref Main.chain18Texture, "Chain18");
			this.AddTextureToDictionary(ref Main.chain19Texture, "Chain19");
			this.AddTextureToDictionary(ref Main.chain20Texture, "Chain20");
			this.AddTextureToDictionary(ref Main.chain21Texture, "Chain21");
			this.AddTextureToDictionary(ref Main.chain22Texture, "Chain22");
			this.AddTextureToDictionary(ref Main.chain23Texture, "Chain23");
			this.AddTextureToDictionary(ref Main.chain24Texture, "Chain24");
			this.AddTextureToDictionary(ref Main.chain25Texture, "Chain25");
			this.AddTextureToDictionary(ref Main.chain26Texture, "Chain26");
			this.AddTextureToDictionary(ref Main.chain27Texture, "Chain27");
			this.AddTextureToDictionary(ref Main.chain28Texture, "Chain28");
			this.AddTextureToDictionary(ref Main.chain29Texture, "Chain29");
			this.AddTextureToDictionary(ref Main.chain30Texture, "Chain30");
			this.AddTextureToDictionary(ref Main.chain31Texture, "Chain31");
			this.AddTextureToDictionary(ref Main.chain32Texture, "Chain32");
			this.AddTextureToDictionary(ref Main.chain33Texture, "Chain33");
			this.AddTextureToDictionary(ref Main.chain34Texture, "Chain34");
			this.AddTextureToDictionary(ref Main.chain35Texture, "Chain35");
			this.AddTextureToDictionary(ref Main.chain36Texture, "Chain36");
			this.AddTextureToDictionary(ref Main.chain37Texture, "Chain37");
			this.AddTextureToDictionary(ref Main.boneArmTexture, "Arm_Bone");
			this.AddTextureToDictionary(ref Main.boneArm2Texture, "Arm_Bone_2");
			for (int b = 1; b < (int)Main.gemChainTexture.Length; b++)
			{
				this.AddTextureToDictionary(ref Main.gemChainTexture[b], string.Concat("GemChain_", b));
			}
			for (int c = 1; c < (int)Main.golemTexture.Length; c++)
			{
				this.AddTextureToDictionary(ref Main.golemTexture[c], string.Concat("GolemLights", c));
			}
			this.AddTextureToDictionary(ref Main.jackHatTexture, "JackHat");
			this.AddTextureToDictionary(ref Main.treeFaceTexture, "TreeFace");
			this.AddTextureToDictionary(ref Main.pumpkingArmTexture, "PumpkingArm");
			this.AddTextureToDictionary(ref Main.pumpkingCloakTexture, "PumpkingCloak");
			this.AddTextureToDictionary(ref Main.pumpkingFaceTexture, "PumpkingFace");
			this.AddTextureToDictionary(ref Main.pumpkinMoonTexture, "Moon_Pumpking");
			this.AddTextureToDictionary(ref Main.snowMoonTexture, "Moon_Snow");
			this.AddTextureToDictionary(ref Main.xmasLightTexture, "XmasLight");
			this.AddTextureToDictionary(ref Main.iceQueenTexture, "IceQueen");
			this.AddTextureToDictionary(ref Main.glowSnailTexture, "GlowSnail");
			this.AddTextureToDictionary(ref Main.santaTankTexture, "SantaTank");
			this.AddTextureToDictionary(ref Main.dukeFishronTexture, "DukeFishron");
			this.AddTextureToDictionary(ref Main.miniMinotaurTexture, "MiniMinotaur");
			this.AddTextureToDictionary(ref Main.bunnyMountTexture, "Mount_Bunny");
			this.AddTextureToDictionary(ref Main.minecartMountTexture, "Mount_Minecart");
			this.AddTextureToDictionary(ref Main.pigronMountTexture, "Mount_Pigron");
			this.AddTextureToDictionary(ref Main.slimeMountTexture, "Mount_Slime");
			this.AddTextureToDictionary(ref Main.turtleMountTexture, "Mount_Turtle");
			this.AddTextureToDictionary(ref Main.beeMountTexture[0], "Mount_Bee");
			this.AddTextureToDictionary(ref Main.beeMountTexture[1], "Mount_BeeWings");
			for (int d = 0; d < (int)Main.rudolphMountTexture.Length; d++)
			{
				this.AddTextureToDictionary(ref Main.rudolphMountTexture[d], string.Concat("rudolph_", d));
			}
			for (int e = 0; e < (int)Main.woodTexture.Length; e++)
			{
				this.AddTextureToDictionary(ref Main.woodTexture[e], string.Concat("Tiles_5_", e));
			}
			for (int f = 0; f < (int)Main.xmasTree.Length; f++)
			{
				this.AddTextureToDictionary(ref Main.xmasTree[f], string.Concat("xmas_", f));
			}
		}

		private void LoadItemFlames(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 0; i < (int)Main.itemFlameTexture.Length; i++)
				{
					try
					{
						this.AddTextureToDictionary(ref Main.itemFlameTexture[i], string.Concat("ItemFlame_", i));
					}
					catch
					{
					}
					Main.itemFlameLoaded[i] = true;
				}
				return;
			}
			for (int j = 0; j < (int)Main.itemFlameTexture.Length; j++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("ItemFlame_", j))))
				{
					Main.itemFlameTexture[j] = this.LoadPng(name, string.Concat("ItemFlame_", j));
					Main.itemFlameLoaded[j] = true;
				}
			}
		}

		private void LoadNPC(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 0; i < (int)Main.npcTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.npcTexture[i], string.Concat("NPC_", i));
					Main.NPCLoaded[i] = true;
				}
				return;
			}
			for (int j = 0; j < (int)Main.npcTexture.Length; j++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("NPC_", j))))
				{
					Main.npcTexture[j] = this.LoadPng(name, string.Concat("NPC_", j));
					Main.NPCLoaded[j] = true;
				}
			}
		}

		private void LoadPacks()
		{
			string[] directories = Directory.GetDirectories(string.Concat(Helper.StartUpPath, "\\Custom Content"));
			Packs.Clear();
			for (int i = 0; i < (int)directories.Length; i++)
			{
				Pack pack = new Pack()
				{
					sName = directories[i].Replace(string.Concat(Helper.StartUpPath, "\\Custom Content\\"), "")
				};
				pack.Read();
				Packs.Add(pack);
			}
		}

		private Texture2D LoadPng(string modName, string pngFile)
		{
			return this.LoadPng2(modName, pngFile);
		}

		private Texture2D LoadPng2(string modName, string pngFile)
		{
			Splasher.Status = string.Concat("Loading: ", modName, "\n", pngFile);
			Texture2D preMultipliedAlpha = null;
			using (Stream stream = File.Open(Path.Combine("Custom Content", modName, "Images", string.Concat(pngFile, ".png")), FileMode.Open, FileAccess.Read))
			{
				preMultipliedAlpha = Texture2D.FromStream(base.GraphicsDevice, stream).ConvertToPreMultipliedAlpha();
			}
			Splasher.Progress = Splasher.Progress + 1;
			return preMultipliedAlpha;
		}

		private void LoadProjectile(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 0; i < (int)Main.projectileTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.projectileTexture[i], string.Concat("Projectile_", i));
					Main.projectileLoaded[i] = true;
				}
				return;
			}
			for (int j = 0; j < (int)Main.projectileTexture.Length; j++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Projectile_", j))))
				{
					Main.projectileTexture[j] = this.LoadPng(name, string.Concat("Projectile_", j));
					Main.projectileLoaded[j] = true;
				}
			}
		}

		private void LoadSounds()
		{
			Main.soundMech[0] = base.Content.Load<SoundEffect>("Sounds\\Mech_0");
			Main.soundInstanceMech[0] = Main.soundMech[0].CreateInstance();
			Main.soundGrab = base.Content.Load<SoundEffect>("Sounds\\Grab");
			Main.soundInstanceGrab = Main.soundGrab.CreateInstance();
			Main.soundPixie = base.Content.Load<SoundEffect>("Sounds\\Pixie");
			Main.soundInstancePixie = Main.soundGrab.CreateInstance();
			Main.soundDig[0] = base.Content.Load<SoundEffect>("Sounds\\Dig_0");
			Main.soundInstanceDig[0] = Main.soundDig[0].CreateInstance();
			Main.soundDig[1] = base.Content.Load<SoundEffect>("Sounds\\Dig_1");
			Main.soundInstanceDig[1] = Main.soundDig[1].CreateInstance();
			Main.soundDig[2] = base.Content.Load<SoundEffect>("Sounds\\Dig_2");
			Main.soundInstanceDig[2] = Main.soundDig[2].CreateInstance();
			Main.soundTink[0] = base.Content.Load<SoundEffect>("Sounds\\Tink_0");
			Main.soundInstanceTink[0] = Main.soundTink[0].CreateInstance();
			Main.soundTink[1] = base.Content.Load<SoundEffect>("Sounds\\Tink_1");
			Main.soundInstanceTink[1] = Main.soundTink[1].CreateInstance();
			Main.soundTink[2] = base.Content.Load<SoundEffect>("Sounds\\Tink_2");
			Main.soundInstanceTink[2] = Main.soundTink[2].CreateInstance();
			Main.soundPlayerHit[0] = base.Content.Load<SoundEffect>("Sounds\\Player_Hit_0");
			Main.soundInstancePlayerHit[0] = Main.soundPlayerHit[0].CreateInstance();
			Main.soundPlayerHit[1] = base.Content.Load<SoundEffect>("Sounds\\Player_Hit_1");
			Main.soundInstancePlayerHit[1] = Main.soundPlayerHit[1].CreateInstance();
			Main.soundPlayerHit[2] = base.Content.Load<SoundEffect>("Sounds\\Player_Hit_2");
			Main.soundInstancePlayerHit[2] = Main.soundPlayerHit[2].CreateInstance();
			Main.soundFemaleHit[0] = base.Content.Load<SoundEffect>("Sounds\\Female_Hit_0");
			Main.soundInstanceFemaleHit[0] = Main.soundFemaleHit[0].CreateInstance();
			Main.soundFemaleHit[1] = base.Content.Load<SoundEffect>("Sounds\\Female_Hit_1");
			Main.soundInstanceFemaleHit[1] = Main.soundFemaleHit[1].CreateInstance();
			Main.soundFemaleHit[2] = base.Content.Load<SoundEffect>("Sounds\\Female_Hit_2");
			Main.soundInstanceFemaleHit[2] = Main.soundFemaleHit[2].CreateInstance();
			Main.soundPlayerKilled = base.Content.Load<SoundEffect>("Sounds\\Player_Killed");
			Main.soundInstancePlayerKilled = Main.soundPlayerKilled.CreateInstance();
			Main.soundChat = base.Content.Load<SoundEffect>("Sounds\\Chat");
			Main.soundInstanceChat = Main.soundChat.CreateInstance();
			Main.soundGrass = base.Content.Load<SoundEffect>("Sounds\\Grass");
			Main.soundInstanceGrass = Main.soundGrass.CreateInstance();
			Main.soundDoorOpen = base.Content.Load<SoundEffect>("Sounds\\Door_Opened");
			Main.soundInstanceDoorOpen = Main.soundDoorOpen.CreateInstance();
			Main.soundDoorClosed = base.Content.Load<SoundEffect>("Sounds\\Door_Closed");
			Main.soundInstanceDoorClosed = Main.soundDoorClosed.CreateInstance();
			Main.soundMenuTick = base.Content.Load<SoundEffect>("Sounds\\Menu_Tick");
			Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
			Main.soundMenuOpen = base.Content.Load<SoundEffect>("Sounds\\Menu_Open");
			Main.soundInstanceMenuOpen = Main.soundMenuOpen.CreateInstance();
			Main.soundMenuClose = base.Content.Load<SoundEffect>("Sounds\\Menu_Close");
			Main.soundInstanceMenuClose = Main.soundMenuClose.CreateInstance();
			Main.soundShatter = base.Content.Load<SoundEffect>("Sounds\\Shatter");
			Main.soundInstanceShatter = Main.soundShatter.CreateInstance();
			Main.soundZombie[0] = base.Content.Load<SoundEffect>("Sounds\\Zombie_0");
			Main.soundInstanceZombie[0] = Main.soundZombie[0].CreateInstance();
			Main.soundZombie[1] = base.Content.Load<SoundEffect>("Sounds\\Zombie_1");
			Main.soundInstanceZombie[1] = Main.soundZombie[1].CreateInstance();
			Main.soundZombie[2] = base.Content.Load<SoundEffect>("Sounds\\Zombie_2");
			Main.soundInstanceZombie[2] = Main.soundZombie[2].CreateInstance();
			Main.soundZombie[3] = base.Content.Load<SoundEffect>("Sounds\\Zombie_3");
			Main.soundInstanceZombie[3] = Main.soundZombie[3].CreateInstance();
			Main.soundZombie[4] = base.Content.Load<SoundEffect>("Sounds\\Zombie_4");
			Main.soundInstanceZombie[4] = Main.soundZombie[4].CreateInstance();
			Main.soundRoar[0] = base.Content.Load<SoundEffect>("Sounds\\Roar_0");
			Main.soundInstanceRoar[0] = Main.soundRoar[0].CreateInstance();
			Main.soundRoar[1] = base.Content.Load<SoundEffect>("Sounds\\Roar_1");
			Main.soundInstanceRoar[1] = Main.soundRoar[1].CreateInstance();
			Main.soundSplash[0] = base.Content.Load<SoundEffect>("Sounds\\Splash_0");
			Main.soundInstanceSplash[0] = Main.soundRoar[0].CreateInstance();
			Main.soundSplash[1] = base.Content.Load<SoundEffect>("Sounds\\Splash_1");
			Main.soundInstanceSplash[1] = Main.soundSplash[1].CreateInstance();
			Main.soundDoubleJump = base.Content.Load<SoundEffect>("Sounds\\Double_Jump");
			Main.soundInstanceDoubleJump = Main.soundRoar[0].CreateInstance();
			Main.soundRun = base.Content.Load<SoundEffect>("Sounds\\Run");
			Main.soundInstanceRun = Main.soundRun.CreateInstance();
			Main.soundCoins = base.Content.Load<SoundEffect>("Sounds\\Coins");
			Main.soundInstanceCoins = Main.soundCoins.CreateInstance();
			Main.soundUnlock = base.Content.Load<SoundEffect>("Sounds\\Unlock");
			Main.soundInstanceUnlock = Main.soundUnlock.CreateInstance();
			Main.soundMaxMana = base.Content.Load<SoundEffect>("Sounds\\MaxMana");
			Main.soundInstanceMaxMana = Main.soundMaxMana.CreateInstance();
			Main.soundDrown = base.Content.Load<SoundEffect>("Sounds\\Drown");
			Main.soundInstanceDrown = Main.soundDrown.CreateInstance();
			for (int i = 1; i < (int)Main.soundItem.Length; i++)
			{
				Main.soundItem[i] = base.Content.Load<SoundEffect>(string.Concat("Sounds\\Item_", i));
				Main.soundInstanceItem[i] = Main.soundItem[i].CreateInstance();
			}
			for (int j = 1; j < (int)Main.soundNPCHit.Length; j++)
			{
				Main.soundNPCHit[j] = base.Content.Load<SoundEffect>(string.Concat("Sounds\\NPC_Hit_", j));
				Main.soundInstanceNPCHit[j] = Main.soundNPCHit[j].CreateInstance();
			}
			for (int k = 1; k < (int)Main.soundNPCKilled.Length; k++)
			{
				Main.soundNPCKilled[k] = base.Content.Load<SoundEffect>(string.Concat("Sounds\\NPC_Killed_", k));
				Main.soundInstanceNPCKilled[k] = Main.soundNPCKilled[k].CreateInstance();
			}
			for (int l = 1; l < (int)Main.soundLiquid.Length; l++)
			{
				Main.soundLiquid[l] = base.Content.Load<SoundEffect>(string.Concat("Sounds\\Liquid_", l));
				Main.soundInstanceItem[l] = Main.soundLiquid[l].CreateInstance();
			}
		}

		private void LoadTiles(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 0; i < (int)Main.tileTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.tileTexture[i], string.Concat("Tiles_", i));
					Main.tileSetsLoaded[i] = true;
				}
				return;
			}
			for (int j = 0; j < (int)Main.tileTexture.Length; j++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Tiles_", j))))
				{
					Main.tileTexture[j] = this.LoadPng(name, string.Concat("Tiles_", j));
					Main.tileSetsLoaded[j] = true;
				}
			}
		}

		private void LoadWalls(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 1; i < (int)Main.wallTexture.Length; i++)
				{
					this.AddTextureToDictionary(ref Main.wallTexture[i], string.Concat("Wall_", i));
					Main.wallLoaded[i] = true;
				}
				return;
			}
			for (int j = 1; j < (int)Main.wallTexture.Length; j++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Wall_", j))))
				{
					Main.wallTexture[j] = this.LoadPng(name, string.Concat("Wall_", j));
					Main.wallLoaded[j] = true;
				}
			}
		}

		private SoundEffect LoadWav(string modName, string wavFile)
		{
			SoundEffect soundEffect;
			try
			{
				Splasher.Status = string.Concat("Loading: ", modName, "\n", wavFile);
				FileStream fileStream = new FileStream(this.GetContentPath(modName, wavFile, ".wav", "sounds"), FileMode.Open, FileAccess.ReadWrite);
				SoundEffect soundEffect1 = SoundEffect.FromStream(fileStream);
				fileStream.Close();
				fileStream = null;
				Splasher.Progress = Splasher.Progress + 1;
				soundEffect = soundEffect1;
			}
			catch (Exception exception)
			{
				TCCL.Log.Write(exception);
				base.Exit();
				return null;
			}
			return soundEffect;
		}

		private void LoadWings(string name = "", bool png = false)
		{
			if (!png)
			{
				for (int i = 1; i < (int)Main.wingsTexture.Length; i++)
				{
					try
					{
						this.AddTextureToDictionary(ref Main.wingsTexture[i], string.Concat("Wings_", i));
						Main.wingsLoaded[i] = true;
					}
					catch
					{
					}
				}
				return;
			}
			for (int j = 1; j < (int)Main.wingsTexture.Length; j++)
			{
				if (File.Exists(this.GetPngPath(name, string.Concat("Wings_", j))))
				{
					Main.wingsTexture[j] = this.LoadPng(name, string.Concat("Wings_", j));
					Main.wingsLoaded[j] = true;
				}
			}
		}

		protected override void OnExiting(object sender, System.EventArgs args)
		{
			base.OnExiting(sender, args);
			Config.Save();
			Packs.Write();
			TCCL.Log.Close();
		}

		private bool PngExists(string filename)
		{
			for (int i = Packs.Count - 1; i >= 0; i--)
			{
				if (Packs.LoadPngs(i) && File.Exists(this.GetPngPath(Packs.GetName(i), filename)))
				{
					return true;
				}
			}
			return false;
		}

		private void Recalc(object sender, ResizeEventArgs e)
		{
			for (int i = 0; i < this.advModControl.Count; i++)
			{
				this.advModControl[i].Top = this.vert.Value * -1 + this.offset[i];
			}
			this.savedValue = this.vert.Value;
		}

		private void Refill()
		{
			this.advModControl.Clear();
			this.offset.Clear();
			this.lb.Dispose();
			this.lb = new ImageBox(this.manager)
			{
				Height = 300,
				Width = this.window.Width - 32
			};
			this.lb.SetPosition(10, 10);
			this.lb.Parent = this.window;
			this.lb.Margins = new Margins(0, 0, 0, 0);
			this.vert.Dispose();
			this.vert = new TomShane.Neoforce.Controls.ScrollBar(this.manager, TomShane.Neoforce.Controls.Orientation.Vertical);
			this.vert.Init();
			this.vert.Parent = this.lb;
			this.vert.Top = 0;
			this.vert.Left = this.lb.ClientWidth - this.vert.Width;
			this.vert.Height = this.lb.ClientHeight;
			this.vert.Value = 0;
			this.vert.Anchor = Anchors.Top | Anchors.Right | Anchors.Bottom | Anchors.Vertical;
			this.vert.ValueChanged += new TomShane.Neoforce.Controls.EventHandler(this.ValueChanged);
			for (int i = 0; i < Packs.Count; i++)
			{
				Packs.GetPack(i).priority = i;
			}
			Packs.GetPackArray().Sort();
			int num = 0;
			for (int j = 0; j < Packs.Count; j++)
			{
				AdvModControl advModControl = new AdvModControl(this.manager, this, Packs.GetPack(j));
				advModControl.SetSize(this.lb.Width - this.vert.Width, 32);
				advModControl.SetPosition(0, num);
				advModControl.Load();
				advModControl.Parent = this.lb;
				advModControl.CanFocus = false;
				if (j % 2 != 0)
				{
					advModControl.Color = new Color(74, 105, 120);
				}
				else
				{
					advModControl.Color = new Color(84, 115, 130);
				}
				this.lb.Add(advModControl);
				num += 32;
				this.advModControl.Add(advModControl);
				this.offset.Add(advModControl.Top);
			}
			this.vert.Range = num;
			this.vert.PageSize = this.lb.Height;
			this.vert.Value = this.savedValue;
		}

		private void Reload(object what)
		{
			TCCL.CleanUp();
			switch ((int)what)
			{
				case 0:
				{
					this.LoadImages();
					this.LoadFonts();
					this.LoadBank();
					this.LoadSounds();
					break;
				}
				case 1:
				{
					this.LoadImages();
					break;
				}
				case 2:
				{
					this.LoadFonts();
					break;
				}
				case 3:
				{
					this.LoadSounds();
					break;
				}
				case 4:
				{
					this.LoadBank();
					break;
				}
			}
			for (int i = Packs.Count - 1; i >= 0; i--)
			{
				string name = Packs.GetName(i);
				switch ((int)what)
				{
					case 0:
					{
						Splasher.Progress = 0;
						Splasher.Status = string.Concat("Loading Pack ", Packs.GetName(i), "...");
						Splasher.SetMinMaxValues(0, this.GetFileCount(name, "All"));
						this.LoadCustomPNGs(Packs.LoadPngs(i), Packs.GetName(i));
						Splasher.Status = string.Concat("Loading ", Packs.GetName(i), " Fonts");
						this.LoadCustomFonts(Packs.LoadFonts(i), Packs.GetName(i));
						Splasher.Status = string.Concat("Loading ", Packs.GetName(i), " Sounds");
						this.LoadCustomSounds(Packs.LoadSounds(i), Packs.GetName(i));
						Splasher.Status = string.Concat("Loading ", Packs.GetName(i), " Music");
						this.LoadCustomMusic(Packs.LoadMusic(i), Packs.GetName(i));
						break;
					}
					case 1:
					{
						Splasher.Progress = 0;
						Splasher.Status = string.Concat("Loading Pack ", Packs.GetName(i), "...");
						Splasher.SetMinMaxValues(0, this.GetFileCount(name, "Images"));
						this.LoadCustomPNGs(Packs.LoadPngs(i), Packs.GetName(i));
						break;
					}
					case 2:
					{
						Splasher.Progress = 0;
						Splasher.Status = string.Concat("Loading ", Packs.GetName(i), " Fonts");
						Splasher.SetMinMaxValues(0, this.GetFileCount(name, "Fonts"));
						this.LoadCustomFonts(Packs.LoadFonts(i), Packs.GetName(i));
						break;
					}
					case 3:
					{
						Splasher.Progress = 0;
						Splasher.Status = string.Concat("Loading ", Packs.GetName(i), " Sounds");
						Splasher.SetMinMaxValues(0, this.GetFileCount(name, "Sounds"));
						this.LoadCustomSounds(Packs.LoadSounds(i), Packs.GetName(i));
						break;
					}
					case 4:
					{
						Splasher.Status = string.Concat("Loading ", Packs.GetName(i), " Music");
						this.LoadCustomMusic(Packs.LoadMusic(i), Packs.GetName(i));
						break;
					}
				}
			}
			for (int j = 0; j < Main.tileAltTextureDrawn.GetLength(0); j++)
			{
				for (int k = 0; k < Main.tileAltTextureDrawn.GetLength(1); k++)
				{
					Main.tileAltTextureInit[j, k] = false;
					Main.tileAltTextureDrawn[j, k] = false;
				}
			}
			for (int l = 0; l < Main.wallAltTextureDrawn.GetLength(0); l++)
			{
				for (int m = 0; m < Main.wallAltTextureDrawn.GetLength(1); m++)
				{
					Main.wallAltTextureInit[l, m] = false;
					Main.wallAltTextureDrawn[l, m] = false;
				}
			}
			for (int n = 0; n < Main.treeAltTextureDrawn.GetLength(0); n++)
			{
				for (int o = 0; o < Main.treeAltTextureDrawn.GetLength(1); o++)
				{
					Main.treeAltTextureInit[n, o] = false;
					Main.treeAltTextureDrawn[n, o] = false;
				}
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.Ansi, ExactSpelling=true, SetLastError=true)]
		private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

		protected override void UnloadContent()
		{
			base.Content.Unload();
		}

		protected override void Update(GameTime gameTime)
		{
			this.chkSkipDialog.Focused = false;
			this.chkSkipSplash.Focused = false;
			Main.cursorTexture = Main.itemTexture[0];
			KeyboardState state = Keyboard.GetState();
			if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F5) && !this.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F5))
			{
				this.Reload(0);
			}
			if (this.window != null && this.window.Visible)
			{
				if (Helper.Redraw)
				{
					this.manager.Cursor = this.manager.Skin.Cursors["Busy"].Resource;
					this.Reload(Helper.what);
					this.manager.Cursor = this.manager.Skin.Cursors["Default"].Resource;
					this.Refill();
					Helper.Redraw = false;
					Helper.what = -1;
					Splasher.Close();
				}
				if (Helper.updateList)
				{
					this.Refill();
					Helper.updateList = false;
				}
			}
			if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1) && !this.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1) && this.window != null)
			{
				if (!this.window.Visible)
				{
					this.Refill();
					this.window.Show();
				}
				else
				{
					this.window.Hide();
				}
			}
			if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2) && !this.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2))
			{
				Main.dayTime = !Main.dayTime;
			}
			this.oldKeyState = Keyboard.GetState();
			base.UpdateMusic();
			if (!this.window.Visible)
			{
				this.enableScroll = false;
				base.IsMouseVisible = false;
				base.Update(gameTime);
				return;
			}
			this.enableScroll = true;
			this.manager.Update(gameTime);
			base.IsMouseVisible = true;
		}

		private void ValueChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
			this.Recalc(sender, null);
		}

		private void window_MouseOver(object sender, TomShane.Neoforce.Controls.MouseEventArgs e)
		{
		}

		private void window_ResizeEnd(object sender, TomShane.Neoforce.Controls.EventArgs e)
		{
		}
	}
}