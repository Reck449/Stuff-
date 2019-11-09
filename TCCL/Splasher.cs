using System;
using System.Threading;
using System.Windows.Forms;

namespace TCCL
{
	public class Splasher
	{
		private static Splash MySplashForm;

		private static Thread MySplashThread;

		public static int Progress
		{
			get
			{
				if (Splasher.MySplashForm == null)
				{
					return 0;
				}
				return Splasher.MySplashForm.ProgressInfo;
			}
			set
			{
				if (Splasher.MySplashForm == null)
				{
					return;
				}
				Splasher.MySplashForm.ProgressInfo = value;
			}
		}

		public static string Status
		{
			get
			{
				if (Splasher.MySplashForm == null)
				{
					return "";
				}
				return Splasher.MySplashForm.StatusInfo;
			}
			set
			{
				if (Splasher.MySplashForm == null)
				{
					return;
				}
				Splasher.MySplashForm.StatusInfo = value;
			}
		}

		static Splasher()
		{
		}

		public Splasher()
		{
		}

		public static void Close()
		{
			if (Splasher.MySplashThread == null)
			{
				return;
			}
			if (Splasher.MySplashForm == null)
			{
				return;
			}
			try
			{
				Splasher.MySplashForm.Invoke(new MethodInvoker(Splasher.MySplashForm.Close));
			}
			catch (Exception exception)
			{
			}
			Splasher.MySplashThread = null;
			Splasher.MySplashForm = null;
		}

		public static void SetMinMaxValues(int min, int max)
		{
			if (Splasher.MySplashForm == null)
			{
				return;
			}
			Splasher.MySplashForm.ChangeProgressMinMaxValue(min, max);
		}

		public static void Show(Form parentForm)
		{
			if (Splasher.MySplashThread != null)
			{
				return;
			}
			Splasher.MySplashThread = new Thread(new ThreadStart(Splasher.ShowThread))
			{
				IsBackground = true
			};
			Splasher.MySplashThread.SetApartmentState(ApartmentState.MTA);
			Splasher.MySplashThread.Start();
		}

		private static void ShowThread()
		{
			Splasher.MySplashForm = new Splash();
			Application.Run(Splasher.MySplashForm);
		}
	}
}