namespace JustAssembly.Core.Decompilation {
	class OffsetSpan : IOffsetSpan {
		public int StartOffset { get; private set; }

		public int EndOffset { get; private set; }

		public OffsetSpan(int startOffset, int endOffset) {
			StartOffset = startOffset;
			EndOffset = endOffset;
		}
	}
}
