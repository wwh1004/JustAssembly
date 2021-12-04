namespace JustAssembly.Core.Decompilation {
	public interface IEventMethod {
		EventMethodType EventMethodType { get; }

		uint EventMethodToken { get; }
	}
}
