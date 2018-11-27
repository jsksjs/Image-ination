using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Image_ination
{
	/// <summary>
	/// Toolbar events.
	/// </summary>
	public partial class MainWindow
	{
		private static bool open = false;
		public bool Open
		{
			get { return open; }
			set { open = value; }
		}

		private static string filepath = null;
		public string Filepath
		{
			get { return filepath; }
			set { filepath = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		private OptionsWindow CreateOptions(string title)
		{
			OptionsWindow options = null;
			if (!OptionsWindow.IsAlive)
			{
				this.IsEnabled = false;
				if (WindowState == WindowState.Normal)
					options = new OptionsWindow(
						title,
						Left + ActualWidth / 2,
						Top + ActualHeight / 2,
						UIColor,
						ContentColor);
				else
					options = new OptionsWindow(
						title,
						ActualWidth / 2 + Helper.ScreenFix.WidthScreensRight(this),
						ActualHeight / 2 + Helper.ScreenFix.HeightScreensAbove(this),
						UIColor,
						ContentColor);
				return options;
			}
			return null;
		}

		/// <summary>
		/// Toggles tooltips.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolTips_Checked(object sender, RoutedEventArgs e)
		{
			SetValue(ToolTipEnabled.ToolTipEnabledProperty, !(Boolean)GetValue(ToolTipEnabled.ToolTipEnabledProperty));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizeCanvas_Click(object sender, RoutedEventArgs e)
		{
			OptionsWindow options = CreateOptions("Canvas Resize");
			if (options.Equals(null))
				return;
			options.Lists.SelectedItem = options.CanvasResizeTab;
			options.CanvasInputW.Text = options.CurrentCanvasWidth.Text = (int)ImageCanvas.Width + "";
			options.CanvasInputH.Text = options.CurrentCanvasHeight.Text = (int)ImageCanvas.Height + "";
			FocusManager.SetFocusedElement(FocusManager.GetFocusScope(options), options.CanvasInputW as IInputElement);
			OptionsWindow.CurrentOpen = "CanvasResizeTab";
			options.Show();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizeImage_Click(object sender, RoutedEventArgs e)
		{
			OptionsWindow options = CreateOptions("Image Resize");
			if (options.Equals(null))
				return;
			options.Lists.SelectedItem = options.ImageResizeTab;
			options.ImageInputW.Text = options.CurrentImageWidth.Text = (int)(((Image)currentLayer.Children[0]).Source as WriteableBitmap).Width + "";
			options.ImageInputH.Text = options.CurrentImageHeight.Text = (int)(((Image)currentLayer.Children[0]).Source as WriteableBitmap).Height + "";
			FocusManager.SetFocusedElement(FocusManager.GetFocusScope(options), options.ImageInputW as IInputElement);
			OptionsWindow.CurrentOpen = "ImageResizeTab";
			options.Show();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void About_Click(object sender, RoutedEventArgs e)
		{
			OptionsWindow options = CreateOptions("About");
			if (options.Equals(null))
				return;
			options.Lists.SelectedItem = options.AboutTab;
			options.AboutText.Text = "ImEd is the standard image editor." +
									"\n\n\nThis program comes with some\n\ntools and a slick, user-friendly\n\n interface that was made after 1999." +
									"\n\n\nIt's pretty okay 👌";
			options.AcceptLabel.Visibility = Visibility.Collapsed;
			options.ExitLabel.ToolTip = "Close";
			OptionsWindow.CurrentOpen = "AboutTab";
			options.Show();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Hotkeys_Click(object sender, RoutedEventArgs e)
		{
			OptionsWindow options = CreateOptions("Hotkeys");
			if (options.Equals(null))
				return;
			options.Lists.SelectedItem = options.HotkeysTab;
			OptionsWindow.CurrentOpen = "HotkeysTab";
			options.Show();
		}

		private void Open_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog fileD = new OpenFileDialog();
			fileD.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			fileD.Filter = "Images | *.png; *.bmp; *.jpg; *.jpeg; *.tiff";
			if (fileD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				NewCanvas(fileD.FileName);
				if (Actions.Commands.ZoomFitCmd.CanExecute(null, null))
					Actions.Commands.ZoomFitCmd.Execute(null, null);
				ImageName.Text = fileD.SafeFileName;
				ImageName.Visibility = Visibility.Visible;
				ImageSize.Visibility = Visibility.Visible;
				MousePos.Visibility = Visibility.Visible;
				ResetSelection();
				ToolbarSave.IsEnabled = true;
				ToolbarSaveAs.IsEnabled = true;
				ToolbarResizeCanvas.IsEnabled = true;
				ToolbarResizeImage.IsEnabled = true;
				Filepath = fileD.FileName;
				open = true;
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (ImageName.Text == "")
				return;
			else if (Filepath == null)
			{
				SaveAs_Click(null, null);
				return;
			}
			if (open)
			{
				ResetSelection();
				WriteableBitmap temp = BitmapFactory.New((int)ImageCanvas.Width, (int)ImageCanvas.Height);
				Rect size = new Rect(new Size(temp.PixelWidth, temp.PixelHeight));
				BitmapEncoder save;
				string ext = Path.GetExtension(ImageName.Text);
				switch (ext)
				{
					case (".png"):
						save = new PngBitmapEncoder();
						break;
					case (".tiff"):
						save = new TiffBitmapEncoder();
						break;
					case (".jpg"):
						temp.Clear(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
						save = new JpegBitmapEncoder();
						break;
					case (".jpeg"):
						temp.Clear(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
						save = new JpegBitmapEncoder();
						break;
					default:
						return;
				}
				foreach (FrameworkElement g in ImageCanvas.Children)
				{
					if (g is Grid && ((Grid)g).Children[0] is Image)
						temp.Blit(size, (((Grid)g).Children[0] as Image).Source as WriteableBitmap, size, WriteableBitmapExtensions.BlendMode.Alpha);
				}
				using (FileStream s = File.Create(filepath))
				{
					save.Frames.Add(BitmapFrame.Create(temp));
					save.Save(s);
				}
			}		
		}

		private void SaveAs_Click(object sender, RoutedEventArgs e)
		{
			if (ImageName.Text == "")
				return;
			SaveFileDialog fileD = new SaveFileDialog();
			string name = ImageName.Text;
			if (name.Contains("."))
			{
				fileD.FileName = Path.GetFileName(ImageName.Text);
				fileD.DefaultExt = Path.GetExtension(ImageName.Text);
			}
			else
			{
				fileD.FileName = name;
				fileD.DefaultExt = ".png";
			}
			fileD.Filter = "png|*.png|tiff|*.tiff|jpg|*.jpg|jpeg|*.jpeg|All Files|*";
			fileD.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			fileD.AddExtension = true;

			if (fileD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				WriteableBitmap temp = BitmapFactory.New((int)ImageCanvas.Width, (int)ImageCanvas.Height);
				Rect size = new Rect(new Size(temp.PixelWidth, temp.PixelHeight));
				BitmapEncoder save;
				string ext = Path.GetExtension(fileD.FileName);
				switch (ext)
				{
					case (".png"):
						save = new PngBitmapEncoder();
						break;
					case (".tiff"):
						save = new TiffBitmapEncoder();
						break;
					case (".jpg"):
						temp.Clear(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
						save = new JpegBitmapEncoder();
						break;
					case (".jpeg"):
						temp.Clear(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
						save = new JpegBitmapEncoder();
						break;
					default:
						return;
				}
				foreach (FrameworkElement g in ImageCanvas.Children)
				{
					if (g is Grid && ((Grid)g).Children[0] is Image)
						temp.Blit(size, (((Grid)g).Children[0] as Image).Source as WriteableBitmap, size, WriteableBitmapExtensions.BlendMode.Alpha);
				}
				using (FileStream s = File.Create(fileD.FileName))
				{
					save.Frames.Add(BitmapFrame.Create(temp));
					save.Save(s);
					ImageName.Text = Path.GetFileName(fileD.FileName);
					ImageName.Visibility = Visibility.Visible;
					Filepath = fileD.FileName;
				}
			}
		}

		private void New_Click(object sender, RoutedEventArgs e)
		{
			OptionsWindow options = CreateOptions("New Image");
			if (options.Equals(null))
				return;
			options.Lists.SelectedItem = options.NewImageTab;
			if (ImageCanvas.Width == 0)
				options.NewImageWidth.Text = "300";
			else
				options.NewImageWidth.Text = ImageCanvas.Width.ToString();
			if (ImageCanvas.Height == 0)
				options.NewImageHeight.Text = "300";
			else
				options.NewImageHeight.Text = ImageCanvas.Height.ToString();
			OptionsWindow.CurrentOpen = "NewImageTab";
			FocusManager.SetFocusedElement(FocusManager.GetFocusScope(options), options.NewImageWidth as IInputElement);
			options.Show();
		}
	}
}