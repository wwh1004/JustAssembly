using System;
using JustAssembly.Core;
using JustAssembly.Core.Decompilation;
using JustAssembly.Interfaces;
using JustAssembly.MergeUtilities;
using JustAssembly.Nodes;
using JustAssembly.Nodes.APIDiff;

namespace JustAssembly.ViewModels {
	class AssemblyBrowseTabItem : BrowserTabSourceItemBase {
		private double verticalOffset;

		public AssemblyBrowseTabItem(IOldToNewTupleMap<string> comparableModel) {
			APIDiffInfo diffInfo = new APIDiffInfo(APIDiffHelper.GetAPIDifferences(comparableModel.OldType, comparableModel.NewType));

			var generationProjectInfoMap = new OldToNewTupleMap<GeneratedProjectOutputInfo>
			   (
				   new GeneratedProjectOutputInfo(comparableModel.OldType),
				   new GeneratedProjectOutputInfo(comparableModel.NewType)
			   );

			FilterSettings filterSettings = new FilterSettings(ShowAllUnmodified);
			nodes = new AssemblyNode[] {
				new AssemblyNode(comparableModel, null, null, this, filterSettings) { GenerationProjectInfoMap = generationProjectInfoMap }
			};

			header = GetTabTitle(comparableModel);
			toolTip = GetTabToolTip(comparableModel);

			nodes[0].ChildrenLoaded += OnAssemblyNodeChildrenLoaded;
			contentLoaded = new bool[2];
		}

		public override TabKind TabKind {
			get { return TabKind.AssemblyBrowse; }
		}

		public double VerticalOffset {
			get => verticalOffset;
			set {
				if (verticalOffset != value) {
					verticalOffset = value;

					RaisePropertyChanged("VerticalOffset");
				}
			}
		}

		public override void OnProjectFileGenerated(IFileGeneratedInfo args) {
			Progress = progress + 1;
		}

		public override void Dispose() {
			CancelProgress();

			RemoveFromCache(((AssemblyNode)nodes[0]).TypesMap.OldType);
			RemoveFromCache(((AssemblyNode)nodes[0]).TypesMap.NewType);
		}

		private bool RemoveFromCache(string assemblyName) {
			if (!string.IsNullOrWhiteSpace(assemblyName)) {
				return GlobalDecompilationResultsRepository.Instance.RemoveByAssemblyPath(assemblyName);
			}
			return false;
		}

		private AssemblyNode GetCurrentNode() {
			return nodes[currentNode] as AssemblyNode;
		}

		public override void LoadContent() {
			var assemblyHelper = new AssemblyHelper();

			if (GetCurrentNode().HasInvalidAssemblies(assemblyHelper)) {
				assemblyHelper.TriggerNotSupportedFilesEvent();
			}
		}

		private void OnAssemblyNodeChildrenLoaded(object sender, EventArgs e) {
			contentLoaded[currentNode] = true;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}
}
