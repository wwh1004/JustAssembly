using ICSharpCode.AvalonEdit.Editing;

namespace JustAssembly.Infrastructure.CodeViewer {
	internal class DiffLineNumberMargin : LineNumberMargin {
		public DiffLineNumberMargin(ICodeViewerResults sourceCode) {
			SourceCode = sourceCode;
		}

		public ICodeViewerResults SourceCode { get; set; }

		//protected override string GetLineNumberAsString(VisualLine line)
		//{
		//    if (this.SourceCode == null)
		//    {
		//        return line.FirstDocumentLine.LineNumber.ToString(CultureInfo.CurrentCulture);
		//    }

		//    return this.SourceCode.GetLineNumberString(line.FirstDocumentLine.LineNumber - 1);
		//}
	}
}
