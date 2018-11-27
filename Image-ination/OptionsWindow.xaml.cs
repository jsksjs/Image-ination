using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.Imaging;

/// <summary>
/// Will be dynamically filled with C O N T E N T when C R E A T E D.
/// </summary>
namespace Image_ination
{
	/// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>
	public partial class OptionsWindow
	{
		private static bool isAlive = false;
		private double left = 0;
		private double top = 0;
		public static HashSet<string> modKeyPairs = new HashSet<string>();

		/// <summary>
		/// GETSET RADIO
		/// </summary>
		public static bool IsAlive
		{
			get { return isAlive; }
			set { isAlive = value; }
		}


		/// <summary>
		/// Set startup position and fill hotkeys if needed.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="top">Startup location in window, y</param>
		/// <param name="left">Startup location in window, x</param>
		/// <param name="back"></param>
		/// <param name="content"></param>
		public OptionsWindow(string title, double left, double top, Brush back, Brush content)
		{
			((MainWindow)Application.Current.MainWindow).IsEnabled = false;
			SetValue(ToolTipEnabled.ToolTipEnabledProperty, ToolTipEnabled.GetIsToolTipEnabled(((MainWindow)Application.Current.MainWindow)));
			InitializeComponent();
			TitleText.Text = title;
			Content.Background = content;
			OptionsContent.Background = back;
			IsAlive = true;
			this.top = top;
			this.left = left;
			switch (title)
			{
				case ("Hotkeys"):
					Regex sWhitespace = new Regex(@"\s+");
					KeyConverter kc = new KeyConverter();
					ModifierKeysConverter mc = new ModifierKeysConverter();
					modKeyPairs.Clear();
					foreach (InputBinding i in ((MainWindow)Application.Current.MainWindow).InputBindings)
					{
						DockPanel dock = new DockPanel
						{
							Height = 35,
							Margin = new Thickness(0, 0, 0, 10)
						};
						Label cmdName = new Label
						{
							Style = (System.Windows.Style)this.FindResource("HotkeyLabelName"),
							Content = ((RoutedCommand)i.Command).Name.ToString()
						};
						string[] mods = ((KeyBinding)i).Modifiers.ToString().Split(',');
						string txt = "";

						for (int j = 0; j < mods.Length; j++)
						{
							if (mods[j] == "None")
							{
								txt = ModifierKeys.None.ToString();
								break;
							}
							else if (j == 1)
								txt += ", " + mc.ConvertFromString(mods[j]).ToString();
							else
								txt += mc.ConvertFromString(mods[j]).ToString();
						}
						TextBox key1 = new TextBox
						{
							Style = (System.Windows.Style)this.FindResource("HotkeyModInput"),
							Text = txt,
							Name = sWhitespace.Replace(((RoutedCommand)i.Command).Name.ToString(), "") + "Mod",
							TabIndex = HotkeysStack.Children.Count + 1
						};
						Label plus = new Label
						{
							Style = (System.Windows.Style)this.FindResource("HotkeyLabelPlus")
						};
						TextBox key2 = new TextBox
						{
							Style = (System.Windows.Style)this.FindResource("HotkeyInput"),
							Text = kc.ConvertToString(((KeyBinding)i).Key),
							Name = sWhitespace.Replace(((RoutedCommand)i.Command).Name.ToString(), "") + "Key",
							TabIndex = key1.TabIndex + 1
						};
						key1.SetValue(PartnerElement.PartnerElementProperty, key2);
						key2.SetValue(PartnerElement.PartnerElementProperty, key1);
						RegisterName(key1.Name, key1);
						RegisterName(key2.Name, key2);
						dock.Children.Add(key2);
						dock.Children.Add(plus);
						dock.Children.Add(key1);
						dock.Children.Add(cmdName);
						HotkeysStack.Children.Add(dock);
						modKeyPairs.Add(((KeyBinding)i).Modifiers.ToString() + ((KeyBinding)i).Key.ToString());
					}
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Options_Loaded(object sender, RoutedEventArgs e)
		{
			Left = left - ActualWidth / 2;
			Top = top - ActualHeight / 2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Options_Closed(object sender, System.EventArgs e)
		{
			try
			{
				((MainWindow)Application.Current.MainWindow).IsEnabled = true;
				IsAlive = false;
				((MainWindow)Application.Current.MainWindow).Content.Focus();
			}
			catch (Exception) { };
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AcceptButton_Click(object sender, RoutedEventArgs e)
		{
			bool success = true;
			switch (TitleText.Text)
			{
				case ("Canvas Resize"):
					if (int.Parse(CanvasInputW.Text) < 1 || int.Parse(CanvasInputH.Text) > int.MaxValue
						|| int.Parse(CanvasInputW.Text) > int.MaxValue || int.Parse(CanvasInputH.Text) < 1)
					{
						AcceptButton.ToolTip = "Please enter valid input (integers > 0 and < 2.14748E9)";
						success = false;
						break;
					}
					else
					{
						((MainWindow)Application.Current.MainWindow).ImageCanvas.Width = int.Parse(CanvasInputW.Text);
						((MainWindow)Application.Current.MainWindow).ImageCanvas.Height = int.Parse(CanvasInputH.Text);
						foreach (FrameworkElement fe in ((MainWindow)Application.Current.MainWindow).ImageCanvas.Children)
						{
                            if(fe is Grid)
                            {
                                fe.Width = ((MainWindow)Application.Current.MainWindow).ImageCanvas.Width;
                                fe.Height = ((MainWindow)Application.Current.MainWindow).ImageCanvas.Height;
                                WriteableBitmap wb = ((Image)((Grid)fe).Children[0]).Source as WriteableBitmap;

                                WriteableBitmap bigboi = BitmapFactory.New((int)((MainWindow)Application.Current.MainWindow).ImageCanvas.Width, (int)((MainWindow)Application.Current.MainWindow).ImageCanvas.Height);

                                Rect sourceRect = new Rect(new Size(wb.Width, wb.Height));

                                bigboi.Blit(new Point(0, 0), wb, sourceRect, Colors.White, WriteableBitmapExtensions.BlendMode.Alpha);
                                ((Image)((Grid)fe).Children[0]).Source = bigboi;
                            }
						}
						break;
					}
				case ("Image Resize"):
					if (int.Parse(ImageInputW.Text) < 1 || int.Parse(ImageInputH.Text) > int.MaxValue
						|| int.Parse(ImageInputW.Text) > int.MaxValue || int.Parse(ImageInputH.Text) < 1)
					{
						AcceptButton.ToolTip = "Please enter valid input (integers > 0 and < 2.14748E9)";
						success = false;
						break;
					}
					else
					{
						if (int.Parse(ImageInputW.Text) != (int)((MainWindow)Application.Current.MainWindow).ImageCanvas.Width
							|| int.Parse(ImageInputH.Text) != (int)((MainWindow)Application.Current.MainWindow).ImageCanvas.Height)
						{
							double wChange = Double.Parse(ImageInputW.Text) / ((MainWindow)Application.Current.MainWindow).ImageCanvas.ActualWidth;
							double hChange = Double.Parse(ImageInputH.Text) / ((MainWindow)Application.Current.MainWindow).ImageCanvas.ActualHeight;

							((MainWindow)Application.Current.MainWindow).ImageCanvas.Width = int.Parse(ImageInputW.Text);
							((MainWindow)Application.Current.MainWindow).ImageCanvas.Height = int.Parse(ImageInputH.Text);
							foreach (FrameworkElement fe in ((MainWindow)Application.Current.MainWindow).ImageCanvas.Children)
							{
                                if(fe is Grid)
                                {
                                    fe.Width = ((MainWindow)Application.Current.MainWindow).ImageCanvas.Width;
                                    fe.Height = ((MainWindow)Application.Current.MainWindow).ImageCanvas.Height;
                                    WriteableBitmap wb = ((Image)((Grid)fe).Children[0]).Source as WriteableBitmap;
                                    wb = wb.Resize((int)(wb.PixelWidth * wChange), (int)(wb.PixelHeight * hChange), WriteableBitmapExtensions.Interpolation.NearestNeighbor);

                                    WriteableBitmap bigboi = BitmapFactory.New((int)((MainWindow)Application.Current.MainWindow).ImageCanvas.Width, (int)((MainWindow)Application.Current.MainWindow).ImageCanvas.Height);

                                    Rect sourceRect = new Rect(new Size(wb.Width, wb.Height));

                                    bigboi.Blit(new Point(0, 0), wb, sourceRect, Colors.White, WriteableBitmapExtensions.BlendMode.Alpha);
                                    ((Image)((Grid)fe).Children[0]).Source = bigboi;
                                }
							}
						}
						break;
					}
				case ("Hotkeys"):
					FocusManager.SetFocusedElement(this, null);
					Regex sWhitespace = new Regex(@"\s+");
					KeyConverter kc = new KeyConverter();
					ModifierKeysConverter mc = new ModifierKeysConverter();
					InputBindingCollection temp = new InputBindingCollection();

					foreach (InputBinding i in ((MainWindow)Application.Current.MainWindow).InputBindings)
					{
						string modName = sWhitespace.Replace(((RoutedCommand)i.Command).Name, "") + "Mod";
						string keyName = sWhitespace.Replace(((RoutedCommand)i.Command).Name, "") + "Key";
						string[] modParts = ((TextBox)HotkeysStack.FindName(modName)).Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

						if (modParts[0] != "None")
						{
							ModifierKeys a = (ModifierKeys)mc.ConvertFromString(modParts[0]);
							if (modParts.Length > 1)
							{
								ModifierKeys b = (ModifierKeys)mc.ConvertFromString(modParts[1]);
								Key c = (Key)kc.ConvertFromString(((TextBox)HotkeysStack.FindName(keyName)).Text);
								InputBinding copy = (InputBinding)i.Clone();
								copy.Gesture = new KeyGesture(c, a | b);
								temp.Add(copy);
							}
							else
							{
								Key c = (Key)kc.ConvertFromString(((TextBox)HotkeysStack.FindName(keyName)).Text);
								InputBinding copy = (InputBinding)i.Clone();
								((KeyBinding)copy).Key = c;
								((KeyBinding)copy).Modifiers = a;
								temp.Add(copy);
							}
						}
						else
						{
							Key c = (Key)kc.ConvertFromString(((TextBox)HotkeysStack.FindName(keyName)).Text);
							InputBinding copy = (InputBinding)i.Clone();
							((KeyBinding)copy).Key = c;
							((KeyBinding)copy).Modifiers = ModifierKeys.None;
							temp.Add(copy);
						}
					}
					((MainWindow)Application.Current.MainWindow).InputBindings.Clear();
					((MainWindow)Application.Current.MainWindow).InputBindings.AddRange(temp);
					((MainWindow)Application.Current.MainWindow).ReadWriteHotKeys(true);
					break;
				case ("New Image"):
					if (int.Parse(NewImageWidth.Text) < 1 || int.Parse(NewImageWidth.Text) > int.MaxValue
								|| int.Parse(NewImageHeight.Text) > int.MaxValue || int.Parse(NewImageHeight.Text) < 1)
					{
						AcceptButton.ToolTip = "Please enter valid input (integers > 0 and < 2.14748E9)";
						success = false;
						break;
					}
					else
					{
						((MainWindow)Application.Current.MainWindow).ImageCanvas.Width = Double.Parse(NewImageWidth.Text);
						((MainWindow)Application.Current.MainWindow).ImageCanvas.Height = Double.Parse(NewImageHeight.Text);
						((MainWindow)Application.Current.MainWindow).NewCanvas(null);
                        ((MainWindow)Application.Current.MainWindow).ImageName.Text = "Image";
                        ((MainWindow)Application.Current.MainWindow).ImageName.Visibility = Visibility.Visible;
                        ((MainWindow)Application.Current.MainWindow).ImageSize.Visibility = Visibility.Visible;
						((MainWindow)Application.Current.MainWindow).MousePos.Visibility = Visibility.Visible;
						((MainWindow)Application.Current.MainWindow).ResetSelection();
                        ((MainWindow)Application.Current.MainWindow).ToolbarSave.IsEnabled = true;
                        ((MainWindow)Application.Current.MainWindow).ToolbarSaveAs.IsEnabled = true;
                        ((MainWindow)Application.Current.MainWindow).ToolbarResizeCanvas.IsEnabled = true;
                        ((MainWindow)Application.Current.MainWindow).ToolbarResizeImage.IsEnabled = true;
                        ((MainWindow)Application.Current.MainWindow).Open = true;
						((MainWindow)Application.Current.MainWindow).Filepath = null;
						break;
					}
			}
			if (success)
				Close();
			if (TitleText.Text == "New Image" || TitleText.Text == "Canvas Resize" || TitleText.Text == "Image Resize")
				if (Actions.Commands.ZoomFitCmd.CanExecute(null, null))
					Actions.Commands.ZoomFitCmd.Execute(null, null);
		}

		/// <summary>
		/// Allows dragging of window from title.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.OriginalSource is DockPanel && e.LeftButton == MouseButtonState.Pressed)
				DragMove();
		}

        private void Options_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AcceptLabel.Visibility == Visibility.Visible)
                AcceptButton_Click(null, null);
            else if (e.Key == Key.Escape && ExitLabel.Visibility == Visibility.Visible)
                ExitButton_Click(null, null);
        }
    }
}
