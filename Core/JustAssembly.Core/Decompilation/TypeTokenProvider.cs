using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	class TypeTokenProvider : ITypeTokenProvider
	{
		public TokenProviderType TokenProviderType
		{
			get
			{
				return TokenProviderType.TypeTokenProvider;
			}
		}

		public uint TypeToken { get; private set; }

		public TypeTokenProvider(uint typeToken)
		{
			this.TypeToken = typeToken;
		}
	}
}
