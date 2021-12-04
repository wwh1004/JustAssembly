using System;
using System.Collections.Generic;

namespace JustAssembly.Core.Decompilation {
	class CodeViewerResults : ICodeViewerResults {
		public CodeViewerResults() {
			throw new NotImplementedException();
		}

		public IEnumerable<Tuple<int, ITokenProvider>> LineToMemberTokenMap { get; private set; }

		public string NewLine { get; private set; }
	}
}
