using System.Collections.Generic;

namespace JustAssembly.Core.Decompilation {
	class ModuleDecompilationResults : IModuleDecompilationResults {
		public uint ModuleToken { get; private set; }

		public string ModuleFilePath { get; private set; }

		public Dictionary<uint, IDecompilationResults> TypeDecompilationResults { get; private set; }

		public ModuleDecompilationResults(uint moduleToken, string moduleFilePath, Dictionary<uint, IDecompilationResults> typeDecompilationResults) {
			ModuleToken = moduleToken;
			ModuleFilePath = moduleFilePath;
			TypeDecompilationResults = typeDecompilationResults;
		}
	}
}
