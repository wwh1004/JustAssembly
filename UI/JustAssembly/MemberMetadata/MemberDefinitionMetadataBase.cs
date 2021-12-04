using JustAssembly.Core.Decompilation;
namespace JustAssembly {
	abstract class MemberDefinitionMetadataBase : MemberMetadataBase {
		private bool? isAPIMember;

		public MemberDefinitionMetadataBase(ModuleMetadata parentModule, uint tokenId)
			: base(parentModule.AssemblyPath, tokenId) {
			Module = parentModule;
		}

		public MemberDefinitionMetadataBase(TypeMetadata typeMetadata, uint tokenId)
			: this(typeMetadata.Module, tokenId) {
			Type = typeMetadata;
		}

		public abstract MemberType MemberType { get; }

		public override bool IsPublic {
			get {
				if (isAPIMember == null) {
					AccessModifier modifier = GetAccessModifier();
					isAPIMember = modifier == AccessModifier.Protected || modifier == AccessModifier.Public;
				}
				return isAPIMember.Value;
			}
		}

		protected abstract AccessModifier GetAccessModifier();

		public ModuleMetadata Module { get; private set; }

		public TypeMetadata Type { get; protected set; }
	}
}
