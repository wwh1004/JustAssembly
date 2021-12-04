using JustAssembly.Core.Decompilation;
using JustAssembly.Infrastructure.CodeViewer;
using JustAssembly.Nodes;

namespace JustAssembly.ViewModels {
	class AssemblyAttributeTabItem : CodeDiffTabItemBase<AssemblyNode> {
		public AssemblyAttributeTabItem(AssemblyNode assemblyNode)
			: base(assemblyNode) { }

		public override void LoadContent() {
			IsIndeterminate = true;
			IsBusy = true;
			IAssemblyDecompilationResults oldAssemblyResult;
			IAssemblyDecompilationResults newAssemblyResult;

			header = GetTabTitle(instance);
			toolTip = GetTabToolTip(instance);

			if (instance.TypesMap.OldType != null &&
				GlobalDecompilationResultsRepository.Instance.TryGetAssemblyDecompilationResult(instance.TypesMap.OldType, out oldAssemblyResult)) {
				LeftSourceCode = new DecompiledSourceCode(instance.OldMemberMetada, oldAssemblyResult.AssemblyAttributesDecompilationResults, instance.OldSource);
			}
			if (instance.TypesMap.NewType != null &&
				GlobalDecompilationResultsRepository.Instance.TryGetAssemblyDecompilationResult(instance.TypesMap.NewType, out newAssemblyResult)) {
				RightSourceCode = new DecompiledSourceCode(instance.NewMemberMetada, newAssemblyResult.AssemblyAttributesDecompilationResults, instance.NewSource);
			}
			ApplyDiff();
			IsBusy = false;
		}

		private string GetTabTitle(AssemblyNode instance) {
			return string.Format("{0} Attributes", instance.Name);
		}

		private string GetTabToolTip(AssemblyNode instance) {
			return string.Format("{0} Attributes", instance.Name);
		}
	}
}
