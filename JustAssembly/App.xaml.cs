using System;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using JustAssembly.MergeUtilities;
using JustAssembly.ViewModels;

namespace JustAssembly {
	public partial class App : Application {
		private string[] args;

		public App() {
			AssemblyHelper.ErrorReadingAssembly += OnErrorReadingAssembly;
		}

		private void OnErrorReadingAssembly(object sender, ErrorAssemblyReadingEventArgs e) {
			if (e.NotSupportedAssemblyPaths.Count == 0) {
				return;
			}
			StringBuilder unsupportedFilesNames = new StringBuilder();
			foreach (string assembly in e.NotSupportedAssemblyPaths) {
				unsupportedFilesNames.Append(assembly);
				unsupportedFilesNames.Append(Environment.NewLine);
			}
			string errorCaption = "Not supported file(s):";

			string errorMessage = unsupportedFilesNames.ToString();

			DispatcherObjectExt.BeginInvoke(() => ToolWindow.ShowDialog(new ErrorMessageWindow(errorMessage, errorCaption), width: 800, height: 500), DispatcherPriority.Background);
		}

		protected override void OnStartup(StartupEventArgs e) {
			args = e.Args;
			base.OnStartup(e);

			OnShellRun();
		}

		private void OnShellRun() {
			Shell shell = new Shell(new ShellViewModel(args), args);
			shell.Show();
		}
	}
}
