using Microsoft.Xna.Framework;
using System;
using System.Windows.Forms;

namespace TCCL
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Helper.StartUpPath = Application.StartupPath;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Log.Open();
			Helper.isLoading = true;
			string str = "";
			if ((int)args.Length > 0)
			{
				str = args[0];
			}
			using (TCCL tCCL = new TCCL(str))
			{
				Splasher.Show(Form.ActiveForm);
				Splasher.Status = "Please wait...";
				tCCL.Run();
			}
		}
	}
}