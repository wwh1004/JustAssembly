using System.ComponentModel;
using System.IO;
using JustAssembly.Infrastructure;
using JustAssembly.Nodes;

namespace JustAssembly.Interfaces {
	abstract class BrowserTabSourceItemBase : TabSourceItemBase {
		private int projectCount;
		private bool breakingChangesOnly;
		private bool showAllUnmodified;
		private string selectedModificationFilter;

		protected string toolTip;
		protected ItemNodeBase[] nodes;
		protected bool[] contentLoaded;
		protected bool apiOnly;
		protected int currentNode;

		public BrowserTabSourceItemBase() {
			JustAssemblyViewModel = new JustAssemblyViewModel();

			JustAssemblyViewModel.PropertyChanged += OnJustAssemblyPropertyChanged;

			RaisePropertyChanged("JustAssemblyViewModel");

			ShowAllUnmodified = false;
		}

		public JustAssemblyViewModel JustAssemblyViewModel { get; private set; }

		public int ProjectCount {
			get => projectCount;
			set {
				if (projectCount != value) {
					projectCount = value;
				}
				RaisePropertyChanged("LoadingString");
			}
		}

		public bool ShowAllUnmodified {
			get => showAllUnmodified;
			set {
				if (showAllUnmodified != value) {
					if (Root != null) {
						Root.FilterSettings.ShowUnmodified = value;
						Root.ReloadChildren();
					}

					showAllUnmodified = value;

					RaisePropertyChanged("ShowAllUnmodified");
				}
			}
		}

		public bool APIOnly {
			get => apiOnly;
			set {
				if (apiOnly != value) {
					apiOnly = value;
					currentNode = value ? 1 : 0;
					if (!contentLoaded[currentNode]) {
						ReloadContent();
					}
					RaisePropertyChanged("APIOnly");
				}
			}
		}

		public bool BreakingChangesOnly {
			get => breakingChangesOnly;
			set {
				if (breakingChangesOnly != value) {
					breakingChangesOnly = value;
					ItemNodeBase apiNode = nodes[1];
					apiNode.BreakingChangesOnly = value;
					apiNode.RefreshDecoration();
					RaisePropertyChanged("BreakingChangesOnly");
				}
			}
		}

		public override void ReloadContent() {
			nodes[currentNode].ReloadChildren();
		}

		public ItemNodeBase Root {
			get {
				if (nodes != null) {
					return nodes[currentNode];
				}

				return null;
			}
		}

		public override string ToolTip {
			get {
				return toolTip;
			}
		}

		protected string GetTabTitle(IOldToNewTupleMap<string> comparableModel) {
			string oldItemName = string.Empty;
			string newItemName = string.Empty;
			if (comparableModel.OldType != null) {
				var oldDirInfo = new DirectoryInfo(comparableModel.OldType);
				oldItemName = oldDirInfo.Name;
			}
			if (comparableModel.NewType != null) {
				var newDirInfo = new DirectoryInfo(comparableModel.NewType);
				newItemName = newDirInfo.Name;
			}
			return string.Format("{0} <> {1}", oldItemName, newItemName);
		}

		protected string GetTabToolTip(IOldToNewTupleMap<string> comparableModel) {
			string oldItemName = string.Empty;
			string newItemName = string.Empty;
			if (comparableModel.OldType != null) {
				var oldDirInfo = new DirectoryInfo(comparableModel.OldType);
				oldItemName = oldDirInfo.FullName;
			}
			if (comparableModel.NewType != null) {
				var newDirInfo = new DirectoryInfo(comparableModel.NewType);
				newItemName = newDirInfo.FullName;
			}
			return string.Format("{0} <> {1}", oldItemName, newItemName);
		}

		private void OnJustAssemblyPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == "SelectedJustAssembly") {
				UpdateJustAssembly();
			}
		}

		private void UpdateJustAssembly() {
			switch (JustAssemblyViewModel.GetSelectedJustAssembly()) {
			case JustAssemblyerences.All:
				APIOnly = false;
				BreakingChangesOnly = false;
				break;
			case JustAssemblyerences.AllPublicApi:
				APIOnly = true;
				BreakingChangesOnly = false;
				break;
			case JustAssemblyerences.PublicApiBreakingChanges:
				APIOnly = true;
				BreakingChangesOnly = true;
				break;
			}
		}
	}
}
