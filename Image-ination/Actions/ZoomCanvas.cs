using System.Windows.Input;

namespace Image_ination
{
	// Zooms canvas through keys and textbox.
	public partial class MainWindow
	{
		private void Zoom_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		/// <summary>
		/// Zoom to 100%.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Zoom_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Scale.ScaleX = Scale.ScaleY = 1;
			Zoom = (Scale.ScaleX * 100.00) + "%";
		}

		private void ZoomFit_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		/// <summary>
		/// Zoom to fit user's screen.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ZoomFit_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// AGAIN IT TOOK ME 2 MINUTES TO FIX THIS WHY DO I MAKE MISTAKES
			if ((LayerCanvas.ActualWidth - ImageCanvas.Width) / LayerCanvas.ActualWidth >= (LayerCanvas.ActualHeight - ImageCanvas.Height) / LayerCanvas.ActualHeight)
				Scale.ScaleX = Scale.ScaleY = (LayerCanvas.ActualHeight - Infobar.ActualHeight) / ImageCanvas.Height;
			else
				Scale.ScaleX = Scale.ScaleY = (LayerCanvas.ActualWidth - LeftDock.ActualWidth) / ImageCanvas.Width;

			Trans.X = LayerCanvas.ActualWidth / 2 - ImageCanvas.Width / 2;
			Trans.Y = LayerCanvas.ActualHeight / 2 - ImageCanvas.Height / 2;
			Zoom = (Scale.ScaleX * 100.00) + "%";
			BackgroundScale.ScaleX = BackgroundScale.ScaleY = (ImageCanvas.Width / 100) * (Scale.ScaleX);
		}

		private void ZoomEnter_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		/// <summary>
		/// Enter Zoom text
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ZoomEnter_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ZoomPercent.Focus();
			ZoomPercent.SelectAll();
		}
	}
}
