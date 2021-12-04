using System.Collections.Generic;

namespace JustAssembly.Core.Decompilation {
	class DecompilationResults : IDecompilationResults {
		public string FilePath { get; private set; }
		public ICodeViewerResults CodeViewerResults { get; set; }
		public Dictionary<uint, IOffsetSpan> MemberDeclarationToCodePostionMap { get; private set; }
		public Dictionary<uint, IOffsetSpan> MemberTokenToDocumentationMap { get; private set; }
		public Dictionary<uint, IOffsetSpan> MemberTokenToAttributesMap { get; private set; }
		public Dictionary<uint, IOffsetSpan> MemberTokenToDecompiledCodeMap { get; private set; }
		public ICollection<uint> MembersWithExceptions { get; private set; }

		public DecompilationResults(string filePath, ICodeViewerResults codeViewerResults, Dictionary<uint, IOffsetSpan> memberDeclarationToCodePosition,
			Dictionary<uint, IOffsetSpan> memberTokenToDocumentationMap, Dictionary<uint, IOffsetSpan> memberTokenToAttributesMap, Dictionary<uint, IOffsetSpan> memberTokenToDecompiledCodeMap,
			ICollection<uint> membersWithExceptions) {
			FilePath = filePath;
			CodeViewerResults = codeViewerResults;
			MemberDeclarationToCodePostionMap = memberDeclarationToCodePosition;
			MemberTokenToDocumentationMap = memberTokenToDocumentationMap;
			MemberTokenToAttributesMap = memberTokenToAttributesMap;
			MemberTokenToDecompiledCodeMap = memberTokenToDecompiledCodeMap;
			MembersWithExceptions = membersWithExceptions;
		}
	}
}
