using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Image_ination
{
	public partial class MainWindow
	{
		// Marks where line starts
		private static Point startPaint;
		// Image data to edit
		private static WriteableBitmap currentBMP = BitmapFactory.New(1, 1);
		// Current line color
		private static Color paintColor = Color.FromArgb(255, 0, 0, 0);
		// Line color to byte[]
		private static byte[] colorBytes = { paintColor.A, paintColor.R, paintColor.G, paintColor.B };
		private static WriteableBitmap brush = null;
		private static int thick = 2;

		private void StartPaint(object sender, MouseButtonEventArgs e)
		{
			MouseMove -= Paint;
			if (!(bool)Brush.IsChecked
				|| (Keyboard.Modifiers & ModifierKeys.Control) > 0
				|| (Keyboard.Modifiers & ModifierKeys.Alt) > 0
				|| e.LeftButton != MouseButtonState.Pressed)
				return;
			if (selection.Visibility == Visibility.Visible)
			{
				currentBMP = ((Image)currentLayer.Children[0]).Source as WriteableBitmap;
				WriteableBitmap temp = BitmapFactory.New(currentBMP.PixelWidth, currentBMP.PixelHeight);
				startPaint = e.GetPosition(currentLayer);
				int halfThick = thick / 2;
				Point centeredStart = new Point(startPaint.X - halfThick, startPaint.Y - halfThick);
				Rect sourceRect = new Rect(new Size(brush.Width, brush.Height));
				temp.Blit(centeredStart, brush, sourceRect, Colors.White, WriteableBitmapExtensions.BlendMode.Alpha);
				currentBMP.Blit(selectArea, temp, selectArea, WriteableBitmapExtensions.BlendMode.Alpha);
			}
			else
			{
				currentBMP = ((Image)currentLayer.Children[0]).Source as WriteableBitmap;
				startPaint = e.GetPosition(currentLayer);
				int halfThick = thick / 2;
				Point centeredStart = new Point(startPaint.X - halfThick, startPaint.Y - halfThick);
				Rect sourceRect = new Rect(new Size(brush.Width, brush.Height));
				currentBMP.Blit(centeredStart, brush, sourceRect, Colors.White, WriteableBitmapExtensions.BlendMode.Alpha);
			}
			MouseMove += Paint;
		}

		private void Paint(object sender, MouseEventArgs e)
		{
			if (!(bool)Brush.IsChecked
				|| (Keyboard.Modifiers & ModifierKeys.Control) > 0
				|| (Keyboard.Modifiers & ModifierKeys.Alt) > 0
				|| e.LeftButton != MouseButtonState.Pressed)
				return;
			if (selection.Visibility == Visibility.Visible)
			{
				currentBMP = ((Image)currentLayer.Children[0]).Source as WriteableBitmap;
				WriteableBitmap temp = BitmapFactory.New(currentBMP.PixelWidth, currentBMP.PixelHeight);
				int halfThick = thick / 2;
				temp.DrawLinePenned((int)startPaint.X - halfThick, (int)startPaint.Y - halfThick,
					(int)e.GetPosition(currentLayer).X - halfThick, (int)e.GetPosition(currentLayer).Y - halfThick, brush);
				currentBMP.Blit(selectArea, temp, selectArea, WriteableBitmapExtensions.BlendMode.Alpha);
				startPaint = e.GetPosition(currentLayer);
			}
			else
			{
				int halfThick = thick / 2;
				currentBMP.DrawLinePenned((int)startPaint.X - halfThick, (int)startPaint.Y - halfThick,
					(int)e.GetPosition(currentLayer).X - halfThick, (int)e.GetPosition(currentLayer).Y - halfThick, brush);
				startPaint = e.GetPosition(currentLayer);
			}
		}

		private void StopPaint(object sender, MouseButtonEventArgs e)
		{
			MouseMove -= Paint;
		}

		private void Color_TextChanged(object sender, TextChangedEventArgs e)
		{
			string origText = ((TextBox)e.OriginalSource).Text;
			TextBox source = ((TextBox)e.OriginalSource);
			if (!OptionsWindow.IsNums(origText)
				|| origText == " "
				|| origText == ""
				|| origText.Length > 3)
			{
				if (origText.Contains("]") && origText != "]")
					source.Text = (byte.Parse(origText.Replace("]", "")) + 1).ToString();
				else if (origText.Contains("[") && origText != "[")
					source.Text = (byte.Parse(origText.Replace("[", "")) - 1).ToString();
				else
					source.Text = "255";
			}
			else if (int.Parse(origText) < 0)
				source.Text = "255";
			else if (int.Parse(origText) > 255)
				source.Text = "0";
			SetPen();
		}

		private void PaintOption_TextChanged(object sender, TextChangedEventArgs e)
		{
			string origText = ((TextBox)e.OriginalSource).Text;
			if (!OptionsWindow.IsNums(origText)
				|| origText == " "
				|| origText == "")
			{
				if (origText.Contains("]") && origText != "]")
					((TextBox)e.OriginalSource).Text = (int.Parse(origText.Replace("]", "")) + 1).ToString();
				else if (origText.Contains("[") && origText != "[")
					((TextBox)e.OriginalSource).Text = (int.Parse(origText.Replace("[", "")) - 1).ToString();
				else
					((TextBox)e.OriginalSource).Text = "0";
			}
			else if (int.Parse(origText) > int.MaxValue)
				((TextBox)e.OriginalSource).Text = "1";
			if (((TextBox)e.OriginalSource).Name == "Thickness")
			{
				if (origText == "1")
				{
					CircularBrush.IsEnabled = false;
					TriangularBrush.IsEnabled = false;
					if (CircularBrush.IsSelected || TriangularBrush.IsSelected)
						SquareBrush.IsSelected = true;
				}
				else
				{
					CircularBrush.IsEnabled = true;
					TriangularBrush.IsEnabled = true;
                    int num;
					int.TryParse(origText, out num);
					if (num > 999 || num.ToString().Length > 3)
						((TextBox)e.OriginalSource).Text = "999";
				}
			}
			SetPen();
		}

		private void SetPen()
		{
			if (Red != null && Blue != null && Green != null && Alpha != null && ColorPreview != null
				&& BrushPreview != null && Thickness != null && BrushStyle != null)
			{
				paintColor = Color.FromArgb(byte.Parse(Alpha.Text),
								   byte.Parse(Red.Text),
								   byte.Parse(Green.Text),
								   byte.Parse(Blue.Text));
				colorBytes = new byte[] { paintColor.A, paintColor.R, paintColor.G, paintColor.B };
				ColorPreview.Background = new SolidColorBrush(paintColor);
				thick = int.Parse(Thickness.Text);
				WriteableBitmap preview = BitmapFactory.New(180, 180);
				brush = BitmapFactory.New(thick, thick);
				switch (((ComboBoxItem)BrushStyle.SelectedItem).Content.ToString())
				{
					case ("Square"):
						brush = new WriteableBitmap(new BitmapImage(new Uri("Resources/sq.png", UriKind.Relative)) as BitmapSource);
						break;
					case ("Circular"):
						brush = new WriteableBitmap(new BitmapImage(new Uri("Resources/circ.png", UriKind.Relative)) as BitmapSource);
						break;
					case ("Triangular"):
						brush = new WriteableBitmap(new BitmapImage(new Uri("Resources/tri.png", UriKind.Relative)) as BitmapSource);
						break;
				}
				brush = brush.Resize(thick, thick, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
				brush.ForEach((x, y, color) => Color.FromArgb((color.A == 0) ? color.A : paintColor.A, paintColor.R, paintColor.G, paintColor.B));
				if (thick <= 180)
				{
					int yPos = 90 - thick / 2;
					preview.DrawLinePenned(0, yPos, 180, yPos, brush);
					BrushPreview.Source = preview;
				}
				else if(BrushPreview.Source != preview)
				{
					preview.DrawLinePenned(0, 0, 180, 0, brush);
					BrushPreview.Source = preview;
				}
			}
		}

		private void PaintOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SetPen();
		}
	}
}
