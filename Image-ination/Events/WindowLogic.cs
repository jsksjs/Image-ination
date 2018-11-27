using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Image_ination
{
	/// <summary>
	/// Window logic for movement and interaction.
	/// </summary>
	public partial class MainWindow
	{
		/// <summary>
		/// Set sizes of things based on window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizyBoi_Loaded(object sender, RoutedEventArgs e)
		{
			Trans.X = LayerCanvas.ActualWidth / 2 - ImageCanvas.Width / 2;
			Trans.Y = LayerCanvas.ActualHeight / 2 - ImageCanvas.Height / 2;
			LayerScroll.ScrollToBottom();
			SetValue(ToolTipEnabled.ToolTipEnabledProperty, ToolTipsChecked.IsChecked);
			LayersList.MaxHeight = RightMenu.ActualHeight / 2;
			LayerScroll.MaxHeight = RightMenu.ActualHeight / 2 - Toolbario.ActualHeight - TitleDock.ActualHeight;
			OptionsList.MaxHeight = RightMenu.ActualHeight / 2;
			PaintScroll.Height = PaintScroll.MaxHeight = OptionsList.MaxHeight - BrushPreviewContainer.ActualHeight - PaintTitle.ActualHeight - 28;
			TranslateScroll.Height = PaintScroll.MaxHeight = OptionsList.MaxHeight - PaintTitle.ActualHeight - 28;
			SetPen();
		}

		/// <summary>
		/// Exit button. Complicated implementation very hard ggez.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Allows dragging of window from title.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TitleBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Disable icon drag
			if (e.OriginalSource is Border && e.LeftButton == MouseButtonState.Pressed)
			{
				// On double click normalize
				if (e.ClickCount == 2)
				{
					ToggleState();
				}
				else
				{
					// Handle a maximized window with a new handler registered to MouseMove
					if (WindowState == WindowState.Maximized)
					{
						// Register event handler to trigger and MouseMove
						MouseMove += new System.Windows.Input.MouseEventHandler(Handle_Window_Move);
						// Record position of initial event
						xPos = e.GetPosition(this).X;
						yPos = e.GetPosition(this).Y;
					}
					else
					{
						DragMove();
					}
				}
			}
		}

		/// <summary>
		/// Event handler registered by @TitleBorder_MouseLeftButtonDown(...), triggered at MouseMove.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Handle_Window_Move(object sender, System.Windows.Input.MouseEventArgs e)
		{
			// Check mouse delta between beginning pos and current pos in either axis
			if (Math.Abs(e.GetPosition(this).X - xPos) > delta || Math.Abs(e.GetPosition(this).Y - yPos) > delta)
			{
				// Deregister MouseMove event handler
				MouseMove -= Handle_Window_Move;
				// Maxed windows must be dealt with before move
				if (WindowState == WindowState.Maximized && e.LeftButton == MouseButtonState.Pressed)
				{
					ToggleState();

					// Go to top of current screen
					Top = Helper.ScreenFix.HeightScreensAbove(this);
					// Go to relative mouse position
					if (Helper.ScreenFix.GetScreenFrom(this).DeviceBounds.Left != 0)
					{
						double screens = Helper.ScreenFix.WidthScreensRight(this);
						if (screens != 0)
							Left = Helper.ScreenFix.GetScreenFrom(this).DeviceBounds.Left + e.GetPosition(this).X - ((e.GetPosition(this).X / Helper.ScreenFix.GetScreenFrom(this).WorkingArea.Width) * ActualWidth);
						else
							Left = e.GetPosition(this).X - ((e.GetPosition(this).X / MaxWidth) * ActualWidth);
					}
					else
						Left = e.GetPosition(this).X - ((e.GetPosition(this).X / Helper.ScreenFix.GetScreenFrom(this).WorkingArea.Width) * ActualWidth);
					DragMove();
				}
			}
		}

		/// <summary>
		/// Max/Normal button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RestoreDownMax_Click(object sender, RoutedEventArgs e)
		{
			ToggleState();
		}

		/// <summary>
		/// Minimize button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MinMax_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		/// <summary>
		/// Called on window state changed.
		/// Adds/removes padding based on pre-retrieved WinChrome values (@MainWindow()).
		/// Updates tooltips.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizyBoi_StateChanged(object sender, EventArgs e)
		{
			// Current padding of window
			Thickness pad = WindowBorder.Padding;
			switch (WindowState)
			{
				case WindowState.Maximized:
					{
						MaxHeight = Helper.ScreenFix.GetScreenFrom(this).WorkingArea.Height;
						MaxWidth = Helper.ScreenFix.GetScreenFrom(this).WorkingArea.Width;
						Chrome.ResizeBorderThickness = new Thickness(0);
						RestoreDownMaxLabel.ToolTip = "Normalize";
						WindowBorder.BorderThickness = new Thickness(0);
						if ((Application.Current.Windows.Count) > 2)
							App.Current.Windows[2].WindowState = WindowState.Normal;
						break;
					}
				case WindowState.Normal:
					{
						MaxHeight = SystemParameters.VirtualScreenHeight;
						MaxWidth = SystemParameters.VirtualScreenWidth;
						Chrome.ResizeBorderThickness = new Thickness(3);
						RestoreDownMaxLabel.ToolTip = "Maximize";
						WindowBorder.BorderThickness = new Thickness(1);
						if ((Application.Current.Windows.Count) > 2)
							App.Current.Windows[2].WindowState = WindowState.Normal;
						break;
					}
				case WindowState.Minimized:
					{
						if ((Application.Current.Windows.Count) > 2)
							App.Current.Windows[2].WindowState = WindowState.Minimized;
						break;
					}
			}
		}

		/// <summary>
		/// Toggles between normal and max.
		/// </summary>
		private void ToggleState()
		{
			switch (WindowState)
			{
				case WindowState.Maximized:
					{
						WindowState = WindowState.Normal;
						break;
					}
				case WindowState.Normal:
					{
						WindowState = WindowState.Maximized;
						break;
					}
			}

		}

		/// <summary>
		/// Toggle color modes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToggleMode_Click(object sender, RoutedEventArgs e)
		{
			BrushConverter conv = new BrushConverter();

			switch (dark)
			{
				// Commence eye bleed
				case true:
					{
						ContentColor = (Brush)(conv.ConvertFrom("#FF7F7F7F"));
						UIColor = (Brush)(conv.ConvertFrom("#FFB1B1B1"));
						OptionsListColor = (Brush)(conv.ConvertFrom("#FFB1B1B4"));
						LayersListColor = (Brush)(conv.ConvertFrom("#FFB1B1AD"));

						dark = !dark;
						ToggleModeLabel.ToolTip = "Dark Mode";
						break;
					}
				// Dark mode
				case false:
					{
						ContentColor = (Brush)(conv.ConvertFrom("#FF323238"));
						UIColor = (Brush)(conv.ConvertFrom("#FF3F3F46"));
						OptionsListColor = (Brush)(conv.ConvertFrom("#FF3F3F49"));
						LayersListColor = (Brush)(conv.ConvertFrom("#FF3F3F42"));

						dark = !dark;
						ToggleModeLabel.ToolTip = "Light Mode";
						break;
					}
			}
		}

		private void Tool_Checked(object sender, RoutedEventArgs e)
		{
			switch (((RadioButton)e.Source).Name)
			{
				case ("Brush"):
					Lists.SelectedItem = PaintTab;
					break;
				case ("Translate"):
					Lists.SelectedItem = TranslateTab;
					break;
			}
		}

		/// <summary>
		/// Shut it down.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizyBoi_Closed(object sender, System.EventArgs e)
		{
			App.Current.Shutdown();
		}

		/// <summary>
		/// Keyboard exits current text box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResizyBoi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), this as IInputElement);
		}

		private void ResetToolbarLabels()
		{
			foreach(InputBinding i in InputBindings)
			{
				switch (((RoutedCommand)i.Command).Name)
				{
					case ("New Image"):
						ToolbarNew.Header = "New      |  " + ((KeyBinding)i).Modifiers + " + " + ((KeyBinding)i).Key;
						break;
					case ("Open Image"):
						ToolbarOpen.Header = "Open     |  " + ((KeyBinding)i).Modifiers + " + " + ((KeyBinding)i).Key;
						break;
					case ("Save"):
						ToolbarSave.Header = "Save     |  " + ((KeyBinding)i).Modifiers + " + " + ((KeyBinding)i).Key;
						break;
					case ("Save As"):
						ToolbarSaveAs.Header = "Save As  |  " + ((KeyBinding)i).Modifiers + " + " + ((KeyBinding)i).Key;
						break;
					case ("Resize Canvas"):
						ToolbarResizeCanvas.Header = "Resize Canvas  |  " + ((KeyBinding)i).Modifiers + " + " + ((KeyBinding)i).Key;
						break;
					case ("Resize Image"):
						ToolbarResizeImage.Header = "Resize Image   |  " + ((KeyBinding)i).Modifiers + " + " + ((KeyBinding)i).Key;
						break;
					case ("Hotkeys"):
						ToolbarHotkeys.Header = "Hotkeys  |  " + ((KeyBinding)i).Modifiers + " + " + ((KeyBinding)i).Key;
						break;
				}
			}
		}

		private void ImageCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			Point mouse = e.GetPosition(ImageCanvas);
			MousePos.Text = (int)mouse.X + ", " + (int)mouse.Y;
		}
	}
}
