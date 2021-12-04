namespace JustAssembly.Core.Decompilation {
	public interface IFileGeneratedInfo {
		string FullPath { get; }

		bool HasErrors { get; }
	}
}
