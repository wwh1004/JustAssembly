using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using JustAssembly.Core.Decompilation;
using JustAssembly.Interfaces;
using JustAssembly.MergeUtilities;

namespace JustAssembly.Nodes {
	class DefaultResourceNode : ItemNodeBase {
		private readonly IOldToNewTupleMap<string> typesMap;

		public DefaultResourceNode(IOldToNewTupleMap<string> typesMap, ItemNodeBase parent, IOldToNewTupleMap<GeneratedProjectOutputInfo> generationProjectInfoMap, FilterSettings filterSettings)
			: base("Resources", parent, filterSettings) {
			this.typesMap = typesMap;
			GenerationProjectInfoMap = generationProjectInfoMap;

			IsExpanded = true;
		}

		public override string FullName {
			get {
				return string.Format("{0}\\{1}", ParentNode.FullName, Name);
			}
		}

		public override object Icon {
			get { return ImagesResourceStrings.FolderNode; }
		}

		public override NodeType NodeType {
			get { return NodeType.DefaultResource; }
		}

		protected override void LoadChildren() {
			ICollection<string> oldResources = new Collection<string>();
			ICollection<string> newResources = new Collection<string>();

			IAssemblyDecompilationResults oldAssemblyResult;
			IAssemblyDecompilationResults newAssemblyResult;

			if (GlobalDecompilationResultsRepository.Instance.TryGetAssemblyDecompilationResult(typesMap.OldType, out oldAssemblyResult)) {
				oldResources = oldAssemblyResult.ResourcesFilePaths;
			}
			if (GlobalDecompilationResultsRepository.Instance.TryGetAssemblyDecompilationResult(typesMap.NewType, out newAssemblyResult)) {
				newResources = newAssemblyResult.ResourcesFilePaths;
			}
			var resourceMergeManager = new ResourceMergeManager(new OldToNewTupleMap<ICollection<string>>(oldResources, newResources),
				OldResourceNameComparer, NewResourceNameComparer, ResourceNameComparer);

			List<ItemNodeBase> resourceNodes = resourceMergeManager.GetMergedCollection()
																   .Select(GetResourceNode)
																   .ToList();

			DispatcherObjectExt.Invoke(() => {
				foreach (var item in resourceNodes) {
					Children.Add(item);
				}

				OnChildrenLoaded();

				differenceDecoration = GetDifferenceDecoration();
			});
		}

		private int OldResourceNameComparer(string firstResourcePath, string secondResourcePath) {
			string firstResourceRelativePath = GenerationProjectInfoMap.OldType.GetRelativePath(firstResourcePath);
			string secondResourceRelativePath = GenerationProjectInfoMap.OldType.GetRelativePath(secondResourcePath);

			return string.Compare(firstResourceRelativePath, secondResourceRelativePath, true);
		}

		private int NewResourceNameComparer(string firstResourcePath, string secondResourcePath) {
			string firstResourceRelativePath = GenerationProjectInfoMap.NewType.GetRelativePath(firstResourcePath);
			string secondResourceRelativePath = GenerationProjectInfoMap.NewType.GetRelativePath(secondResourcePath);

			return string.Compare(firstResourceRelativePath, secondResourceRelativePath, true);
		}

		private int ResourceNameComparer(string oldName, string newName) {
			bool isOldNameEmpty = string.IsNullOrWhiteSpace(oldName);

			bool isNewNameEmpty = string.IsNullOrWhiteSpace(newName);

			if (!isOldNameEmpty && !isNewNameEmpty) {
				oldName = GenerationProjectInfoMap.OldType.GetRelativePath(oldName);
				newName = GenerationProjectInfoMap.NewType.GetRelativePath(newName);

				return string.Compare(oldName, newName, true);
			}
			else if (isOldNameEmpty && isNewNameEmpty) {
				return 0;
			}
			else if (isOldNameEmpty && !isNewNameEmpty) {
				return 1;
			}
			else {
				return -1;
			}
		}

		private ItemNodeBase GetResourceNode(IOldToNewTupleMap<string> resourceMap) {
			string resourceFile = Path.GetExtension(resourceMap.GetFirstNotNullItem()).ToLower();

			switch (resourceFile) {
			case ".xaml":
				return new XamlResourceNode(resourceMap, GetResourceName(resourceMap), this, FilterSettings);

			default:
				return new ResourceNode(resourceMap, this, FilterSettings);
			}
		}

		private string GetResourceName(IOldToNewTupleMap<string> resourceMap) {
			if (resourceMap.OldType != null) {
				return GenerationProjectInfoMap.OldType.GetRelativePath(resourceMap.OldType);
			}
			else {
				return GenerationProjectInfoMap.NewType.GetRelativePath(resourceMap.NewType);
			}
		}

		protected override DifferenceDecoration GetDifferenceDecoration() {
			if (CanUseParentDiffDecoration) {
				return ParentNode.DifferenceDecoration;
			}
			else if (Children.Count == 0) {
				return DifferenceDecoration.NoDifferences;
			}
			else {
				bool isNew = true;
				bool isDeleted = true;
				bool isModified = false;

				foreach (IResourceNode typeMap in Children) {
					if (!string.IsNullOrWhiteSpace(typeMap.ResourceMap.OldType)) {
						isNew = false;
					}
					else {
						isModified = true;
					}
					if (!string.IsNullOrWhiteSpace(typeMap.ResourceMap.NewType)) {
						isDeleted = false;
					}
					else {
						isModified = true;
					}
					if (typeMap.DifferenceDecoration == DifferenceDecoration.Modified) {
						return DifferenceDecoration.Modified;
					}
				}
				if (isNew) {
					return DifferenceDecoration.Added;
				}
				else if (isDeleted) {
					return DifferenceDecoration.Deleted;
				}
				else if (isModified) {
					return DifferenceDecoration.Modified;
				}
				else {
					return DifferenceDecoration.NoDifferences;
				}
			}
		}
	}
}
