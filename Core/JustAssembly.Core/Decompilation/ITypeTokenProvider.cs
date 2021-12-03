using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public interface ITypeTokenProvider : ITokenProvider
	{
		uint TypeToken { get; }
	}
}
