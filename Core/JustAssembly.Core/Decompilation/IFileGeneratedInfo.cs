using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public interface IFileGeneratedInfo
	{
		string FullPath { get; }

		bool HasErrors { get; }
	}
}
