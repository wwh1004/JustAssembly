using System.IO;
using JustAssembly.Interfaces;

namespace JustAssembly.Nodes {
	class ResourceNode : ItemNodeBase, IResourceNode {
		public ResourceNode(IOldToNewTupleMap<string> resourceMap, ItemNodeBase parent, FilterSettings filterSettings)
			: base(GetResourceName(resourceMap.GetFirstNotNullItem()), parent, filterSettings) {
			ResourceMap = resourceMap;

			differenceDecoration = GetDifferenceDecoration();

			// This node has no child items, so this removes the expander.
			LazyLoading = false;
		}

		public override string FullName {
			get {
				return string.Format("{0}\\{1}", ParentNode.FullName, Name);
			}
		}

		public override object Icon {
			get { return ImagesResourceStrings.ResourceImage; }
		}

		public override NodeType NodeType {
			get { return NodeType.Resource; }
		}

		public IOldToNewTupleMap<string> ResourceMap { get; private set; }

		private static string GetResourceName(string resourcePath) {
			return Path.GetFileName(resourcePath);
		}

		protected override DifferenceDecoration GetDifferenceDecoration() {
			if (CanUseParentDiffDecoration) {
				return ParentNode.DifferenceDecoration;
			}
			else if (string.IsNullOrWhiteSpace(ResourceMap.OldType)) {
				return DifferenceDecoration.Added;
			}
			else if (string.IsNullOrWhiteSpace(ResourceMap.NewType)) {
				return DifferenceDecoration.Deleted;
			}
			else {
				byte[] oldResourceBytes = File.ReadAllBytes(ResourceMap.OldType);

				byte[] newResourceBytes = File.ReadAllBytes(ResourceMap.NewType);

				if (oldResourceBytes.Length != newResourceBytes.Length) {
					return DifferenceDecoration.Modified;
				}
				else {
					for (int j = 0; j < oldResourceBytes.Length; j++) {
						if (oldResourceBytes[j] != newResourceBytes[j]) {
							return DifferenceDecoration.Modified;
						}
					}
				}
				return DifferenceDecoration.NoDifferences;
			}
		}
	}
}
