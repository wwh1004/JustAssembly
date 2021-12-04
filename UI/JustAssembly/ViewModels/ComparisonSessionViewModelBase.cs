using System.Windows.Input;
using JustAssembly.Controls;
using JustAssembly.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace JustAssembly.ViewModels {
	abstract class ComparisonSessionViewModelBase : BindableBase, IOldToNewTupleMap<string>, IComparisonSessionModel {
		private string oldPath;
		private string newPath;

		public ICommand SwapPathsCommand { get; private set; }

		public ComparisonSessionViewModelBase(string header) {
			SwapPathsCommand = new DelegateCommand(OnSwapPathsCommandExecuted);

			Header = header;
		}

		public abstract SelectedItemType SelectedItemType { get; }

		public string Header { get; private set; }

		public string OldType {
			get => oldPath;
			set {
				if (oldPath != value) {
					oldPath = value;

					RaisePropertyChanged("OldType");

					RaisePropertyChanged("IsLoadEnabled");
				}
			}
		}

		public string NewType {
			get => newPath;
			set {
				if (newPath != value) {
					newPath = value;

					RaisePropertyChanged("NewType");

					RaisePropertyChanged("IsLoadEnabled");
				}
			}
		}

		public bool IsLoadEnabled {
			get { return GetLoadButtonState(); }
		}

		protected abstract bool GetLoadButtonState();

		public abstract ITabSourceItem GetTabSourceItem();

		private void OnSwapPathsCommandExecuted() {
			string tempPath = OldType;

			OldType = NewType;

			NewType = tempPath;
		}

		public string GetFirstNotNullItem() {
			return OldType ?? NewType;
		}
	}
}
