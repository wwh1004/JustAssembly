using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public interface IEventMethod
	{
		EventMethodType EventMethodType { get; }

		uint EventMethodToken { get; }
	}
}
