namespace JustAssembly.Core.DiffAlgorithm.Models {
	public class DiffBlock {
		public int EndPosition { get; set; }
		public int Offset { get; set; }
		public int StartPosition { get; set; }
		public DiffBlockType Type { get; set; }
		public DiffBlock() {
		}

		public DiffBlock(int offset, int startPosition, int endPosition)
			: this(offset, startPosition, endPosition, DiffBlockType.Imaginary) {
		}

		public DiffBlock(int offset, int startPosition, int endPosition, DiffBlockType type) {
			Offset = offset;
			StartPosition = startPosition;
			EndPosition = endPosition;
			Type = type;
		}
	}
}
