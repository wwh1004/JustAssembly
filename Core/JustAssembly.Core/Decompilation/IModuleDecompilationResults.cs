using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public interface IModuleDecompilationResults
	{
		uint ModuleToken { get; }
		string ModuleFilePath { get; }
		Dictionary<uint, IDecompilationResults> TypeDecompilationResults { get; }
	}
}
