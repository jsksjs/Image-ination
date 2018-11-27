using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Image_ination
{
	/// <summary>
	/// Zoom textbox events.
	/// </summary>
	partial class MainWindow
	{

		/// <summary>
		/// Remove %.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ZoomPercent_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			ZoomPercent.Text = ZoomPercent.Text.Substring(0, ZoomPercent.Text.Length - 1);
		}

		/// <summary>
		/// Don't allow invalid input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ZoomPercent_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{             
			if ((e.Text == "." && ZoomPercent.Text.Contains(".")))
				e.Handled = true;
			else
				e.Handled = !IsNums(e.Text);
		}

		/// <summary>
		/// Check if input is nums.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsNums(string text)
		{
            if (text == ".")
                return true;
			Double dummy;
			return Double.TryParse(text, out dummy);
		}

		/// <summary>
		/// On enter submit.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ZoomPercent_PreviewKeyDown(object sender, KeyEventArgs e)
		{
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
			if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
			{
                if (ZoomPercent.Text == "")
                    ZoomPercent.Text = "" + Scale.ScaleX * 100;
                if (ZoomPercent.Text.Length < 1 || ZoomPercent.Text == ".")
					ZoomPercent.Text = "100.00%";
				Double zScale = Double.Parse(ZoomPercent.Text);
				Keyboard.ClearFocus();
				if (zScale <= 0)
				{
					zScale = 1;
					ZoomPercent.Text = zScale + ".00%";
				}
				else if (zScale > double.MaxValue)
				{
					zScale = 100;
					ZoomPercent.Text = zScale + ".00%";
				}
				Scale.ScaleX = Scale.ScaleY = zScale / 100;
				Content.Focus();
			}
		}

		/// <summary>
		/// Prevent pasting of tresh.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ZoomPercent_Pasting(object sender, DataObjectPastingEventArgs e)
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
		/// Add % again.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ZoomPercent_LostKeyboardFocus(object sender, RoutedEventArgs e)
		{
            if (ZoomPercent.Text == "")
                ZoomPercent.Text = "" + Scale.ScaleX * 100;
            ZoomPercent.Text += "%";
		}
	}
}
