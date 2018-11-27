using System;
using System.Windows;
using System.Windows.Controls;

namespace Image_ination
{
	/// <summary>
	/// Contains tool tip service enable property that is bound.
	/// </summary>
	public static class ToolTipEnabled
	{
		public static Boolean GetIsToolTipEnabled(FrameworkElement obj)
		{
			return (Boolean)obj.GetValue(ToolTipEnabledProperty);
		}

		public static void SetToolTipEnabled(FrameworkElement obj, Boolean value)
		{
			obj.SetValue(ToolTipEnabledProperty, value);
		}

		public static readonly DependencyProperty ToolTipEnabledProperty = DependencyProperty.RegisterAttached(
			"IsToolTipEnabled",
			typeof(Boolean),
			typeof(ToolTipEnabled),
			new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits, (sender, e) =>
			{
				FrameworkElement element = sender as FrameworkElement;
				if (element != null)
				{
					element.SetValue(ToolTipService.IsEnabledProperty, e.NewValue);
				}
			}));
	}
}
