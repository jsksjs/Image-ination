using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

/// <summary>
/// Window init.
/// </summary>
/// TODO: Make customizable key bindings.
namespace Image_ination
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// For element movement tied to mouse
		private int delta = 10;
		private double xPos;
		private double yPos;
		private double xCanvas;
		private double yCanvas;
		// Color mode
		private bool dark = true;
		public Grid currentLayer;

		/// <summary>
		/// Init window and set max window width, max window height, and create padding (for maximized window) based on resize border.
		/// </summary>
		public MainWindow()
		{
			SourceInitialized += (s, e) =>
			{
				WindowCompositionTarget = PresentationSource.FromVisual(this).CompositionTarget;
				HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(WindowProc);
			};
			InitializeComponent();
			ReadWriteHotKeys(false);
		}

		/********\ The following had to be done because WPF is broken if you want to have custom window styling.\********/
		/**************************************************************************************************************/
		/*************************************************************************************************************/

		CompositionTarget WindowCompositionTarget { get; set; }
		double CachedMinWidth { get; set; }
		double CachedMinHeight { get; set; }
		POINT CachedMinTrackSize { get; set; }

		IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch (msg)
			{
				case 0x0024:
					MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
					IntPtr monitor = MonitorFromWindow(hwnd, 0x00000002 /*MONITOR_DEFAULTTONEAREST*/);
					if (monitor != IntPtr.Zero)
					{
						MONITORINFO monitorInfo = new MONITORINFO { };
						GetMonitorInfo(monitor, monitorInfo);
						RECT rcWorkArea = monitorInfo.rcWork;
						RECT rcMonitorArea = monitorInfo.rcMonitor;
						mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
						mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
						mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
						mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
						if (!CachedMinTrackSize.Equals(mmi.ptMinTrackSize) || CachedMinHeight != MinHeight && CachedMinWidth != MinWidth)
						{
							mmi.ptMinTrackSize.x = (int)((CachedMinWidth = MinWidth) * WindowCompositionTarget.TransformToDevice.M11);
							mmi.ptMinTrackSize.y = (int)((CachedMinHeight = MinHeight) * WindowCompositionTarget.TransformToDevice.M22);
							CachedMinTrackSize = mmi.ptMinTrackSize;
						}
					}
					Marshal.StructureToPtr(mmi, lParam, true);
					handled = true;
					break;
			}
			return IntPtr.Zero;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct MINMAXINFO
		{
			public POINT ptReserved;
			public POINT ptMaxSize;
			public POINT ptMaxPosition;
			public POINT ptMinTrackSize;
			public POINT ptMaxTrackSize;
		};
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MONITORINFO
		{
			public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
			public RECT rcMonitor = new RECT { };
			public RECT rcWork = new RECT { };
			public int dwFlags = 0;
		}
		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}
		[DllImport("user32")]
		internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);
		[DllImport("User32")]
		internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
	}
}
