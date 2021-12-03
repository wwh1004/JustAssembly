using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	class MemberTokenProvider : IMemberTokenProvider
	{
		public TokenProviderType TokenProviderType
		{
			get
			{
				return TokenProviderType.MemberTokenProvider;
			}
		}

		public uint DeclaringTypeToken { get; private set; }

		public uint MemberToken { get; private set; }

		public MemberTokenProvider(uint declaringTypeToken, uint memberToken)
		{
			this.DeclaringTypeToken = declaringTypeToken;
			this.MemberToken = memberToken;
		}
	}
}
