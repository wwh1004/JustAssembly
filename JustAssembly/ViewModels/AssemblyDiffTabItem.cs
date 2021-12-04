using JustAssembly.Infrastructure.CodeViewer;
using JustAssembly.Nodes;

namespace JustAssembly.ViewModels {
	class JustAssemblyTabItem : CodeDiffTabItemBase<DecompiledMemberNodeBase> {
		public JustAssemblyTabItem(DecompiledMemberNodeBase typeNode)
			: base(typeNode) { }

		public override void LoadContent() {
			IsIndeterminate = true;
			IsBusy = true;

			header = instance.Name;
			toolTip = instance.FullName;

			if (instance.OldDecompileResult != null) {
				LeftSourceCode = new DecompiledSourceCode(instance.OldMemberMetada, instance.OldDecompileResult, instance.OldSource);
			}
			if (instance.NewDecompileResult != null) {
				RightSourceCode = new DecompiledSourceCode(instance.NewMemberMetada, instance.NewDecompileResult, instance.NewSource);
			}
			ApplyDiff();
			IsBusy = false;
		}
	}
}
