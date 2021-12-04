using System.Collections.Generic;

namespace JustAssembly.Core.DiffAlgorithm.Models {
	public class DiffFile {
		public IList<DiffBlock> Blocks { get; set; }
		public IList<string> Lines { get; set; }
		public DiffFile()
			: this(new List<string>(), new List<DiffBlock>()) {
		}

		public DiffFile(IList<string> lines, IList<DiffBlock> blocks) {
			Lines = lines;
			Blocks = blocks;
		}
	}
}
