using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JustAssembly.Infrastructure;
using JustAssembly.Interfaces;
using JustAssembly.Nodes;
using Prism.Commands;
using Prism.Mvvm;

namespace JustAssembly.ViewModels {
	using System.IO;

	class ShellViewModel : BindableBase, IShellViewModel {
		private int selectedTabIndex;
		private DelegateCommand<ITabSourceItem> closeAllButThisCommand;
		private readonly string[] args;

		public ShellViewModel() {
			OpenNewSessionCommand = new DelegateCommand(OpenNewSessionCommandExecuted);

			TabCloseCommand = new DelegateCommand<ITabSourceItem>(OnTabCloseCommand);

			Commands.TabItemCloseCommand.RegisterCommand(TabCloseCommand);

			closeAllButThisCommand = new DelegateCommand<ITabSourceItem>(OnCloseAllButThisExecuted, OnCloseAllButThisCanExecute);
			Commands.CloseAllButThisCommand.RegisterCommand(closeAllButThisCommand);

			Commands.CloseAllCommand.RegisterCommand(new DelegateCommand(OnCloseAllExecuted));

			Tabs = new ObservableCollection<ITabSourceItem>();

			MouseDoubleSelectedCommand = new DelegateCommand<ItemNodeBase>(OnMouseDoubleSelectedCommandExecuted);
		}

		public ShellViewModel(string[] args) : this() {
			this.args = args;
		}

		public ICommand OpenNewSessionCommand { get; private set; }

		public ICommand MouseDoubleSelectedCommand { get; private set; }

		public ICommand TabCloseCommand { get; private set; }

		public ObservableCollection<ITabSourceItem> Tabs { get; private set; }

		public int SelectedTabIndex {
			get => selectedTabIndex;
			set {
				if (selectedTabIndex != value) {
					selectedTabIndex = value;

					RaisePropertyChanged("SelectedTabIndex");
				}
			}
		}

		public void CancelCurrentOperation() {
			if (selectedTabIndex > -1 && selectedTabIndex < Tabs.Count) {
				ITabSourceItem tabItem = Tabs[selectedTabIndex];

				tabItem.CancelProgress();

				tabItem.Dispose();
			}
		}

		public void OpenNewSessionCommandExecuted() {
			NewSessionViewModel newSessionViewModel = new NewSessionViewModel();
			var newSessionDialog = new NewSessionDialog { DataContext = newSessionViewModel };

			if (newSessionDialog.ShowDialog() == true) {
				OnLoadCommandExecuted(newSessionViewModel.SelectedSession);
			}
		}


		public void OpenNewSessionWithCmdLineArgsCommandExecuted() {
			if (!CommandLineArgumentsAreValidToSkipNewSessionDialog(args, showErrorMessageBoxIfPresentButInvalid: true)) {
				OpenNewSessionCommandExecuted();
			}
			else {
				AssembliesComparisonViewModel newAssembliesComparisonViewModel = new AssembliesComparisonViewModel(args);
				OnLoadCommandExecuted(newAssembliesComparisonViewModel);
			}
		}

		private static bool CommandLineArgumentsAreValidToSkipNewSessionDialog(string[] args, bool showErrorMessageBoxIfPresentButInvalid = false) {
			if (args.Length != 2) {
				//just return and show no message.
				return false;
			}

			var left = args[0];
			var right = args[1];
			var isValid = true;
			if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right)) {
				isValid = false;
			}
			else if (!File.Exists(left) || !File.Exists(right)) {
				isValid = false;
			}

			if (!isValid && showErrorMessageBoxIfPresentButInvalid) {
				ToolWindow.ShowDialog(
					new ErrorMessageWindow(
						$"Invalid path arguments.\nPath1: \"{left ?? "NULL"}\"\nPath2: \"{right ?? "NULL"}\"",
						"Invalid arguments"),
					width: 700,
					height: 200);
			}

			return isValid;

		}


		private void OnLoadCommandExecuted(IComparisonSessionModel newSession) {
			ITabSourceItem tabItem = newSession.GetTabSourceItem();

			tabItem.LoadContent();

			AddNewTabItem(tabItem);
		}

		private void OnTabCloseCommand(ITabSourceItem tabSourceItem) {
			tabSourceItem.Dispose();

			Tabs.Remove(tabSourceItem);

			closeAllButThisCommand.RaiseCanExecuteChanged();
		}

		private void OnMouseDoubleSelectedCommandExecuted(ItemNodeBase itemNode) {
			if (itemNode == null) {
				return;
			}
			switch (itemNode.NodeType) {
			case NodeType.Module:
			case NodeType.Directory:
			case NodeType.Namespace:
			case NodeType.DefaultResource:
				itemNode.IsExpanded = !itemNode.IsExpanded;
				break;

			case NodeType.AssemblyNode:
				var assemblyNode = (AssemblyNode)itemNode;
				OnAssemblySelected(assemblyNode, () => AddJustAssemblyTab(assemblyNode));
				break;

			case NodeType.DecompiledResource:
				var xamlResourceNode = (XamlResourceNode)itemNode;
				OnXamlSelected(xamlResourceNode);
				break;

			case NodeType.TypeDefinition:
			case NodeType.MemberDefinition:
				OnTypesSelected((DecompiledMemberNodeBase)itemNode);
				break;
			}
		}

		private void OnAssemblySelected(AssemblyNode assemblyNode, Action completeAction = null) {
			if (assemblyNode.Children.Count == 0) {
				Task loadingTask = assemblyNode.LoadItemsAsync(() => assemblyNode.IsExpanded = false);

				loadingTask.ContinueWith(t => {
					if (!t.IsCanceled && !t.IsFaulted) {
						if (completeAction != null) {
							completeAction();
						}
					}
					else if (t.Exception.InnerExceptions.Count > 0) {
						// already shown an error window.
						//this.ShowExceptionMessage(t.Exception.InnerExceptions);

					}
				}, TaskScheduler.FromCurrentSynchronizationContext());
			}
			else {
				if (completeAction != null) {
					completeAction();
				}
			}
		}

		private void ShowExceptionMessage(ReadOnlyCollection<Exception> readOnlyCollection) {
			var exceptionStringBuilder = new StringBuilder(readOnlyCollection.Count * 3);

			foreach (Exception exception in readOnlyCollection) {
				exceptionStringBuilder.Append(exception.Message)
									  .AppendLine()
									  .AppendLine(exception.StackTrace);
			}
			ToolWindow.ShowDialog(new ErrorMessageWindow(exceptionStringBuilder.ToString(), "Exception"), width: 800, height: 500);
		}

		private void AddJustAssemblyTab(AssemblyNode node) {
			var JustAssemblyTabItem = new AssemblyAttributeTabItem(node);

			JustAssemblyTabItem.LoadContent();

			AddNewTabItem(JustAssemblyTabItem);
		}

		private void OnTypesSelected(DecompiledMemberNodeBase typeNode) {
			var JustAssemblyTabItem = new JustAssemblyTabItem(typeNode);

			JustAssemblyTabItem.LoadContent();

			AddNewTabItem(JustAssemblyTabItem);
		}

		private void OnXamlSelected(XamlResourceNode xamlResourceNode) {
			var JustAssemblyTabItem = new XamlDiffTabItem(xamlResourceNode);

			JustAssemblyTabItem.LoadContent();

			AddNewTabItem(JustAssemblyTabItem);
		}

		private bool OnCloseAllButThisCanExecute(ITabSourceItem arg) {
			return Tabs.Count > 1;
		}

		private void OnCloseAllButThisExecuted(ITabSourceItem tabItem) {
			var removedIndex = new List<int>(Tabs.Count);

			for (int i = 0; i < Tabs.Count; i++) {
				if (Tabs[i] != tabItem) {
					removedIndex.Add(i);
				}
			}
			RemoveTabsByIndexes(removedIndex);
		}

		private void OnCloseAllExecuted() {
			var tabsList = Tabs.ToList();

			Tabs.Clear();

			foreach (var tab in tabsList) {
				tab.Dispose();
			}

			RaisePropertyChanged("Tabs");

			closeAllButThisCommand.RaiseCanExecuteChanged();
		}

		private void RemoveTabsByIndexes(IEnumerable<int> removedTabsIndexes) {
			var tabs = new Queue<ITabSourceItem>(Tabs.Count);

			List<ITabSourceItem> currentTabs = Tabs.ToList();

			foreach (int index in removedTabsIndexes) {
				tabs.Enqueue(Tabs[index]);
			}
			while (tabs.Count > 0) {
				ITabSourceItem removedTabItems = tabs.Dequeue();

				removedTabItems.Dispose();

				currentTabs.Remove(removedTabItems);
			}
			Tabs = new ObservableCollection<ITabSourceItem>(currentTabs);

			SelectedTabIndex = PredictNextValidTabIndex(removedTabsIndexes, selectedTabIndex);

			RaisePropertyChanged("Tabs");

			closeAllButThisCommand.RaiseCanExecuteChanged();
		}

		private int PredictNextValidTabIndex(IEnumerable<int> removedTabsIndexes, int currentTabIndex) {
			if (removedTabsIndexes.Contains(currentTabIndex)) {
				return PredictNextValidTabIndex(removedTabsIndexes, --currentTabIndex);
			}
			else if (Tabs.Count <= currentTabIndex) {
				currentTabIndex = Tabs.Count - 1;
			}
			return Math.Max(0, currentTabIndex);
		}

		private void AddNewTabItem(ITabSourceItem item) {
			Tabs.Add(item);

			SelectedTabIndex = Tabs.Count - 1;

			closeAllButThisCommand.RaiseCanExecuteChanged();
		}
	}
}
