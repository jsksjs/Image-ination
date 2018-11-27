using System.Windows;

namespace Image_ination
{
	public static class PartnerElement
	{

		public static UIElement GetPartnerElement(FrameworkElement obj)
		{
			return (FrameworkElement)obj.GetValue(PartnerElementProperty);
		}

		public static void SetPartnerElement(FrameworkElement obj, FrameworkElement value)
		{
			obj.SetValue(PartnerElementProperty, value);
		}

		public static readonly DependencyProperty PartnerElementProperty = DependencyProperty.Register(
			"PartnerElement",
			typeof(FrameworkElement),
			typeof(PartnerElement));
	}
}
