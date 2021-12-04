using System.Collections.Generic;
using JustAssembly.Core.DiffItems.Events;

namespace JustAssembly.Core.Comparers.Accessors {
	class RemoveAccessorComparer : BaseAccessorComparer<EventDefinition> {
		public RemoveAccessorComparer(EventDefinition oldEvent, EventDefinition newEvent)
			: base(oldEvent, newEvent) {
		}

		protected override MethodDefinition SelectAccessor(EventDefinition element) {
			return element.RemoveMethod;
		}

		protected override IMetadataDiffItem<MethodDefinition> CreateAccessorDiffItem(IEnumerable<IDiffItem> declarationDiffs) {
			return new RemoveAccessorDiffItem(oldElement, newElement, declarationDiffs);
		}
	}
}
