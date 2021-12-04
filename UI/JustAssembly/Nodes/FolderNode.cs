using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ICSharpCode.TreeView;
using JustAssembly.Core;
using JustAssembly.Interfaces;
using JustAssembly.MergeUtilities;
using JustAssembly.Nodes.APIDiff;

namespace JustAssembly.Nodes {
	class FolderNode : ItemNodeBase {
		private readonly IOldToNewTupleMap<string> folderMap;

		private readonly IProgressNotifier progressNotifier;

		private bool loadChildrenAsync;

		public FolderNode(IOldToNewTupleMap<string> folderMap, ItemNodeBase parent, APIDiffInfo apiDiffInfo, IProgressNotifier progressNotifier, FilterSettings filterSettings, bool loadChildrenAsync)
			: base(GetName(folderMap), parent, apiDiffInfo, filterSettings) {
			this.folderMap = folderMap;

			this.progressNotifier = progressNotifier;

			// This line needs to be before this.IsExpanded = true; in any case.
			this.loadChildrenAsync = loadChildrenAsync;

			IsExpanded = true;
		}

		public override string FullName {
			get {
				return string.Format("{0}\\{1}", ParentNode.Name, Name);
			}
		}

		public override object Icon {
			get { return ImagesResourceStrings.FolderNode; }
		}

		public override NodeType NodeType {
			get { return NodeType.Directory; }
		}

		private void ConfigProgressNotifier() {
			progressNotifier.IsIndeterminate = false;
			progressNotifier.IsBusy = true;
			progressNotifier.LoadingMessage = "Processing folders...";
		}

		protected override void LoadChildren() {
			if (loadChildrenAsync) {
				ConfigProgressNotifier();
				CancellationToken cancellationToken = progressNotifier.GetCanellationToken();
				LoadItemsAsync(cancellationToken, true);
			}
			else {
				LoadItemsSequential(false);
			}
		}

		private void LoadItemsAsync(CancellationToken cancellationToken, bool provideNotSupportedAssemblyNotification) {
			Task task = Task.Factory.StartNew(() => LoadItemsInternal(provideNotSupportedAssemblyNotification), cancellationToken);
			task.ContinueWith(t => progressNotifier.Completed());
		}

		private void LoadItemsInternal(bool provideNotSupportedAssemblyNotification) {
			ClrAssemblyFileMergeManager fileMergeManager = new ClrAssemblyFileMergeManager(folderMap, provideNotSupportedAssemblyNotification, progressNotifier);

			int count = fileMergeManager.GetAssembliesCount(folderMap);
			progressNotifier.TotalFileCount = (uint)count;

			LoadItemsSequential(provideNotSupportedAssemblyNotification);
		}

		private static string GetName(IOldToNewTupleMap<string> oldToNewTupleMap) {
			var dirInfo = new DirectoryInfo(oldToNewTupleMap.GetFirstNotNullItem());

			return dirInfo.Name;
		}

		private void LoadItemsSequential(bool provideNotSupportedAssemblyNotification) {
			ClrAssemblyFileMergeManager fileMergeManager = new ClrAssemblyFileMergeManager(folderMap, provideNotSupportedAssemblyNotification, progressNotifier);

			FolderMergeManager folderMergeManager = new FolderMergeManager(folderMap);

			IEnumerable<FolderNode> mergedFolders = folderMergeManager.GetMergedCollection().Select(ProcessFolderNodeCreation).ToList();

			IEnumerable<ItemNodeBase> mergedAssemblies = fileMergeManager.GetMergedCollection().Select(ProcessAssemblyNodeCreation).ToList();

			List<ItemNodeBase> nodes = mergedAssemblies.Union(mergedFolders).ToList();

			differenceDecoration = GetDifferenceDecoration(nodes);

			LoadItems(nodes);
		}

		private void LoadItems(List<ItemNodeBase> nodes) {
			DispatcherObjectExt.BeginInvoke(() => {
				foreach (var item in nodes) {
					Children.Add(item);
				}

				OnChildrenLoaded();
			}, DispatcherPriority.Background);
		}

		protected override DifferenceDecoration GetDifferenceDecoration() {
			return GetDifferenceDecoration(Children);
		}

		private DifferenceDecoration GetDifferenceDecoration(IEnumerable<SharpTreeNode> mergedAssemblies) {
			if (CanUseParentDiffDecoration) {
				return ParentNode.DifferenceDecoration;
			}
			else if (string.IsNullOrWhiteSpace(folderMap.OldType)) {
				return DifferenceDecoration.Added;
			}
			else if (string.IsNullOrWhiteSpace(folderMap.NewType)) {
				return DifferenceDecoration.Deleted;
			}
			else {
				foreach (ItemNodeBase assemblyNode in mergedAssemblies) {
					if (assemblyNode.DifferenceDecoration == DifferenceDecoration.NoDifferences) {
						continue;
					}
					else {
						return DifferenceDecoration.Modified;
					}
				}
				return DifferenceDecoration.NoDifferences;
			}
		}

		private FolderNode ProcessFolderNodeCreation(IOldToNewTupleMap<string> typesMap) {
			APIDiffInfo diffInfo = apiDiffInfo == null ? null : new APIDiffInfo(null);

			var folderNode = new FolderNode(typesMap, this, diffInfo, progressNotifier, FilterSettings, false);

			return folderNode;
		}

		private AssemblyNode ProcessAssemblyNodeCreation(IOldToNewTupleMap<string> typesMap) {
			APIDiffInfo diffInfo = apiDiffInfo != null ? new APIDiffInfo(APIDiffHelper.GetAPIDifferences(typesMap.OldType, typesMap.NewType)) : null;

			var generationProjectInfoMap = new OldToNewTupleMap<GeneratedProjectOutputInfo>
			  (
				  new GeneratedProjectOutputInfo(typesMap.OldType),
				  new GeneratedProjectOutputInfo(typesMap.NewType)
			  );
			var assemblyNode = new AssemblyNode(typesMap, this, diffInfo, progressNotifier, FilterSettings) { GenerationProjectInfoMap = generationProjectInfoMap };

			assemblyNode.SetDifferenceDecoration();

			return assemblyNode;
		}
	}
}
