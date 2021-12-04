using System.Threading;
using JustAssembly.Core.Decompilation;
using Prism.Mvvm;

namespace JustAssembly.Interfaces {
	abstract class TabSourceItemBase : BindableBase, ITabSourceItem {
		private bool isBusy;

		protected string header;

		protected int progress;

		private bool isIndeterminite;

		private uint totalFileCount;

		private string loadingMessage;

		private CancellationTokenSource cancellationTokenSource;

		public abstract void OnProjectFileGenerated(IFileGeneratedInfo args);

		public abstract string ToolTip { get; }

		public uint TotalFileCount {
			get => totalFileCount;
			set {
				if (totalFileCount != value) {
					totalFileCount = value;
					RaisePropertyChanged("TotalFileCount");
				}
			}
		}

		public string LoadingMessage {
			get => loadingMessage;
			set {
				if (loadingMessage != value) {
					loadingMessage = value;
					RaisePropertyChanged("LoadingMessage");
				}
			}
		}

		/// <summary>
		/// Clears up the state of the progress tracking properties, so that they can be reused the next time they're needed.
		/// </summary>
		public void Completed() {
			IsBusy = false;
			Progress = 0;
			TotalFileCount = 0;
		}

		public CancellationToken GetCanellationToken() {
			cancellationTokenSource = new CancellationTokenSource();

			return cancellationTokenSource.Token;
		}

		public void CancelProgress() {
			if (cancellationTokenSource != null) {
				cancellationTokenSource.Cancel();
			}
			Completed();
		}

		public abstract TabKind TabKind { get; }

		public virtual string Header {
			get {
				return header;
			}
		}

		public bool IsBusy {
			get => isBusy;
			set {
				if (isBusy != value) {
					isBusy = value;

					RaisePropertyChanged("IsBusy");
				}
			}
		}

		public bool IsIndeterminate {
			get => isIndeterminite;
			set {
				if (isIndeterminite != value) {
					isIndeterminite = value;
					RaisePropertyChanged("IsIndeterminate");
				}
			}
		}

		public int Progress {
			get {
				if (TotalFileCount == 0) {
					return 0;
				}
				return (progress * 100) / (int)TotalFileCount;
			}
			set {
				int oldProgress = Progress;
				if (progress != value) {
					progress = value;
				}
				if (oldProgress != Progress) {
					// update the progress only when enough items were generated, so that the bar will increase with at least 1%
					// (i.e. dont raise property changed for each item, when generating 5000 files, instead raise it on every 50th)
					RaisePropertyChanged("Progress");
				}
			}
		}

		public abstract void LoadContent();

		public virtual void ReloadContent() {
			LoadContent();
		}

		public virtual void Dispose() {
		}
	}
}
