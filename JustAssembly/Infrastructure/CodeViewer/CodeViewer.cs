using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using JustAssembly.Infrastructure.CodeViewer;
using JustAssembly.ViewModels;

namespace JustAssembly.Views {
	public class CodeViewer : TextEditor {
		private const int MinFontSize = 8;
		private const int MaxFontSize = 20;

		private DiffLineNumberMargin lineNumberMargin;
		private double viewportWidth;
		// We use this flag, because of bug in the horizontal scrolling.
		// A non maximized window is scrolled all the way to the right. When this window is maximized the TextView scrolls to the new value.
		// When the OnScrollOffsetChanged is entered, the value of the HorizontalScrollMaxumumValue is not updated yet and changes the
		// HorizontalOffsetPercent to invalid value. With this flag we skip this.
		private bool skipNextHorizontalScrollOffsetChange;

		public CodeViewer() {
			lineNumberMargin = new DiffLineNumberMargin(SourceCode);

			TextArea.Options.EnableHyperlinks = false;

			Loaded += OnLoaded;

			PreviewMouseWheel += OnPreviewMouseWheel;
			TextView.ScrollOffsetChanged += OnScrollOffsetChanged;
			TextView.VisualLinesChanged += OnVisualLinesChanged;
			TextView.SizeChanged += OnSizeChanged;

			TextArea.Caret.CaretBrush = new SolidColorBrush(Colors.Transparent);
		}

		public TextView TextView {
			get {
				return TextArea.TextView;
			}
		}

		public double HorizontalScrollMaxumumValue {
			get {
				return ExtentWidth - viewportWidth;
			}
		}

		public ICodeViewerResults SourceCode {
			get => (ICodeViewerResults)GetValue(SourceCodeProperty);
			set => SetValue(SourceCodeProperty, value);
		}

		// Using a DependencyProperty as the backing store for SourceCode.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SourceCodeProperty =
			DependencyProperty.Register("SourceCode", typeof(ICodeViewerResults), typeof(CodeViewer), new PropertyMetadata(OnSourceCodeChanged));

		public IBackgroundRenderer BackgroundRenderer {
			get => (IBackgroundRenderer)GetValue(BackgroundRendererProperty);
			set => SetValue(BackgroundRendererProperty, value);
		}

		// Using a DependencyProperty as the backing store for BackgroundRenderer.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BackgroundRendererProperty =
			DependencyProperty.Register("BackgroundRenderer", typeof(IBackgroundRenderer), typeof(CodeViewer), new PropertyMetadata(OnBackgroundRendererChanged));

		public new double VerticalOffset {
			get => (double)GetValue(VerticalOffsetProperty);
			set => SetValue(VerticalOffsetProperty, value);
		}

		// Using a DependencyProperty as the backing store for VerticalOffset.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty VerticalOffsetProperty =
			DependencyProperty.Register("VerticalOffset", typeof(double), typeof(CodeViewer), new PropertyMetadata(OnVerticalOffsetChanged));

		public double HorizontalOffsetPercent {
			get => (double)GetValue(HorizontalOffsetPercentProperty);
			set => SetValue(HorizontalOffsetPercentProperty, value);
		}

		// Using a DependencyProperty as the backing store for HorizontalOffsetPercent.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HorizontalOffsetPercentProperty =
			DependencyProperty.Register("HorizontalOffsetPercent", typeof(double), typeof(CodeViewer), new PropertyMetadata(OnHorizontalOffsetPercentChanged));

		public VisibleLines VisibleLines {
			get => (VisibleLines)GetValue(VisibleLinesProperty);
			set => SetValue(VisibleLinesProperty, value);
		}

		// Using a DependencyProperty as the backing store for VisibleLines.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty VisibleLinesProperty =
			DependencyProperty.Register("VisibleLines", typeof(VisibleLines), typeof(CodeViewer));

		public double ScrollingLimit {
			get => (double)GetValue(ScrollingLimitProperty);
			set => SetValue(ScrollingLimitProperty, value);
		}

		// Using a DependencyProperty as the backing store for ScrollingLimit.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ScrollingLimitProperty =
			DependencyProperty.Register("ScrollingLimit", typeof(double), typeof(CodeViewer), new PropertyMetadata(OnScrollingLimitChanged));

		public new double FontSize {
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		// Using a DependencyProperty as the backing store for FontSize.  This enables animation, styling, binding, etc...
		public static new readonly DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(double), typeof(CodeViewer), new PropertyMetadata(OnFontSizeChanged));

		void OnLoaded(object sender, RoutedEventArgs e) {
			UpdateVisibleLines();
			ScrollingLimit = TextView.DocumentHeight;
		}

		void OnScrollOffsetChanged(object sender, EventArgs e) {
			VerticalOffset = TextView.VerticalOffset;
			if (skipNextHorizontalScrollOffsetChange) {
				skipNextHorizontalScrollOffsetChange = false;
				return;
			}

			double newHorizontalOffsetPercent = TextView.HorizontalOffset / HorizontalScrollMaxumumValue;
			if (!double.IsNaN(newHorizontalOffsetPercent)) {
				HorizontalOffsetPercent = newHorizontalOffsetPercent;
			}
		}

		void OnVisualLinesChanged(object sender, EventArgs e) {
			if (SourceCode != null) {
				UpdateVisibleLines();
			}
			else {
				VisibleLines = new VisibleLines() {
					FirstLine = 0,
					LastLine = 0
				};
			}
		}

		void OnPreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {
			e.Handled = true;
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
				e.Handled = true;
				if (FontSize + e.Delta / 60 < MinFontSize) {
					FontSize = MinFontSize;
				}
				else if (FontSize + e.Delta / 60 > MaxFontSize) {
					FontSize = MaxFontSize;
				}
				else {
					FontSize += (e.Delta / 60);
				}
			}
			else {
				if (VerticalOffset - e.Delta < 0) {
					VerticalOffset = 0;
				}
				else if (VerticalOffset - e.Delta > ScrollingLimit) {
					VerticalOffset = ScrollingLimit;
				}
				else {
					VerticalOffset -= e.Delta;
				}
			}
		}

		void OnSizeChanged(object sender, SizeChangedEventArgs e) {
			if (e.WidthChanged) {
				viewportWidth = e.NewSize.Width;
				ScrollToHorizontalOffset(HorizontalOffsetPercent * HorizontalScrollMaxumumValue);
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
			base.OnRenderSizeChanged(sizeInfo);

			if (sizeInfo.WidthChanged) {
				if (VerticalScrollBarVisibility == System.Windows.Controls.ScrollBarVisibility.Hidden) {
					TextArea.Width = ActualWidth;
				}
				else {
					TextArea.Width = ActualWidth - SystemParameters.VerticalScrollBarWidth;
				}
				skipNextHorizontalScrollOffsetChange = true;
			}
		}

		//protected override LineNumberMargin GetLineNumberMargin()
		//{
		//    return this.lineNumberMargin;
		//}

		private void UpdateVisibleLines() {
			if (!TextView.VisualLinesValid) {
				TextView.EnsureVisualLines();
			}

			VisualLine firstVisualLine = TextView.VisualLines.FirstOrDefault();
			VisualLine lastVisualLine = TextView.VisualLines.LastOrDefault();
			if (firstVisualLine != null && lastVisualLine != null) {
				VisibleLines = new VisibleLines() {
					FirstLine = firstVisualLine.FirstDocumentLine.LineNumber - 1,
					LastLine = lastVisualLine.LastDocumentLine.LineNumber - 1
				};
			}
		}

		private void ScrollToMember() {
			Position position = SourceCode.GetMemberPosition();

			if (position != Position.Empty) {
				Selection selection = Selection.Create(TextArea, position.Start, position.End);

				Select(position.Start, position.Length);

				DispatcherObjectExt.BeginInvoke(() => ScrollTo(selection.StartPosition.Line, selection.EndPosition.Column));
			}
		}

		private static void OnSourceCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((CodeViewer)d).OnSourceCodeChanged();
		}

		private void OnSourceCodeChanged() {
			if (SourceCode != null) {
				Document = new TextDocument(SourceCode.GetSourceCode());
				if (SourceCode.HighlighMember) {
					ScrollToMember();
				}

				lineNumberMargin.SourceCode = SourceCode;
			}
			else {
				Document = new TextDocument();
			}
		}

		private static void OnBackgroundRendererChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((CodeViewer)d).OnBackgroundRendererChanged();
		}

		private void OnBackgroundRendererChanged() {
			TextView.BackgroundRenderers.Add(BackgroundRenderer);
		}

		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((CodeViewer)d).OnVerticalOffsetChanged((double)e.NewValue);
		}

		private void OnVerticalOffsetChanged(double p) {
			if (SourceCode != null) {
				ScrollToVerticalOffset(p);
			}
		}

		private static void OnHorizontalOffsetPercentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((CodeViewer)d).OnHorizontalOffsetPercentChanged((double)e.NewValue);
		}

		private void OnHorizontalOffsetPercentChanged(double p) {
			p = Math.Max(0, p);
			p = Math.Min(p, 1);

			if (SourceCode != null) {
				ScrollToHorizontalOffset(p * HorizontalScrollMaxumumValue);
			}
		}

		private static void OnScrollingLimitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			if ((double)e.NewValue < (double)e.OldValue) {
				((CodeViewer)d).ScrollingLimit = (double)e.OldValue;
			}
		}

		private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((CodeViewer)d).OnFontSizeChanged((double)e.NewValue);
		}

		private void OnFontSizeChanged(double p) {
			base.FontSize = p;
			if (SourceCode != null) {
				ScrollingLimit = TextView.DocumentHeight;
			}
		}
	}
}
