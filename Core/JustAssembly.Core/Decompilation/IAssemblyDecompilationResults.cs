using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public interface IAssemblyDecompilationResults
	{
		string AssemblyFilePath { get; }
		IDecompilationResults AssemblyAttributesDecompilationResults { get; }
		ICollection<IModuleDecompilationResults> ModuleDecompilationResults { get; }
		ICollection<string> ResourcesFilePaths { get; }
	}
}
