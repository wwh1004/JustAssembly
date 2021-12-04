using JustAssembly.Core.Decompilation;
using JustAssembly.Interfaces;
using JustAssembly.Nodes.APIDiff;

namespace JustAssembly.Nodes {
	class MemberNode : DecompiledMemberNodeBase {
		private readonly IOldToNewTupleMap<MemberMetadata> membersMap;

		public MemberNode(IOldToNewTupleMap<MemberMetadata> membersMap, ItemNodeBase parent, APIDiffInfo apiDiffInfo, FilterSettings filterSettings)
			: base(membersMap.GetFirstNotNullItem().GetSignature(), parent, apiDiffInfo, filterSettings) {
			this.membersMap = membersMap;

			differenceDecoration = GetDifferenceDecoration();

			// This node has no child items, so this removes the expander.
			LazyLoading = false;
		}

		public override object Icon {
			get {
				MemberMetadata member = membersMap.GetFirstNotNullItem();

				switch (member.MemberType) {
				case MemberType.Event:
					return ImagesResourceStrings.EventNode;

				case MemberType.Field:
					return ImagesResourceStrings.FieldNode;

				case MemberType.Method:
					return ImagesResourceStrings.MethodNode;

				case MemberType.Property:
					return ImagesResourceStrings.PropertyNode;

				default:
					return ImagesResourceStrings.TypeNode;
				}
			}
		}

		public override NodeType NodeType {
			get { return NodeType.MemberDefinition; }
		}

		public override string FullName {
			get { return string.Format("{0}.{1}", ParentNode.FullName, Name); }
		}

		public override MemberDefinitionMetadataBase OldMemberMetada {
			get { return membersMap.OldType; }
		}

		public override MemberDefinitionMetadataBase NewMemberMetada {
			get { return membersMap.NewType; }
		}

		protected override DifferenceDecoration GetDifferenceDecoration() {
			if (CanUseParentDiffDecoration) {
				return ParentNode.DifferenceDecoration;
			}

			if (apiDiffInfo != null) {
				return apiDiffInfo.APIDiffItem.GetDifferenceDecoration(BreakingChangesOnly);
			}


			if (membersMap.OldType == null) {
				return DifferenceDecoration.Added;
			}
			else if (membersMap.NewType == null) {
				return DifferenceDecoration.Deleted;
			}
			else if (ParentNode.DifferenceDecoration == DifferenceDecoration.Modified) {
				IOffsetSpan offsetSpanL = null;
				IOffsetSpan offsetSpanR = null;

				bool containsValues = OldDecompileResult.MemberTokenToDecompiledCodeMap.TryGetValue(membersMap.OldType.TokenId, out offsetSpanL) &&
									  NewDecompileResult.MemberTokenToDecompiledCodeMap.TryGetValue(membersMap.NewType.TokenId, out offsetSpanR);

				if (containsValues) {
					return GetMemberSource(OldDecompileResult, membersMap.OldType) == GetMemberSource(NewDecompileResult, membersMap.NewType) ?
						DifferenceDecoration.NoDifferences : DifferenceDecoration.Modified;
				}
			}
			return DifferenceDecoration.NoDifferences;
		}
	}
}
