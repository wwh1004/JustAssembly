using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public interface IPropertyMethod
	{
		PropertyMethodType PropertyMethodType { get; }
		uint PropertyMethodToken { get; }
	}
}
