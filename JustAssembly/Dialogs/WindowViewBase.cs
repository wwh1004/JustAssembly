using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JustAssembly {
	public class WindowViewBase : UserControl, IWindowView {
		public static readonly DependencyProperty IsClosedProperty =
			DependencyProperty.Register("IsClosed", typeof(bool), typeof(WindowViewBase), new PropertyMetadata(OnClose));

		public WindowViewBase() {
		}

		public virtual string Title {
			get {
				return string.Empty;
			}
		}

		public virtual ImageSource Icon {
			get {
				return null;// BitmapFrame.Create(new Uri("pack://application:,,,/Images/JD_16.png", UriKind.RelativeOrAbsolute));
			}
		}

		public bool IsClosed {
			get => (bool)GetValue(IsClosedProperty);
			set => SetValue(IsClosedProperty, value);
		}

		public virtual bool TrySetFocus() {
			return true;
		}

		public void Close() {
			OnClose(this, EventArgs.Empty);

			IsClosed = true;
		}

		protected void OnClose(object sender, EventArgs args) {
			SendCloseRequest(sender, args);

			OnClosed();
		}

		public virtual void OnClosed() {
		}

		public virtual void OnShowWindow() {
			IsClosed = false;
		}

		private static void OnClose(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			if ((bool)e.NewValue) {
				((WindowViewBase)sender).Close();
			}
		}

		public event EventHandler SendCloseRequest = delegate { };
	}
}
