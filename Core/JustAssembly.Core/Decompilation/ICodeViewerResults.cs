using System;
using System.Collections.Generic;

namespace JustAssembly.Core.Decompilation {
	public interface ICodeViewerResults {
		IEnumerable<Tuple<int, ITokenProvider>> LineToMemberTokenMap { get; }

		string NewLine { get; }
	}
}
