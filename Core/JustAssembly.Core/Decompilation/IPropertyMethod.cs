namespace JustAssembly.Core.Decompilation {
	public interface IPropertyMethod {
		PropertyMethodType PropertyMethodType { get; }
		uint PropertyMethodToken { get; }
	}
}
