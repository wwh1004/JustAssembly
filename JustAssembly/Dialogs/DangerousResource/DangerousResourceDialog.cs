using System.Windows;

namespace JustAssembly.Dialogs.DangerousResource {
	internal class DangerousResourceDialog {
		private readonly string assemblyFileName;
		private readonly AssemblyType assemblyType;

		public DangerousResourceDialog(string assemblyFileName, AssemblyType assemblyType) {
			this.assemblyFileName = assemblyFileName;
			this.assemblyType = assemblyType;
		}

		public virtual DangerousResourceDialogResult Show() {
			string title = "Warning";
			string message = GetMessage();

			MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
			if (result == MessageBoxResult.Yes) {
				return DangerousResourceDialogResult.Yes;
			}

			return DangerousResourceDialogResult.No;
		}

		private string GetMessage() {
			return $"\"{assemblyFileName}\" [{assemblyType.ToString()}] contains resources that may contain malicious code. Decompilation of such resources will result in execution of that malicious code. Do you want to decompile those resources and use them for the assembly comparison?";
		}
	}
}
