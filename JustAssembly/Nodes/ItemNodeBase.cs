using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ICSharpCode.TreeView;
using JustAssembly.Interfaces;
using JustAssembly.Nodes.APIDiff;

namespace JustAssembly.Nodes {
	abstract class ItemNodeBase : SharpTreeNode {
		private bool breakingChangesOnly;

		protected readonly APIDiffInfo apiDiffInfo;

		protected DifferenceDecoration differenceDecoration = DifferenceDecoration.NoDifferences;

		public readonly ItemNodeBase ParentNode;

		public ItemNodeBase(string name, FilterSettings filterSettings) {
			if (name == null) {
				throw new ArgumentNullException("name");
			}
			Name = name;
			LazyLoading = true;
			Children = new ObservableCollection<ItemNodeBase>();
			Children.CollectionChanged += OnChildrenCollectionChanged;
			FilterSettings = filterSettings;

			RaisePropertyChanged("DifferenceDecoration");
		}

		public ItemNodeBase(string name, ItemNodeBase parentNode, FilterSettings filterSettings)
			: this(name, filterSettings) {
			ParentNode = parentNode;
		}

		public ItemNodeBase(string name, ItemNodeBase parentNode, APIDiffInfo apiDiffInfo, FilterSettings filterSettings)
			: this(name, parentNode, filterSettings) {
			this.apiDiffInfo = apiDiffInfo;
		}

		public abstract NodeType NodeType { get; }

		public IOldToNewTupleMap<GeneratedProjectOutputInfo> GenerationProjectInfoMap { get; set; }

		public string Name { get; private set; }

		public abstract string FullName { get; }

		public DifferenceDecoration DifferenceDecoration {
			get { return differenceDecoration; }
		}

		public override object Text {
			get {
				return Name;
			}
		}

		protected bool CanUseParentDiffDecoration {
			get {
				return ParentNode != null &&
					  (ParentNode.DifferenceDecoration == DifferenceDecoration.Added ||
					   ParentNode.DifferenceDecoration == DifferenceDecoration.Deleted);
			}
		}

		public FilterSettings FilterSettings { get; set; }

		public new ObservableCollection<ItemNodeBase> Children { get; private set; }

		public virtual void ReloadChildren() {
			base.Children.Clear();
			foreach (ItemNodeBase child in Children) {
				if (child.ShouldBeShown(FilterSettings)) {
					base.Children.Add(child);
					child.ReloadChildren();
				}
			}
		}

		protected virtual void OnChildrenLoaded() {
			ChildrenLoaded(this, EventArgs.Empty);
		}

		protected abstract DifferenceDecoration GetDifferenceDecoration();

		protected bool ApiOnlyFilter(IOldToNewTupleMap<MemberMetadataBase> tuple) {
			return apiDiffInfo == null || tuple.OldType != null && tuple.OldType.IsPublic || tuple.NewType != null && tuple.NewType.IsPublic;
		}

		public event EventHandler ChildrenLoaded = delegate { };

		public bool BreakingChangesOnly {
			get {
				if (ParentNode != null) {
					breakingChangesOnly = ParentNode.BreakingChangesOnly;
				}
				return breakingChangesOnly;
			}
			set => breakingChangesOnly = value;
		}

		public void RefreshDecoration() {
			DifferenceDecoration old = differenceDecoration;
			foreach (ItemNodeBase item in Children) {
				item.RefreshDecoration();
			}

			differenceDecoration = GetDifferenceDecoration();
			if (differenceDecoration != old) {
				RaisePropertyChanged("DifferenceDecoration");
			}
		}

		public bool ShouldBeShown(FilterSettings settings) {
			if (settings.ShowUnmodified) {
				return true;
			}
			else if (DifferenceDecoration != DifferenceDecoration.NoDifferences) {
				return true;
			}

			return false;
		}

		private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if (e.Action == NotifyCollectionChangedAction.Add) {
				foreach (ItemNodeBase child in e.NewItems) {
					if (child.ShouldBeShown(FilterSettings)) {
						base.Children.Add(child);
					}
				}
			}
		}
	}
}
