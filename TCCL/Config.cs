using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace TCCL
{
	public static class Config
	{
		public static string config;

		public static bool bShowGui;

		public static bool bSkipSplash;

		public static bool bCursorBorder;

		public static byte iCursorType;

		public static Color colCursorBorderColor;

		public static float fCursorScale;

		static Config()
		{
			Config.config = Path.Combine(Directory.GetCurrentDirectory(), "TCCL.settings");
			Config.bShowGui = true;
			Config.bSkipSplash = false;
			Config.bCursorBorder = false;
			Config.iCursorType = 0;
			Config.colCursorBorderColor = Color.Black;
			Config.fCursorScale = 1f;
		}

		public static Color ColorFromString(string str)
		{
			string[] strArrays = str.Split(new char[] { ':' });
			Color num = new Color();
			try
			{
				num.R = Convert.ToByte(strArrays[0]);
				num.G = Convert.ToByte(strArrays[1]);
				num.B = Convert.ToByte(strArrays[2]);
			}
			catch
			{
				num = Color.Black;
			}
			return num;
		}

		public static string ColorToString(Color color)
		{
			object[] r = new object[] { color.R, ":", color.G, ":", color.B };
			return string.Concat(r);
		}

		public static void Load()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(Config.config);
			xmlDocument.GetElementsByTagName("Version");
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("ShowGui");
			XmlNodeList xmlNodeLists = xmlDocument.GetElementsByTagName("SkipSplash");
			XmlNodeList elementsByTagName1 = xmlDocument.GetElementsByTagName("CursorBorder");
			XmlNodeList xmlNodeLists1 = xmlDocument.GetElementsByTagName("CursorBorderColor");
			XmlNodeList elementsByTagName2 = xmlDocument.GetElementsByTagName("Cursor");
			XmlNodeList xmlNodeLists2 = xmlDocument.GetElementsByTagName("CursorScale");
			try
			{
				Config.bShowGui = Convert.ToBoolean(elementsByTagName[0].InnerText);
			}
			catch
			{
				Config.bShowGui = true;
			}
			try
			{
				Config.bSkipSplash = Convert.ToBoolean(xmlNodeLists[0].InnerText);
			}
			catch
			{
				Config.bSkipSplash = false;
			}
			try
			{
				Config.iCursorType = Convert.ToByte(elementsByTagName2[0].InnerText);
			}
			catch
			{
				Config.iCursorType = 0;
			}
			try
			{
				Config.bCursorBorder = Convert.ToBoolean(elementsByTagName1[0].InnerText);
			}
			catch
			{
				Config.bCursorBorder = false;
			}
			try
			{
				Config.colCursorBorderColor = Config.ColorFromString(xmlNodeLists1[0].InnerText);
			}
			catch
			{
				Config.colCursorBorderColor = Color.Black;
			}
			try
			{
				Config.fCursorScale = Convert.ToSingle(xmlNodeLists2[0].InnerText, new CultureInfo("en-US"));
			}
			catch
			{
				Config.iCursorType = 0;
			}
		}

		public static void Save()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode xmlNodes = xmlDocument.CreateElement("Settings");
			xmlDocument.AppendChild(xmlNodes);
			XmlNode str = xmlDocument.CreateElement("Version");
			str.InnerText = "1";
			xmlNodes.AppendChild(str);
			str = xmlDocument.CreateElement("ShowGui");
			str.InnerText = Convert.ToString(Config.bShowGui);
			xmlNodes.AppendChild(str);
			str = xmlDocument.CreateElement("SkipSplash");
			str.InnerText = Convert.ToString(Config.bSkipSplash);
			xmlNodes.AppendChild(str);
			str = xmlDocument.CreateElement("Cursor");
			str.InnerText = Convert.ToString(Config.iCursorType);
			xmlNodes.AppendChild(str);
			str = xmlDocument.CreateElement("CursorBorder");
			str.InnerText = Convert.ToString(Config.bCursorBorder);
			xmlNodes.AppendChild(str);
			str = xmlDocument.CreateElement("CursorBorderColor");
			str.InnerText = Config.ColorToString(Config.colCursorBorderColor);
			xmlNodes.AppendChild(str);
			str = xmlDocument.CreateElement("CursorScale");
			str.InnerText = Convert.ToString(Config.fCursorScale, new CultureInfo("en-US"));
			xmlNodes.AppendChild(str);
			xmlDocument.Save(Config.config);
		}
	}
}