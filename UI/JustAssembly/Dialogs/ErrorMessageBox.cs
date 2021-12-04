using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JustAssembly {
	public class ErrorMessageBox : TextBox {
		public ErrorMessageBox(string text) {
			Text = text;
			BorderBrush = new SolidColorBrush(Colors.Transparent);
			BorderThickness = new Thickness();
			IsReadOnly = true;
			TextWrapping = TextWrapping.Wrap;
		}
	}
}
