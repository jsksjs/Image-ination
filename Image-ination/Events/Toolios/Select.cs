using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Image_ination
{
	public partial class MainWindow
	{
		private static Point startSelect;
		private static Rectangle selection = new Rectangle
		{
			Visibility = Visibility.Collapsed,
			Width = 0,
			Height = 0,
			StrokeThickness = 1,
			Stroke = Brushes.White,
			StrokeDashArray = new DoubleCollection { 1, 1 },
			UseLayoutRounding = true,
			SnapsToDevicePixels = true,
		};
		private static Rect selectArea = new Rect();

		private void LayerCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (LayerCanvas.IsMouseOver 
				&& (Keyboard.Modifiers & ModifierKeys.Alt) > 0
				&& e.LeftButton == MouseButtonState.Pressed)
			{
				ImageCanvas.Children.Remove(selection);
				Point pos = e.GetPosition(ImageCanvas);
				if (pos.X < 0)
					pos.X = 0;
				else if (pos.X > ImageCanvas.Width)
					pos.X = ImageCanvas.Width;
				if (pos.Y < 0)
					pos.Y = 0;
				else if (pos.Y > ImageCanvas.Height)
					pos.Y = ImageCanvas.Height;

				startSelect = pos;

				int maxed = Math.Max(ImageCanvas.Width.ToString().Length, ImageCanvas.Height.ToString().Length);
				selection.StrokeThickness = maxed;
				selection.StrokeDashArray = new DoubleCollection { maxed, maxed };
				DoubleAnimation dashMove = new DoubleAnimation
				{
					Duration = new Duration(new TimeSpan(0, 0, 10)),
					RepeatBehavior = RepeatBehavior.Forever,
					By = maxed + 250
				};
				selection.BeginAnimation(Rectangle.StrokeDashOffsetProperty, dashMove);
                selection.SetValue(Panel.ZIndexProperty, int.MaxValue);
				ImageCanvas.Children.Add(selection);
				MouseMove += LayerCanvas_MouseMove;
			}
		}

		private void LayerCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			ResetSelection();
		}

		private void LayerCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released || selection == null || (Keyboard.Modifiers & ModifierKeys.Alt) <= 0)
				return;

			Point pos = e.GetPosition(ImageCanvas);
			if (pos.X < 0)
				pos.X = 0;
			else if (pos.X > ImageCanvas.Width)
				pos.X = ImageCanvas.Width;
			if (pos.Y < 0)
				pos.Y = 0;
			else if (pos.Y > ImageCanvas.Height)
				pos.Y = ImageCanvas.Height;

			double x = Math.Min(pos.X, startSelect.X);
			double y = Math.Min(pos.Y, startSelect.Y);

			double w = Math.Max(pos.X, startSelect.X) - x;
			double h = Math.Max(pos.Y, startSelect.Y) - y;

			selection.Width = w;
			selection.Height = h;

			Canvas.SetLeft(selection, x);
			Canvas.SetTop(selection, y);
			selection.Visibility = Visibility.Visible;
		}

		public void ResetSelection()
		{
            selection.Visibility = Visibility.Collapsed;
		}

		private void LayerCanvas_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MouseMove -= LayerCanvas_MouseMove;
			if ((bool)Brush.IsChecked)
				StopPaint(sender, e);
			else if ((bool)Translate.IsChecked)
				StopTranslate(sender, e);
			if (selection.Visibility == Visibility.Visible)
			{
				selectArea.Location = new Point(Canvas.GetLeft(selection), Canvas.GetTop(selection));
				selectArea.Width = Math.Max(0, selection.RenderSize.Width);
				selectArea.Height = Math.Max(0, selection.RenderSize.Height);
			}
		}

	}
}
