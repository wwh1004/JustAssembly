using System.Collections.Generic;

namespace JustAssembly.Core.Decompilation {
	class AssemblyDecompilationResults : IAssemblyDecompilationResults {
		public string AssemblyFilePath { get; private set; }

		public IDecompilationResults AssemblyAttributesDecompilationResults { get; private set; }

		public ICollection<IModuleDecompilationResults> ModuleDecompilationResults { get; private set; }

		public ICollection<string> ResourcesFilePaths { get; private set; }

		public AssemblyDecompilationResults(string assemblyFilePath, IDecompilationResults assemblyAttributesDecompilationResults, ICollection<IModuleDecompilationResults> moduleDecompilationResults,
			ICollection<string> resourcesFilepaths) {
			AssemblyFilePath = assemblyFilePath;
			AssemblyAttributesDecompilationResults = assemblyAttributesDecompilationResults;
			ModuleDecompilationResults = moduleDecompilationResults;
			ResourcesFilePaths = resourcesFilepaths;
		}
	}
}
