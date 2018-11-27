using System.Windows;

namespace Image_ination
{
	/// <summary>
	/// Bound zoom string of ZoomPercent.
	/// </summary>
	public partial class MainWindow
	{

		public static readonly DependencyProperty ZoomProperty =
			DependencyProperty.Register("Zoom", typeof(string), typeof(MainWindow), new PropertyMetadata("100%"));

		public string Zoom
		{
			get { return (string)GetValue(ZoomProperty); }
			set { SetValue(ZoomProperty, value); }
		}
	}
}
