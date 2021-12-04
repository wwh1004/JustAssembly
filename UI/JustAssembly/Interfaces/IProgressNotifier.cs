using System.Threading;
using JustAssembly.Core.Decompilation;

namespace JustAssembly.Interfaces {
	interface IProgressNotifier : IFileGenerationNotifier {
		bool IsBusy { get; set; }

		bool IsIndeterminate { get; set; }

		string LoadingMessage { get; set; }

		int Progress { get; set; }

		void Completed();

		void CancelProgress();

		CancellationToken GetCanellationToken();
	}
}
