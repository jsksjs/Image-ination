using System.Diagnostics;
using System.Windows.Input;

namespace Image_ination
{
	// Pins layer canvas left, center, right (within raisins)
	public partial class MainWindow
	{
		private void Left_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		/// <summary>
		/// Center image canvas on Y and hug left.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Left_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (ImageCanvas.Width * Scale.ScaleX > LayerCanvas.ActualWidth - LeftMenu.ActualWidth - RightMenu.ActualWidth - 50)
				return;
			if(ImageCanvas.Width < LayerCanvas.ActualWidth)
				Trans.X = ImageCanvas.Width * Scale.ScaleX/2;
			else
				Trans.X = (LayerCanvas.ActualWidth - ImageCanvas.Width) - ImageCanvas.Width * Scale.ScaleX / 2 + (ImageCanvas.Width * Scale.ScaleX);
			Trans.Y = LayerCanvas.ActualHeight / 2 - ImageCanvas.Height / 2;
		}

		private void Center_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		/// <summary>
		/// Center image canvas on both planes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Center_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Trans.X = LayerCanvas.ActualWidth / 2 - ImageCanvas.Width / 2;
			Trans.Y = LayerCanvas.ActualHeight / 2 - ImageCanvas.Height / 2;
		}

		private void Right_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		/// <summary>
		/// Center image canvas on Y and hug right.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Right_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (ImageCanvas.Width * Scale.ScaleX > LayerCanvas.ActualWidth - LeftMenu.ActualWidth - RightMenu.ActualWidth - 50)
				return;
			if (ImageCanvas.Width < LayerCanvas.ActualWidth)
				Trans.X = (LayerCanvas.ActualWidth - ImageCanvas.Width) - ImageCanvas.Width * Scale.ScaleX / 2;
			else
				Trans.X = ImageCanvas.Width * Scale.ScaleX / 2 - (ImageCanvas.Width * Scale.ScaleX);
			Trans.Y = LayerCanvas.ActualHeight / 2 - ImageCanvas.Height / 2;
		}
	}
}
