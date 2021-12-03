using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	class OffsetSpan : IOffsetSpan
	{
		public int StartOffset { get; private set; }

		public int EndOffset { get; private set; }

		public OffsetSpan(int startOffset, int endOffset)
		{
			this.StartOffset = startOffset;
			this.EndOffset = endOffset;
		}
	}
}
