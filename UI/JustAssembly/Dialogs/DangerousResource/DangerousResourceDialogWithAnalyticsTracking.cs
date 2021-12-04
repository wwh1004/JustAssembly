namespace JustAssembly.Dialogs.DangerousResource {
	internal class DangerousResourceDialogWithAnalyticsTracking : DangerousResourceDialog {
		public DangerousResourceDialogWithAnalyticsTracking(string assemblyFileName, AssemblyType assemblyType)
			: base(assemblyFileName, assemblyType) { }

		public override DangerousResourceDialogResult Show() {
			DangerousResourceDialogResult dialogResult = base.Show();

			return dialogResult;
		}
	}
}
