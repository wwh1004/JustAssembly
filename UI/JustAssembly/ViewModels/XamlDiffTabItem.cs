using JustAssembly.Infrastructure.CodeViewer;
using JustAssembly.Nodes;

namespace JustAssembly.ViewModels {
	class XamlDiffTabItem : CodeDiffTabItemBase<XamlResourceNode> {
		public XamlDiffTabItem(XamlResourceNode xamlResourceNode)
			: base(xamlResourceNode) {
		}

		public override void LoadContent() {
			IsIndeterminate = true;
			IsBusy = true;

			header = instance.Name;
			toolTip = instance.FullName;

			if (!string.IsNullOrWhiteSpace(instance.OldSource)) {
				LeftSourceCode = new DecompiledSourceCode(instance.OldSource);
			}
			if (!string.IsNullOrWhiteSpace(instance.NewSource)) {
				RightSourceCode = new DecompiledSourceCode(instance.NewSource);
			}
			ApplyDiff();
			IsBusy = false;
		}
	}
}
