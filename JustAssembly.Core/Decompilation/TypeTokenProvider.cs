namespace JustAssembly.Core.Decompilation {
	class TypeTokenProvider : ITypeTokenProvider {
		public TokenProviderType TokenProviderType {
			get {
				return TokenProviderType.TypeTokenProvider;
			}
		}

		public uint TypeToken { get; private set; }

		public TypeTokenProvider(uint typeToken) {
			TypeToken = typeToken;
		}
	}
}
