using JustAssembly.Core;

namespace JustAssembly.Nodes.APIDiff {
	public class APIDiffInfo {
		public IMetadataDiffItem APIDiffItem { get; private set; }

		public APIDiffInfo(IMetadataDiffItem apiDiffItem) {
			APIDiffItem = apiDiffItem;
		}
	}
}
