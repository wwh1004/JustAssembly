using System.Windows;
using System.Windows.Controls;

namespace JustAssembly {
	public class SliderCheck : CheckBox {
		public static readonly DependencyProperty ShowTextProperty =
			DependencyProperty.Register("ShowText", typeof(string), typeof(SliderCheck), null);

		public static readonly DependencyProperty HideTextProperty =
			DependencyProperty.Register("HideText", typeof(string), typeof(SliderCheck), null);

		public SliderCheck() { }

		public string ShowText {
			get => (string)GetValue(ShowTextProperty);
			set => SetValue(ShowTextProperty, value);
		}

		public string HideText {
			get => (string)GetValue(HideTextProperty);
			set => SetValue(HideTextProperty, value);
		}
	}
}
