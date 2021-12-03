using System.Windows.Input;
using JustAssembly.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using JustAssembly.Controls;

namespace JustAssembly.ViewModels
{
    abstract class ComparisonSessionViewModelBase : BindableBase, IOldToNewTupleMap<string>, IComparisonSessionModel
    {
        private string oldPath;
        private string newPath;

        public ICommand SwapPathsCommand { get; private set; }

        public ComparisonSessionViewModelBase(string header)
        {
            this.SwapPathsCommand = new DelegateCommand(OnSwapPathsCommandExecuted);

            this.Header = header;
        }

        public abstract SelectedItemType SelectedItemType { get; }

        public string Header { get; private set; }

        public string OldType
        {
            get
            {
                return this.oldPath;
            }
            set
            {
                if (this.oldPath != value)
                {
                    this.oldPath = value;

                    this.RaisePropertyChanged("OldType");

                    this.RaisePropertyChanged("IsLoadEnabled");
                }
            }
        }

        public string NewType
        {
            get
            {
                return this.newPath;
            }
            set
            {
                if (this.newPath != value)
                {
                    this.newPath = value;

                    this.RaisePropertyChanged("NewType");

                    this.RaisePropertyChanged("IsLoadEnabled");
                }
            }
        }

        public bool IsLoadEnabled
        {
            get { return GetLoadButtonState(); }
        }

        protected abstract bool GetLoadButtonState();

        public abstract ITabSourceItem GetTabSourceItem();

        private void OnSwapPathsCommandExecuted()
        {
            string tempPath = this.OldType;

            this.OldType = this.NewType;

            this.NewType = tempPath;
        }

        public string GetFirstNotNullItem()
        {
            return OldType ?? NewType;
        }
    }
}