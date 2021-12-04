using System.Collections.Generic;

namespace JustAssembly.Core.Decompilation {
	public interface IModuleDecompilationResults {
		uint ModuleToken { get; }
		string ModuleFilePath { get; }
		Dictionary<uint, IDecompilationResults> TypeDecompilationResults { get; }
	}
}
