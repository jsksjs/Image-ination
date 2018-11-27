using System.Windows;
using System.Windows.Media;

namespace Image_ination
{
	public partial class MainWindow
	{
		public static readonly DependencyProperty ContentColorProperty =
			DependencyProperty.Register("ContentColor", typeof(Brush), typeof(MainWindow), new PropertyMetadata((Brush)(new BrushConverter()).ConvertFrom("#FF323238")));

		public Brush ContentColor
		{
			get { return (Brush)GetValue(ContentColorProperty); }
			set { SetValue(ContentColorProperty, value); }
		}

		public static readonly DependencyProperty UIColorProperty =
			DependencyProperty.Register("UIColor", typeof(Brush), typeof(MainWindow), new PropertyMetadata((Brush)(new BrushConverter()).ConvertFrom("#FF3F3F46")));

		public Brush UIColor
		{
			get { return (Brush)GetValue(UIColorProperty); }
			set { SetValue(UIColorProperty, value); }
		}

		public static readonly DependencyProperty OptionsListColorProperty =
			DependencyProperty.Register("OptionsListColor", typeof(Brush), typeof(MainWindow), new PropertyMetadata((Brush)(new BrushConverter()).ConvertFrom("#FF3F3F49")));

		public Brush OptionsListColor
		{
			get { return (Brush)GetValue(OptionsListColorProperty); }
			set { SetValue(OptionsListColorProperty, value); }
		}

		public static readonly DependencyProperty LayersListColorProperty =
			DependencyProperty.Register("LayersListColor", typeof(Brush), typeof(MainWindow), new PropertyMetadata((Brush)(new BrushConverter()).ConvertFrom("#FF3F3F42")));

		public Brush LayersListColor
		{
			get { return (Brush)GetValue(LayersListColorProperty); }
			set { SetValue(LayersListColorProperty, value); }
		}

	}
}
