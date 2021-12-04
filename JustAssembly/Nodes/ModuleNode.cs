using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JustAssembly.Core;
using JustAssembly.Interfaces;
using JustAssembly.MergeUtilities;
using JustAssembly.Nodes.APIDiff;

namespace JustAssembly.Nodes {
	class ModuleNode : ItemNodeBase {
		public ModuleNode(IOldToNewTupleMap<ModuleMetadata> modulesMap, ItemNodeBase parent, APIDiffInfo apiDiffInfo, bool shouldBeExpanded, FilterSettings filterSettings)
			: base(GetName(modulesMap), parent, apiDiffInfo, filterSettings) {
			ModulesMap = modulesMap;

			differenceDecoration = GetDifferenceDecoration();

			IsExpanded = shouldBeExpanded;
		}

		public readonly IOldToNewTupleMap<ModuleMetadata> ModulesMap;

		public override string FullName {
			get {
				return Name;
			}
		}

		public override object Icon {
			get { return ImagesResourceStrings.ModuleNode; }
		}

		public override NodeType NodeType {
			get { return NodeType.Module; }
		}

		protected override void LoadChildren() {
			LoadAPIItemsContext context = apiDiffInfo.GenerateLoadAPIItemsContext();

			List<IOldToNewTupleMap<TypeMetadata>> filteredTypeTuples = new TypesMergeManager(ModulesMap).GetMergedCollection().Where(ApiOnlyFilter).ToList();

			ObservableCollection<ItemNodeBase> result = new ObservableCollection<ItemNodeBase>(filteredTypeTuples
																		.GroupBy(GetNamespaceFromTypeMap)
																		.OrderBy(g => g.Key)
																		.Select(g => new NamespaceNode(g.Key, g.ToList(), GetDiffItemsList(g, context), this, FilterSettings)));



			context.Validate();

			DispatcherObjectExt.BeginInvoke(() => {
				foreach (var item in result) {
					Children.Add(item);
				}
			});
		}

		private IList<IMetadataDiffItem> GetDiffItemsList(IEnumerable<IOldToNewTupleMap<TypeMetadata>> metadataList, LoadAPIItemsContext context) {
			return context == null ? null : metadataList.Select(metadataTuple => context.GetDiffItem(metadataTuple)).ToList();
		}

		protected override DifferenceDecoration GetDifferenceDecoration() {
			if (CanUseParentDiffDecoration) {
				return ParentNode.DifferenceDecoration;
			}

			if (apiDiffInfo != null) {
				return apiDiffInfo.APIDiffItem.GetDifferenceDecoration(BreakingChangesOnly);
			}

			if (ModulesMap.OldType == null) {
				return DifferenceDecoration.Added;
			}
			else if (ModulesMap.NewType == null) {
				return DifferenceDecoration.Deleted;
			}
			else {
				return ModuleManager.AreModulesEquals(ModulesMap.OldType, ModulesMap.NewType) ? DifferenceDecoration.NoDifferences : DifferenceDecoration.Modified;
			}
		}

		private static string GetName(IOldToNewTupleMap<ModuleMetadata> typesMap) {
			return typesMap.GetFirstNotNullItem().GetName();
		}

		private string GetNamespaceFromTypeMap(IOldToNewTupleMap<TypeMetadata> typeDefMap) {
			return typeDefMap.GetFirstNotNullItem().GetNamespace();
		}
	}
}
