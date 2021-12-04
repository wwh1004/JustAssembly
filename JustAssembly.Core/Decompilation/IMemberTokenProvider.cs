namespace JustAssembly.Core.Decompilation {
	public interface IMemberTokenProvider : ITokenProvider {
		uint DeclaringTypeToken { get; }
		uint MemberToken { get; }
	}
}
