using System.Collections.Generic;
using System.Windows.Input;

namespace Image_ination.Actions
{
	/// <summary>
	/// Commands called by hotkeys
	/// </summary>
	public class Commands
	{
		public static RoutedCommand ZoomCmd = new RoutedCommand("Zoom Default", typeof(MainWindow));
		public static RoutedCommand ZoomFitCmd = new RoutedCommand("Zoom to Fit", typeof(MainWindow));
		public static RoutedCommand ZoomEnterCmd = new RoutedCommand("Edit Zoom Percent", typeof(MainWindow));

		public static RoutedCommand LeftCmd = new RoutedCommand("Pin Canvas Left", typeof(MainWindow));
		public static RoutedCommand CenterCmd = new RoutedCommand("Pin Canvas Center", typeof(MainWindow));
		public static RoutedCommand RightCmd = new RoutedCommand("Pin Canvas Right", typeof(MainWindow));

		public static RoutedCommand NewLayerCmd = new RoutedCommand("New Layer", typeof(MainWindow));

		public static RoutedCommand NewImageCmd = new RoutedCommand("New Image", typeof(MainWindow));
		public static RoutedCommand OpenImageCmd = new RoutedCommand("Open Image", typeof(MainWindow));

		public static RoutedCommand ResizeImageCmd = new RoutedCommand("Resize Image", typeof(MainWindow));
		public static RoutedCommand ResizeCanvasCmd = new RoutedCommand("Resize Canvas", typeof(MainWindow));

		public static RoutedCommand SaveImageCmd = new RoutedCommand("Save", typeof(MainWindow));
		public static RoutedCommand SaveImageAsCmd = new RoutedCommand("Save As", typeof(MainWindow));

		public static RoutedCommand HotkeysCmd = new RoutedCommand("Hotkeys", typeof(MainWindow));

        public static RoutedCommand PaintToolCmd = new RoutedCommand("Paint Tool", typeof(MainWindow));

        public static RoutedCommand TranslateToolCmd = new RoutedCommand("Translate Tool", typeof(MainWindow));
    }
}
