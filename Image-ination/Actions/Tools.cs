using System.Windows.Input;

namespace Image_ination
{
	public partial class MainWindow
	{
		private void PaintTool_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PaintTool_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Brush.IsChecked = true;
		}

        private void TranslateTool_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranslateTool_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Translate.IsChecked = true;
        }
    }
}
