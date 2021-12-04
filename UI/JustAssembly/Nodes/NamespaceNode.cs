using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JustAssembly.Core;
using JustAssembly.Interfaces;
using JustAssembly.Nodes.APIDiff;

namespace JustAssembly.Nodes {
	internal class NamespaceNode : ItemNodeBase, IEquatable<NamespaceNode> {
		private readonly IList<IOldToNewTupleMap<TypeMetadata>> typesMap;
		private readonly IList<IMetadataDiffItem> diffItems;

		public NamespaceNode(string name, IList<IOldToNewTupleMap<TypeMetadata>> typesMap, IList<IMetadataDiffItem> diffItems, ItemNodeBase parent, FilterSettings filterSettings)
			: base(name, parent, filterSettings) {
			this.typesMap = typesMap;
			this.diffItems = diffItems;

			IsExpanded = true;
		}

		public override string FullName {
			get {
				return Name;
			}
		}

		public override object Icon {
			get { return ImagesResourceStrings.NamespaceNode; }
		}

		public override NodeType NodeType {
			get { return NodeType.Namespace; }
		}

		protected override void LoadChildren() {
			LoadAPIItemsContext context = diffItems != null ? new LoadAPIItemsContext(diffItems) : null;

			ObservableCollection<ItemNodeBase> result = new ObservableCollection<ItemNodeBase>(typesMap
						.Select(tuple => GenerateTypeNode(tuple, context)));

			context.Validate();

			DispatcherObjectExt.Invoke(() => {
				foreach (var item in result) {
					Children.Add(item);
				}

				differenceDecoration = GetDifferenceDecoration();
			});
		}

		private TypeNode GenerateTypeNode(IOldToNewTupleMap<TypeMetadata> metadataTuple, LoadAPIItemsContext context) {
			return new TypeNode(metadataTuple, this, context.GenerateAPIDiffInfo(metadataTuple), FilterSettings);
		}

		public bool Equals(NamespaceNode other) {
			if (other == null) {
				return false;
			}
			return Name == other.Name;
		}

		protected override DifferenceDecoration GetDifferenceDecoration() {
			if (CanUseParentDiffDecoration) {
				return ParentNode.DifferenceDecoration;
			}

			if (diffItems != null) {
				if (BreakingChangesOnly) {
					return diffItems.Any(item => item != null && item.IsBreakingChange) ? DifferenceDecoration.Modified : DifferenceDecoration.NoDifferences;
				}
				return diffItems.Any(item => item != null) ? DifferenceDecoration.Modified : DifferenceDecoration.NoDifferences;
			}

			bool isNew = true;
			bool isDeleted = true;
			bool isModified = false;

			foreach (TypeNode typeMap in Children) {
				if (typeMap.TypesMap.OldType != null) {
					isNew = false;
				}
				else {
					isModified = true;
				}
				if (typeMap.TypesMap.NewType != null) {
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
