using System.Windows.Input;


namespace Image_ination
{
	// Tools are sent events from here
	public partial class MainWindow
	{
		private void ImageCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if ((bool)Brush.IsChecked)
				StartPaint(sender, e);
			else if ((bool)Translate.IsChecked)
				StartTranslate(sender, e);
		}

		private void ImageCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if ((bool)Brush.IsChecked)
				StopPaint(sender, e);
			else if ((bool)Translate.IsChecked)
				StopTranslate(sender, e);
		}
	}
}
