using JustAssembly.Core.Decompilation;

namespace JustAssembly {
	public class ModuleMetadata : MemberMetadataBase {
		private bool? containsPublicTypes;

		public ModuleMetadata(string assemblyPath, uint tokenId)
			: base(assemblyPath, tokenId) {
		}

		protected override string GetNameInternal() {
			return Decompiler.GetModuleName(AssemblyPath, TokenId);
		}

		public override bool IsPublic {
			get {
				if (containsPublicTypes == null) {
					containsPublicTypes = false;
					foreach (uint typeId in Decompiler.GetModuleTypes(AssemblyPath, TokenId)) {
						AccessModifier access = Decompiler.GetTypeAccessModifier(AssemblyPath, TokenId, typeId);
						if (access == AccessModifier.Public || access == AccessModifier.Protected) {
							containsPublicTypes = true;
							break;
						}
					}
				}
				return containsPublicTypes.Value;
			}
		}
	}
}
