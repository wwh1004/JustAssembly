using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Commands;

namespace JustAssembly.Controls {
	public partial class SelectorControl : System.Windows.Controls.Control {
		public SelectorControl() {
			DefaultStyleKey = typeof(SelectorControl);

			BrowseCommand = new DelegateCommand(OnBrowseFilePathExecuted);
		}

		public static readonly DependencyProperty FilePathProperty =
				DependencyProperty.Register("FilePath", typeof(string), typeof(SelectorControl));

		public static readonly DependencyProperty BrowseCommandProperty =
				DependencyProperty.Register("BrowseCommand", typeof(ICommand), typeof(SelectorControl));

		public static readonly DependencyProperty SelectedItemTypeProperty =
				DependencyProperty.Register("SelectedItemType", typeof(SelectedItemType), typeof(SelectorControl));

		public static readonly DependencyProperty HeaderProperty =
				DependencyProperty.Register("Header", typeof(string), typeof(SelectorControl));

		public static readonly DependencyProperty FilterProperty =
				DependencyProperty.Register("Filter", typeof(string), typeof(SelectorControl));

		public string Header {
			get => (string)GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		public string Filter {
			get => (string)GetValue(FilterProperty);
			set => SetValue(FilterProperty, value);
		}

		public SelectedItemType SelectedItemType {
			get => (SelectedItemType)GetValue(SelectedItemTypeProperty);
			set => SetValue(SelectedItemTypeProperty, value);
		}

		private void OnBrowseFilePathExecuted() {
			if (SelectedItemType == SelectedItemType.File) {
				GetEnteredFilePath();
			}
			else {
				GetEnteredFolderPath();
			}
		}

		private void GetEnteredFolderPath() {
			System.Windows.Forms.FolderBrowserDialog showDialog = new System.Windows.Forms.FolderBrowserDialog() {
				ShowNewFolderButton = true
			};

			System.Windows.Forms.DialogResult showDialogResult = showDialog.ShowDialog();

			if (showDialogResult == System.Windows.Forms.DialogResult.OK) {
				FilePath = showDialog.SelectedPath;
			}
		}

		private void GetEnteredFilePath() {
			OpenFileDialog showDialog = new OpenFileDialog {
				CheckFileExists = true,
				Multiselect = false,
				Filter = Filter
			};
			bool? showDialogResult = showDialog.ShowDialog();

			if (showDialogResult == true) {
				FilePath = showDialog.FileName;
			}
		}

		public ICommand BrowseCommand {
			get => (ICommand)GetValue(BrowseCommandProperty);
			set => SetValue(BrowseCommandProperty, value);
		}

		public string FilePath {
			get => (string)GetValue(FilePathProperty);
			set => SetValue(FilePathProperty, value);
		}
	}
}
