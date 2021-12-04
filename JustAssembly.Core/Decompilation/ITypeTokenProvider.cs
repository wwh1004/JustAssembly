namespace JustAssembly.Core.Decompilation {
	public interface ITypeTokenProvider : ITokenProvider {
		uint TypeToken { get; }
	}
}
