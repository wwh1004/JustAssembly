namespace JustAssembly.Core.DiffAlgorithm.Models {
	public class DiffResult {
		public DiffFile File { get; set; }
		public DiffFile ModifiedFile { get; set; }
		public DiffResult() {
		}

		public DiffResult(DiffFile oldFile, DiffFile modifiedFile) {
			File = oldFile;
			ModifiedFile = modifiedFile;
		}
	}
}
