using JustAssembly.Core.DiffAlgorithm;
using JustAssembly.Core.DiffAlgorithm.Models;
using JustAssembly.Infrastructure.CodeViewer;
using JustAssembly.Interfaces;
using JustAssembly.Nodes;

namespace JustAssembly.ViewModels {
	internal abstract class CodeDiffTabItemBase<T> : TabSourceItemBase
		where T : DecompiledMemberNodeBase {
		private const int InitialFontSize = 12;

		private double verticalOffset;
		private double horizontalOffsetPercent;
		private double fontSize;

		protected string toolTip;

		protected readonly T instance;

		private VisibleLines visibleLines;

		private double scrollingLimit;

		public CodeDiffTabItemBase(T param) {
			instance = param;

			FontSize = InitialFontSize;
		}

		public double VerticalOffset {
			get => verticalOffset;
			set {
				if (verticalOffset != value) {
					verticalOffset = value;
					RaisePropertyChanged("VerticalOffset");
				}
			}
		}

		public VisibleLines VisibleLines {
			get => visibleLines;
			set {
				if (visibleLines != value) {
					visibleLines = value;
					RaisePropertyChanged("VisibleLines");
				}
			}
		}

		public double ScrollingLimit {
			get => scrollingLimit;
			set {
				if (scrollingLimit != value) {
					scrollingLimit = value;
					RaisePropertyChanged("ScrollingLimit");
				}
			}
		}

		public double HorizontalOffsetPercent {
			get => horizontalOffsetPercent;
			set {
				if (horizontalOffsetPercent != value) {
					horizontalOffsetPercent = value;
					RaisePropertyChanged("HorizontalOffsetPercent");
				}
			}
		}

		public double FontSize {
			get => fontSize;
			set {
				if (fontSize != value) {
					fontSize = value;
					RaisePropertyChanged("FontSize");
				}
			}
		}

		public override TabKind TabKind {
			get {
				return TabKind.JustAssembly;
			}
		}

		public ICodeViewerResults LeftSourceCode { get; set; }

		public ICodeViewerResults RightSourceCode { get; set; }

		protected virtual void ApplyDiff() {
			DiffResult diffResult = DiffHelper.Diff(instance.OldSource, instance.NewSource);

			if (LeftSourceCode != null) {
				LeftSourceCode.ApplyDiffInfo(diffResult.File);
			}
			if (RightSourceCode != null) {
				RightSourceCode.ApplyDiffInfo(diffResult.ModifiedFile);
			}
			RaisePropertyChanged("LeftSourceCode");
			RaisePropertyChanged("RightSourceCode");
		}

		public override void OnProjectFileGenerated(JustAssembly.Core.Decompilation.IFileGeneratedInfo args) {

		}

		public override string ToolTip {
			get {
				return toolTip;
			}
		}
	}

	public class VisibleLines {
		public int FirstLine { get; set; }
		public int LastLine { get; set; }
	}
}
