using System.Collections.Generic;

namespace JustAssembly.Core.Decompilation {
	public interface IDecompilationResults {
		string FilePath { get; }
		ICodeViewerResults CodeViewerResults { get; }
		Dictionary<uint, IOffsetSpan> MemberDeclarationToCodePostionMap { get; }
		Dictionary<uint, IOffsetSpan> MemberTokenToDocumentationMap { get; }
		Dictionary<uint, IOffsetSpan> MemberTokenToAttributesMap { get; }
		Dictionary<uint, IOffsetSpan> MemberTokenToDecompiledCodeMap { get; }
		ICollection<uint> MembersWithExceptions { get; }
	}
}
