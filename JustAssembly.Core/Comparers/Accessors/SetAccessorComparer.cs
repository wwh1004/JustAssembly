using System.Collections.Generic;
using JustAssembly.Core.DiffItems.Properties;

namespace JustAssembly.Core.Comparers.Accessors {
	class SetAccessorComparer : BaseAccessorComparer<PropertyDefinition> {
		public SetAccessorComparer(PropertyDefinition oldProperty, PropertyDefinition newProperty)
			: base(oldProperty, newProperty) {
		}

		protected override MethodDefinition SelectAccessor(PropertyDefinition element) {
			return element.SetMethod;
		}

		protected override IMetadataDiffItem<MethodDefinition> CreateAccessorDiffItem(IEnumerable<IDiffItem> declarationDiffs) {
			return new SetAccessorDiffItem(oldElement, newElement, declarationDiffs);
		}
	}
}
