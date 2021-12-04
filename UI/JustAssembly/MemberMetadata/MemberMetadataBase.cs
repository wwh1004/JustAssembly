namespace JustAssembly {
	public abstract class MemberMetadataBase {
		private string name;

		public MemberMetadataBase(string assemblyPath, uint tokenId) {
			AssemblyPath = assemblyPath;

			TokenId = tokenId;
		}

		public readonly uint TokenId;

		public readonly string AssemblyPath;

		public string GetName() {
			if (string.IsNullOrWhiteSpace(name)) {
				name = GetNameInternal();
			}
			return name;
		}

		protected abstract string GetNameInternal();

		public abstract bool IsPublic { get; }
	}
}
