using System.Windows;
using System.Windows.Input;
using JustAssembly.ViewModels;

namespace JustAssembly {

	public partial class Shell : Window {
		private readonly IShellViewModel shellViewModel;
		private readonly string[] args;

		public Shell(IShellViewModel viewModel, string[] args) {
			this.args = args;
			Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

			InitializeComponent();

			DataContext = shellViewModel = viewModel;

			Loaded += OnLoaded;

			PreviewKeyDown += OnMainWindowKeyDown;
		}

		private void OnLoaded(object sender, RoutedEventArgs e) {
			if (args?.Length == 0) {
				shellViewModel.OpenNewSessionCommandExecuted();
			}
			else {
				shellViewModel.OpenNewSessionWithCmdLineArgsCommandExecuted();
			}
		}

		private void OnMainWindowKeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Escape) {
				shellViewModel.CancelCurrentOperation();
			}
		}
	}
}
