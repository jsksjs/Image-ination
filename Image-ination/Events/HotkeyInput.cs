using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Image_ination
{
	public partial class OptionsWindow
	{
		private Key[] modKeys = new Key[] { Key.LeftAlt, Key.RightAlt, Key.LeftCtrl, Key.RightCtrl,
											Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin, Key.None,
											Key.System, Key.Tab, Key.Capital, Key.Escape };
		private string origVal = "";

		/// <summary>
		/// Scruba-dub-dub key input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HotkeyInput_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (!modKeys.Contains(e.Key))
			{
				KeyConverter kc = new KeyConverter();
				string previewInput = ((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text + e.Key;
				if (!modKeyPairs.Contains(previewInput))
				{
                    modKeyPairs.Remove(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text + kc.ConvertFromString(origVal));
                    ((TextBox)e.OriginalSource).Text = kc.ConvertToString(e.Key);
                    origVal = ((TextBox)e.OriginalSource).Text;
                    modKeyPairs.Add(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text + kc.ConvertFromString(origVal));
                }
				else
				{
					((TextBox)e.OriginalSource).Text = origVal;
					e.Handled = true;
				}
			}
			e.Handled = true;
		}


		private void HotkeyInput_GotFocus(object sender, RoutedEventArgs e)
		{
			origVal = ((TextBox)e.OriginalSource).Text;
			((TextBox)e.OriginalSource).Text = "";
		}

		private void HotkeyInput_LostFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)e.OriginalSource).Text = origVal;
			Keyboard.ClearFocus();
		}

		/// <summary>
		/// Scruba-dub-dub mod input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HotkeyModInput_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			KeyConverter kc = new KeyConverter();
			if (e.Key.CompareTo(Key.Escape) == 0)
			{
				string previewInput = ModifierKeys.None.ToString() + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text);
				if (!modKeyPairs.Contains(previewInput))
				{
					modKeyPairs.Remove(origVal + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text));
					((TextBox)e.OriginalSource).Text = ModifierKeys.None.ToString();
					origVal = ((TextBox)e.OriginalSource).Text;
					modKeyPairs.Add(origVal + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text));
				}
				else
					e.Handled = true;
			}
			else if (e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Return)
				e.Handled = true;
			else
			{
				string key = "";
				if (e.Key.CompareTo(Key.LeftShift) == 0 || e.Key.CompareTo(Key.RightShift) == 0)
					key = ModifierKeys.Shift.ToString();
				else if (e.Key.CompareTo(Key.LeftCtrl) == 0 || e.Key.CompareTo(Key.RightCtrl) == 0)
					key = ModifierKeys.Control.ToString();
				else if (e.Key.CompareTo(Key.LeftAlt) == 0 || e.Key.CompareTo(Key.RightAlt) == 0 || e.Key.CompareTo(Key.System) == 0)
					key = ModifierKeys.Alt.ToString();
				else if (e.Key.CompareTo(Key.LWin) == 0 || e.Key.CompareTo(Key.RWin) == 0)
					key = ModifierKeys.Windows.ToString();
				else
				{
					e.Handled = true;
					return;
				}
				if (Regex.Matches(((TextBox)e.OriginalSource).Text, @"\b, \b").Count == 0 && (((TextBox)e.OriginalSource).Text.Length > 0 && ((TextBox)e.OriginalSource).Text != key && ((TextBox)e.OriginalSource).Text != ModifierKeys.None.ToString()))
				{
					List<string> combo = new List<string>();
					combo.Add(((TextBox)e.OriginalSource).Text);
					combo.Add(key);
					combo.Sort();
					string previewInput = (combo.ToArray()[0] + ", " + combo.ToArray()[1] + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text).ToString());
					if (!modKeyPairs.Contains(previewInput))
					{
						modKeyPairs.Remove(origVal + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text).ToString());
						((TextBox)e.OriginalSource).Text = combo.ToArray()[0] + ", " + combo.ToArray()[1];
						origVal = ((TextBox)e.OriginalSource).Text;
						modKeyPairs.Add(origVal + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text).ToString());
					}
					else
					{
						((TextBox)e.OriginalSource).Text = origVal;
						e.Handled = true;
					}
				}
				else
				{
					string previewInput = key + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text);
					if (!modKeyPairs.Contains(previewInput))
					{
						modKeyPairs.Remove(origVal + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text));
						((TextBox)e.OriginalSource).Text = key;
						origVal = ((TextBox)e.OriginalSource).Text;
						modKeyPairs.Add(origVal + kc.ConvertFromString(((TextBox)(((TextBox)e.OriginalSource).GetValue(PartnerElement.PartnerElementProperty))).Text));
					}
					else
					{
						((TextBox)e.OriginalSource).Text = origVal;
						e.Handled = true;
					}
				}
			}
			e.Handled = true;
		}
	}
}
