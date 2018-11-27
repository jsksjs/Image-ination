using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Image_ination.Helper
{
	/// <summary>
	/// H A T E
	/// A T E H
	/// T E H A
	/// E H A T
	/// </summary>
	public class ScreenFix
	{
		public static IEnumerable<ScreenFix> AllScreens()
		{
			foreach (Screen screen in Screen.AllScreens)
			{
				yield return new ScreenFix(screen);
			}
		}

		public static ScreenFix GetScreenFrom(Window window)
		{
			WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
			Screen screen = Screen.FromHandle(windowInteropHelper.Handle);
			ScreenFix wpfScreen = new ScreenFix(screen);
			return wpfScreen;
		}

		public static ScreenFix GetScreenFrom(System.Windows.Point point)
		{
			int x = (int)Math.Round(point.X);
			int y = (int)Math.Round(point.Y);

			System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
			Screen screen = Screen.FromPoint(drawingPoint);
			ScreenFix wpfScreen = new ScreenFix(screen);

			return wpfScreen;
		}

		public static ScreenFix Primary
		{
			get { return new ScreenFix(Screen.PrimaryScreen); }
		}

		private readonly Screen screen;

		internal ScreenFix(Screen screen)
		{
			this.screen = screen;
		}

		public Rect DeviceBounds
		{
			get { return this.GetRect(this.screen.Bounds); }
		}

		public Rect WorkingArea
		{
			get { return this.GetRect(this.screen.WorkingArea); }
		}

		private Rect GetRect(Rectangle value)
		{
			// should x, y, width, height be device-independent-pixels ??
			return new Rect
			{
				X = value.X,
				Y = value.Y,
				Width = value.Width,
				Height = value.Height
			};
		}

		public bool IsPrimary
		{
			get { return this.screen.Primary; }
		}

		public string DeviceName
		{
			get { return this.screen.DeviceName; }
		}

		public static double HeightScreensAbove(Window curr)
		{
			string thisWindow = ScreenFix.GetScreenFrom(curr).DeviceName;
			double screens = 0;
			foreach (ScreenFix s in ScreenFix.AllScreens())
			{
				if (ScreenFix.Primary.DeviceName == s.DeviceName)
					screens += ScreenFix.GetScreenFrom(App.Current.MainWindow).DeviceBounds.Top;
				else
					break;
			}
			return screens;
		}

		public static double WidthScreensLeft(Window curr)
		{
			string thisWindow = ScreenFix.GetScreenFrom(curr).DeviceName;
			double screens = 0;
			foreach (ScreenFix s in ScreenFix.AllScreens())
			{
				if (ScreenFix.Primary.DeviceName == s.DeviceName)
					screens -= ScreenFix.GetScreenFrom(App.Current.MainWindow).DeviceBounds.Left;
				else
					break;
			}
			return screens;
		}

		public static double WidthScreensRight(Window curr)
		{
			string thisWindow = ScreenFix.GetScreenFrom(curr).DeviceName;
			double screens = 0;
			foreach (Helper.ScreenFix s in Helper.ScreenFix.AllScreens())
			{
				if (ScreenFix.Primary.DeviceName == s.DeviceName)
					screens += ScreenFix.GetScreenFrom(App.Current.MainWindow).DeviceBounds.Left;
				else
					break;
			}
			return screens;
		}
	}
}
