using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Image_ination
{
	partial class OptionsWindow
	{
		private static string currentOpen;
		public static string CurrentOpen
		{
			get { return currentOpen; }
			set { currentOpen = value; }
		}

		/// <summary>
		/// Remove trash.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			Regex regex = new Regex("[0-9]+");
			((TextBox)e.OriginalSource).Text = regex.Match(((TextBox)e.OriginalSource).Text).Value;
			((TextBox)e.OriginalSource).SelectAll();
		}

		/// <summary>
		/// Don't allow invalid input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsNums(e.Text);
		}

		/// <summary>
		/// Check if input is nums.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsNums(string text)
		{
			Regex regex = new Regex("[^0-9]+");
			return !regex.IsMatch(text);
		}

		/// <summary>
		/// On enter submit.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter || e.Key == Key.Return)
			{
				TextBox widthInput = null;
				TextBox heightInput = null;
				FrameworkElement ctrl = null;
				switch (CurrentOpen)
				{
					case "CanvasResizeTab":
						widthInput = CanvasInputW;
						heightInput = CanvasInputH;
						ctrl = ((MainWindow)Application.Current.MainWindow).ImageCanvas;
						break;
					case "ImageResizeTab":
						widthInput = ImageInputW;
						heightInput = ImageInputH;
						ctrl = ((MainWindow)Application.Current.MainWindow).currentLayer;
						break;
				}
				Regex regex = new Regex("[0-9]+");
				if (Double.Parse(widthInput.Text) < 1 || Double.Parse(widthInput.Text) > int.MaxValue)
				{
					((TextBox)e.OriginalSource).Text = ctrl.Width + "";
					AcceptLabel.ToolTip = "Please enter valid input for width (integers > 0 and < 2.14748E9)";
				}
				else if (Double.Parse(heightInput.Text) < 1 || Double.Parse(heightInput.Text) > int.MaxValue)
				{
					((TextBox)e.OriginalSource).Text = ctrl.Height + "";
					AcceptLabel.ToolTip = "Please enter valid input for height (integers > 0 and < 2.14748E9)";
				}
				else
				{
					AcceptButton_Click(sender, e);
				}
			}
		}

		private void New_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter || e.Key == Key.Return)
			{
				Regex regex = new Regex("[0-9]+");
				if (Double.Parse(NewImageWidth.Text) < 1 || Double.Parse(NewImageWidth.Text) > int.MaxValue)
				{
					((TextBox)e.OriginalSource).Text = 300 + "";
					AcceptLabel.ToolTip = "Please enter valid input for width (integers > 0 and < 2.14748E9)";
				}
				else if (Double.Parse(NewImageHeight.Text) < 1 || Double.Parse(NewImageWidth.Text) > int.MaxValue)
				{
					((TextBox)e.OriginalSource).Text = 300 + "";
					AcceptLabel.ToolTip = "Please enter valid input for height (integers > 0 and < 2.14748E9)";
				}
				else
				{
					AcceptButton_Click(sender, e);
				}
			}
		}

		/// <summary>
		/// Prevent pasting of tresh.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_Pasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(String)))
			{
				String text = (String)e.DataObject.GetData(typeof(String));
				if (!IsNums(text))
					e.CancelCommand();
			}
			else
				e.CancelCommand();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			TextBox widthInput = null;
			TextBox heightInput = null;
			FrameworkElement ctrl = null;
			switch (CurrentOpen)
			{
				case "CanvasResizeTab":
					widthInput = CanvasInputW;
					heightInput = CanvasInputH;
					ctrl = ((MainWindow)Application.Current.MainWindow).ImageCanvas;
					break;
				case "ImageResizeTab":
					widthInput = ImageInputW;
					heightInput = ImageInputH;
					ctrl = ((MainWindow)Application.Current.MainWindow).currentLayer;
					break;
			}
			Regex regex = new Regex("[0-9]+");
			double widthNum, heightNum;

			try { widthNum = Double.Parse(widthInput.Text); }
			catch (FormatException) { widthNum = -1; }

			try { heightNum = Double.Parse(heightInput.Text); }
			catch (FormatException) { heightNum = -1; }

			if (widthNum < 1 || widthNum > int.MaxValue)
			{
				((TextBox)e.OriginalSource).Text = ctrl.Width + "";
				AcceptLabel.ToolTip = "Please enter valid input for width (integers > 0 and < 2.14748E9)";
			}
			else if (heightNum < 1 || heightNum > int.MaxValue)
			{
				((TextBox)e.OriginalSource).Text = ctrl.Height + "";
				AcceptLabel.ToolTip = "Please enter valid input for height (integers > 0 and < 2.14748E9)";
			}
		}

		private void New_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			Regex regex = new Regex("[0-9]+");
			double widthNum, heightNum;

			try { widthNum = Double.Parse(NewImageWidth.Text); }
			catch (FormatException) { widthNum = -1; }

			try { heightNum = Double.Parse(NewImageHeight.Text); }
			catch (FormatException) { heightNum = -1; }

			if (widthNum < 1 || widthNum > int.MaxValue)
			{
				((TextBox)e.OriginalSource).Text = 300 + "";
				AcceptLabel.ToolTip = "Please enter valid input for width (integers > 0 and < 2.14748E9)";
			}
			else if (heightNum < 1 || heightNum > int.MaxValue)
			{
				((TextBox)e.OriginalSource).Text = 300 + "";
				AcceptLabel.ToolTip = "Please enter valid input for height (integers > 0 and < 2.14748E9)";
			}
		}
	}
}
