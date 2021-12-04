using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JustAssembly.Core.Extensions;

namespace JustAssembly.Core.DiffItems {
	abstract class BaseDiffItem : IDiffItem {
		private readonly DiffType diffType;

		public DiffType DiffType {
			get {
				return diffType;
			}
		}

		public BaseDiffItem(DiffType diffType) {
			this.diffType = diffType;
		}

		public string ToXml() {
			using (StringWriter stringWriter = new StringWriter()) {
				using (XmlWriter xmlWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented }) {
					ToXml(xmlWriter);
				}

				return stringWriter.ToString();
			}
		}

		protected abstract string GetXmlInfoString();

		internal virtual void ToXml(XmlWriter writer) {
			writer.WriteStartElement("DiffItem");
			writer.WriteAttributeString("DiffType", DiffType.ToString());
			writer.WriteString(GetXmlInfoString());
			writer.WriteEndElement();
		}

		public abstract bool IsBreakingChange { get; }
	}

	abstract class BaseDiffItem<T> : BaseDiffItem, IMetadataDiffItem<T> where T : class, IMetadataTokenProvider {
		private readonly T oldElement;
		private readonly T newElement;
		private readonly IEnumerable<IDiffItem> declarationDiffs;
		private readonly IEnumerable<IMetadataDiffItem> childrenDiffs;

		public T OldElement {
			get { return oldElement; }
		}

		public T NewElement {
			get { return newElement; }
		}

		public abstract MetadataType MetadataType { get; }

		public uint OldTokenID {
			get {
				return oldElement.MetadataToken.ToUInt32();
			}
		}

		public uint NewTokenID {
			get {
				return newElement.MetadataToken.ToUInt32();
			}
		}

		public IEnumerable<IDiffItem> DeclarationDiffs {
			get {
				return declarationDiffs;
			}
		}

		public IEnumerable<IMetadataDiffItem> ChildrenDiffs {
			get {
				return childrenDiffs;
			}
		}

		public BaseDiffItem(T oldElement, T newElement, IEnumerable<IDiffItem> declarationDiffs, IEnumerable<IMetadataDiffItem> childrenDiffs)
			: base(newElement == null ? DiffType.Deleted : (oldElement == null ? DiffType.New : DiffType.Modified)) {
			this.oldElement = oldElement;
			this.newElement = newElement;
			this.declarationDiffs = declarationDiffs != null ? declarationDiffs.ToList() : Enumerable.Empty<IDiffItem>();
			this.childrenDiffs = childrenDiffs != null ? childrenDiffs.ToList() : Enumerable.Empty<IMetadataDiffItem>();
		}

		protected T GetElement() {
			return newElement ?? oldElement;
		}

		protected abstract string GetElementShortName(T element);

		internal override void ToXml(XmlWriter writer) {
			writer.WriteStartElement(MetadataType.ToString());
			writer.WriteAttributeString("Name", GetElementShortName(GetElement()));
			writer.WriteAttributeString("DiffType", DiffType.ToString());

			if (!DeclarationDiffs.IsEmpty()) {
				writer.WriteStartElement("DeclarationDiffs");
				foreach (BaseDiffItem item in DeclarationDiffs) {
					item.ToXml(writer);
				}
				writer.WriteEndElement();
			}

			foreach (BaseDiffItem item in ChildrenDiffs) {
				item.ToXml(writer);
			}

			writer.WriteEndElement();
		}

		protected override string GetXmlInfoString() {
			throw new NotSupportedException();
		}

		private bool? isBreakingChange;
		public override bool IsBreakingChange {
			get {
				if (isBreakingChange == null) {
					if (DiffType != Core.DiffType.Modified) {
						isBreakingChange = DiffType == Core.DiffType.Deleted;
					}
					else {
						isBreakingChange = EnumerableExtensions.ConcatAll<IDiffItem>(declarationDiffs, childrenDiffs).Any(item => item.IsBreakingChange);
					}
				}
				return isBreakingChange.Value;
			}
		}
	}
}
