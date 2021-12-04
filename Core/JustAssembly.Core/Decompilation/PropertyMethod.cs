namespace JustAssembly.Core.Decompilation {
	class PropertyMethod : IPropertyMethod {
		public PropertyMethodType PropertyMethodType { get; private set; }

		public uint PropertyMethodToken { get; private set; }

		public PropertyMethod(PropertyMethodType propertyMethodType, uint propertyMethodToken) {
			PropertyMethodType = propertyMethodType;
			PropertyMethodToken = propertyMethodToken;
		}
	}
}
