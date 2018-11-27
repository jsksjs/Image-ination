using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Image_ination
{
	partial class MainWindow
	{
		/// <summary>
		/// The entire hotkey save/load feature. 
		/// Also detects corruptions and resets.
		/// </summary>
		/// <param name="newKeys">If there are new keys to be added, delete the existing file.</param>
		public void ReadWriteHotKeys(bool newKeys)
		{
			try
			{
				if (newKeys)
					File.Delete("Hotkeys.txt");
			}
			catch (UnauthorizedAccessException) { return; }
			// write hotkeys to file
			if (!File.Exists("Hotkeys.txt"))
				using (StreamWriter write = new StreamWriter("Hotkeys.txt"))
				{
					foreach (InputBinding i in ((MainWindow)Application.Current.MainWindow).InputBindings)
						write.WriteLine(((KeyBinding)i).Modifiers.ToString() + "|" + ((KeyGesture)i.Gesture).Key.ToString());
				}
			// load in hotkeys from file
			else
			{
				try
				{
					Regex sWhitespace = new Regex(@"\s+");
					KeyConverter kc = new KeyConverter();
					ModifierKeysConverter mc = new ModifierKeysConverter();
					InputBinding[] temp = new InputBinding[InputBindings.Count];
					InputBindings.CopyTo(temp, 0);
                    string[] lineC = File.ReadAllLines("Hotkeys.txt");
                    if (lineC.Length != temp.Length)
                        ReadWriteHotKeys(true);
                    int j = 0;
					foreach (string line in lineC)
					{
						ModifierKeys a, b;
						Key c;
						string[] modKey = line.Split('|');
						if (modKey[0] != "None")
						{
							string[] modParts = sWhitespace.Replace(modKey[0], "").Split(',');
							a = (ModifierKeys)mc.ConvertFromString(modParts[0]);
							b = a;
							if (modParts.Length > 1)
							{
								b = (ModifierKeys)mc.ConvertFromString(modParts[1]);
								c = (Key)kc.ConvertFromString(modKey[1]);
							}
							else
								c = (Key)kc.ConvertFromString(modKey[1]);
							if (a != b)
								temp[j].Gesture = new KeyGesture(c, a | b);
							else
							{
								((KeyBinding)temp[j]).Key = c;
								((KeyBinding)temp[j]).Modifiers = a;
							}
						}
						else
						{
							c = (Key)kc.ConvertFromString(modKey[1]);
							((KeyBinding)temp[j]).Key = c;
							((KeyBinding)temp[j]).Modifiers = ModifierKeys.None;
						}
						j++;
					}
					InputBindings.Clear();
					InputBindings.AddRange(temp);
				}
				catch (Exception)
				{
					ReadWriteHotKeys(true);
				}
			}
			ResetToolbarLabels();
		}

		private void NewImage_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		private void NewImage_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			New_Click(null, null);
		}

		private void OpenImage_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		private void OpenImage_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Open_Click(null, null);
		}

		private void SaveImage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (open)
				e.CanExecute = true;
			else
				e.CanExecute = false;
		}
		private void SaveImage_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Save_Click(null, null);
		}

		private void SaveImageAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (open)
				e.CanExecute = true;
			else
				e.CanExecute = false;
		}
		private void SaveImageAs_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveAs_Click(null, null);
		}

		private void ResizeImage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (open)
				e.CanExecute = true;
			else
				e.CanExecute = false;
		}
		private void ResizeImage_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ResizeImage_Click(null, null);
		}

		private void ResizeCanvas_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (open)
				e.CanExecute = true;
			else
				e.CanExecute = false;
		}
		private void ResizeCanvas_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ResizeCanvas_Click(null, null);
		}

		private void Hotkeys_CanExecute(object sender, CanExecuteRoutedEventArgs e){ e.CanExecute = true; }
		private void Hotkeys_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Hotkeys_Click(null, null);
		}
	}
}
