using System.Collections.ObjectModel;
using JustAssembly.Interfaces;
using Prism.Mvvm;

namespace JustAssembly.ViewModels {
	class NewSessionViewModel : BindableBase {
		private IComparisonSessionModel selectedSession;

		public NewSessionViewModel() {
			Tabs = new ObservableCollection<IComparisonSessionModel>
			{
				new AssembliesComparisonViewModel(),
				new FolderComparisonViewModel()
			};
			SelectedSession = Tabs[0];
		}

		public ObservableCollection<IComparisonSessionModel> Tabs { get; private set; }

		public IComparisonSessionModel SelectedSession {
			get => selectedSession;
			set {
				if (selectedSession != value) {
					selectedSession = value;

					RaisePropertyChanged("SelectedSession");
				}
			}
		}
	}
}
