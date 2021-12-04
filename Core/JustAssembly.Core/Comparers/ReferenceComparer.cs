using JustAssembly.Core.DiffItems.References;

namespace JustAssembly.Core.Comparers {
	class ReferenceComparer : BaseDiffComparer<AssemblyNameReference> {
		protected override IDiffItem GetMissingDiffItem(AssemblyNameReference element) {
			return new AssemblyReferenceDiffItem(element, null, null);
		}

		protected override IDiffItem GenerateDiffItem(AssemblyNameReference oldElement, AssemblyNameReference newElement) {
			return null;
		}

		protected override IDiffItem GetNewDiffItem(AssemblyNameReference element) {
			return null;
		}

		protected override int CompareElements(AssemblyNameReference x, AssemblyNameReference y) {
			return x.FullName.CompareTo(y.FullName);
		}

		protected override bool IsAPIElement(AssemblyNameReference element) {
			return true;
		}
	}
}
