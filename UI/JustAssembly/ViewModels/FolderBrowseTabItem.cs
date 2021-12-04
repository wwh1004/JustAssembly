using System;
using JustAssembly.Interfaces;
using JustAssembly.Nodes;

namespace JustAssembly.ViewModels {
	class FolderBrowseTabItem : BrowserTabSourceItemBase {
		public FolderBrowseTabItem(IOldToNewTupleMap<string> tupleMap) {
			FilterSettings filterSettings = new FilterSettings(ShowAllUnmodified);
			nodes = new ItemNodeBase[] {
				new FolderNode(tupleMap, null, null, this, filterSettings, true)
			};

			header = GetTabTitle(tupleMap);
			toolTip = GetTabToolTip(tupleMap);
		}

		public override TabKind TabKind {
			get { return TabKind.DirectoryBrowse; }
		}

		public override void LoadContent() {
		}

		public override void ReloadContent() {
			nodes[currentNode].ReloadChildren();
		}

		private void OnFolderNodeChildrenLoaded(object sender, EventArgs e) {
		}

		public override void Dispose() {
			CancelProgress();
		}

		public override void OnProjectFileGenerated(JustAssembly.Core.Decompilation.IFileGeneratedInfo args) {
			Progress = progress + 1;
		}
	}
}
