using JustAssembly.Core.Decompilation;

namespace JustAssembly {
	class TypeMetadata : MemberDefinitionMetadataBase {
		private string fullName;

		public TypeMetadata(ModuleMetadata parentModule, uint tokenId)
			: base(parentModule, tokenId) {
		}

		public TypeMetadata(TypeMetadata typeMetadata, uint tokenId)
			: base(typeMetadata.Module, tokenId) {
			Type = typeMetadata;
		}

		public override MemberType MemberType {
			get { return MemberType.Type; }
		}

		protected override string GetNameInternal() {
			return Decompiler.GetTypeName(AssemblyPath, Module.TokenId, TokenId, SupportedLanguage.CSharp);
		}

		public string GetTypeFullName() {
			if (string.IsNullOrWhiteSpace(fullName)) {
				fullName = Decompiler.GetTypeFullName(AssemblyPath, Module.TokenId, TokenId, SupportedLanguage.CSharp);
			}
			return fullName;
		}

		public string GetNamespace() {
			return Decompiler.GetTypeNamespace(AssemblyPath, Module.TokenId, TokenId);
		}

		public override string ToString() {
			return GetName();
		}

		protected override AccessModifier GetAccessModifier() {
			return Decompiler.GetTypeAccessModifier(AssemblyPath, Module.TokenId, TokenId);
		}
	}
}
