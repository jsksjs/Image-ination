using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Image_ination
{
	/// <summary>
	/// Canvas manipulation events.
	/// </summary>
	public partial class MainWindow
	{
		/// <summary>
		/// Move Image Canvas based on mouse (ctrl + LMB down).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LayerCanvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && (Keyboard.Modifiers & ModifierKeys.Control) > 0)
			{
				Cursor = Cursors.Hand;
				xPos = e.GetPosition(this).X;
				yPos = e.GetPosition(this).Y;
				xCanvas = Trans.X;
				yCanvas = Trans.Y;
				LayerCanvas.MouseMove += Handle_Pan;
			}
		}

		/// <summary>
		/// Handle panning event once it begins.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Handle_Pan(object sender, MouseEventArgs e)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Control) > 0 && e.LeftButton == MouseButtonState.Pressed)
			{
				Trans.X = xCanvas + e.GetPosition(this).X - xPos;
				Trans.Y = yCanvas + e.GetPosition(this).Y - yPos;
			}
			else
			{
				LayerCanvas.MouseMove -= Handle_Pan;
				Cursor = Cursors.Arrow;
			}
		}

		/// <summary>
		/// Zoom Image Canvas based on mouse wheel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LayerCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.LeftButton == MouseButtonState.Released)
			{
				double d = 1;
				if (e.Delta < 0)
					d = -1;
				if ((Keyboard.Modifiers & ModifierKeys.Alt) > 0)
					Scale.ScaleX = Scale.ScaleY = Math.Max(0.05, Scale.ScaleX + (d * 2.5));
				else if((Keyboard.Modifiers & ModifierKeys.Shift) > 0)
					Scale.ScaleX = Scale.ScaleY = Math.Max(0.05, Scale.ScaleX + (d * 0.05));
				else
					Scale.ScaleX = Scale.ScaleY = Math.Max(0.05, Scale.ScaleX + (d * 0.25));
				Zoom = (Scale.ScaleX * 100) + "%";
			}
		}
	}
}
