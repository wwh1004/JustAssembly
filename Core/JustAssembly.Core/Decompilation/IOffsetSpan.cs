using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public interface IOffsetSpan
	{
		int StartOffset { get; }
		int EndOffset { get; }
	}
}
