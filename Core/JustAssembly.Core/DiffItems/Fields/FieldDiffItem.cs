using System.Collections.Generic;

namespace JustAssembly.Core.DiffItems.Fields {
	class FieldDiffItem : BaseMemberDiffItem<FieldDefinition> {
		public FieldDiffItem(FieldDefinition oldField, FieldDefinition newField, IEnumerable<IDiffItem> declarationDiffs)
			: base(oldField, newField, declarationDiffs, null) {
		}

		public override MetadataType MetadataType {
			get { return Core.MetadataType.Field; }
		}
	}
}
