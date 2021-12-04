using System.Windows;
using System.Windows.Input;

namespace JustAssembly {
	public partial class NewSessionDialog : Window {
		public NewSessionDialog() {
			InitializeComponent();

			ShowInTaskbar = false;

			ResizeMode = ResizeMode.NoResize;

			WindowStyle = WindowStyle.ToolWindow;

			WindowStartupLocation = WindowStartupLocation.CenterOwner;

			Owner = Application.Current.MainWindow;

			KeyDown += OnKeyDown;
		}

		private void OnKeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Escape) {
				DialogResult = false;

				Close();
			}
		}

		public bool ShowModal() {
			return ShowDialog() == true;
		}

		private void OnLoadClick(object sender, RoutedEventArgs e) {
			DialogResult = true;

			Close();
		}
	}
}
