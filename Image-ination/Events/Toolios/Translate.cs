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
		private static WriteableBitmap currentTranslateBMP = BitmapFactory.New(1, 1);
		private static Point startTranslate;
		private static Grid transLayer;

		private void StartTranslate(object sender, MouseButtonEventArgs e)
		{
			if (!(bool)Translate.IsChecked
				|| (Keyboard.Modifiers & ModifierKeys.Control) > 0
				|| (Keyboard.Modifiers & ModifierKeys.Alt) > 0
				|| e.LeftButton != MouseButtonState.Pressed)
				return;
			if(selection.Visibility == Visibility.Visible)
			{
				currentTranslateBMP = ((Image)currentLayer.Children[0]).Source as WriteableBitmap;
				WriteableBitmap temp = BitmapFactory.New(currentTranslateBMP.PixelWidth, currentTranslateBMP.PixelHeight);
				temp.Blit(selectArea, currentTranslateBMP, selectArea, WriteableBitmapExtensions.BlendMode.Alpha);
				WriteableBitmap clear = BitmapFactory.New(currentTranslateBMP.PixelWidth, currentTranslateBMP.PixelHeight);
				clear.Clear();
				currentTranslateBMP.Blit(selectArea, clear, selectArea, WriteableBitmapExtensions.BlendMode.None);
				Grid grid = new Grid
				{
					Width = ImageCanvas.Width,
					Height = ImageCanvas.Height,
				};
				grid.SetValue(Panel.ZIndexProperty, currentLayer.GetValue(Panel.ZIndexProperty));

				Image image = new Image
				{
					Source = temp,
				};

				grid.Children.Add(image);
				ImageCanvas.Children.Add(grid);

				transLayer = grid;

				startTranslate = e.GetPosition(ImageCanvas);
                Canvas.SetLeft(transLayer, 0);
				Canvas.SetTop(transLayer, 0);
				MouseMove += Move;
			}
			else
			{
				startTranslate = e.GetPosition(ImageCanvas);
                Canvas.SetLeft(currentLayer, 0);
                Canvas.SetTop(currentLayer, 0);
                MouseMove += Move;
			}
		}

		private void Move(object sender, MouseEventArgs e)
		{
			if (!(bool)Translate.IsChecked
				|| (Keyboard.Modifiers & ModifierKeys.Control) > 0
				|| (Keyboard.Modifiers & ModifierKeys.Alt) > 0
				|| e.LeftButton != MouseButtonState.Pressed)
				return;
			if (selection.Visibility == Visibility.Visible)
			{
				Vector offset = Point.Subtract(e.GetPosition(ImageCanvas), startTranslate);
				Canvas.SetLeft(transLayer, offset.X);
				Canvas.SetTop(transLayer, offset.Y);
			}
			else
			{
				Vector offset = Point.Subtract(e.GetPosition(ImageCanvas), startTranslate);
				Canvas.SetLeft(currentLayer, offset.X);
				Canvas.SetTop(currentLayer, offset.Y);
			}
		}

		private void StopTranslate(object sender, MouseButtonEventArgs e)
		{
			if (!(bool)Translate.IsChecked
			|| (Keyboard.Modifiers & ModifierKeys.Control) > 0
			|| (Keyboard.Modifiers & ModifierKeys.Alt) > 0
            || e.RightButton == MouseButtonState.Pressed)
				return;
			if (selection.Visibility == Visibility.Visible)
			{
				Vector offset = Point.Subtract(Mouse.GetPosition(ImageCanvas), startTranslate);
				currentTranslateBMP = ((Image)currentLayer.Children[0]).Source as WriteableBitmap;
				WriteableBitmap merge = ((Image)transLayer.Children[0]).Source as WriteableBitmap;
				currentTranslateBMP.Blit(new Point(Canvas.GetLeft(transLayer), Canvas.GetTop(transLayer)), merge, new Rect(new Size(merge.PixelWidth, merge.PixelHeight)), Colors.White, WriteableBitmapExtensions.BlendMode.Alpha);
				((Image)currentLayer.Children[0]).Source = currentTranslateBMP;
				Canvas.SetLeft(currentLayer, 0);
				Canvas.SetTop(currentLayer, 0);
				ImageCanvas.Children.Remove(transLayer);
				MouseMove -= Move;
			}
			else
			{
				Vector offset = Point.Subtract(Mouse.GetPosition(ImageCanvas), startTranslate);
				currentTranslateBMP = ((Image)currentLayer.Children[0]).Source as WriteableBitmap;
				WriteableBitmap temp = BitmapFactory.New(currentTranslateBMP.PixelWidth, currentTranslateBMP.PixelHeight);
				temp.Blit(new Point((int)Canvas.GetLeft(currentLayer), (int)Canvas.GetTop(currentLayer)), currentTranslateBMP, new Rect(new Size(currentTranslateBMP.PixelWidth, currentTranslateBMP.PixelHeight)), Colors.White, WriteableBitmapExtensions.BlendMode.Alpha);
				((Image)currentLayer.Children[0]).Source = temp;
				Canvas.SetLeft(currentLayer, 0);
				Canvas.SetTop(currentLayer, 0);
				MouseMove -= Move;
            }
		}
	}
}
