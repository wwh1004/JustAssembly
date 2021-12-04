using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace JustAssembly.Infrastructure {
	class JustAssemblyViewModel : BindableBase {
		private string selectedJustAssembly;

		private readonly IDictionary<string, JustAssemblyerences> diffMap;

		public JustAssemblyViewModel() {
			diffMap = new Dictionary<string, JustAssemblyerences>
			{
				{"All kinds of API changes", JustAssemblyerences.All},
				{"Public API without version changes", JustAssemblyerences.AllPublicApi},
				{"Public API breaking changes", JustAssemblyerences.PublicApiBreakingChanges},
			};
			SelectedJustAssembly = JustAssemblys.First();
		}

		public string SelectedJustAssembly {
			get => selectedJustAssembly;
			set {
				if (selectedJustAssembly != value) {
					selectedJustAssembly = value;

					RaisePropertyChanged("SelectedJustAssembly");
				}
			}
		}

		public IEnumerable<string> JustAssemblys {
			get { return diffMap.Keys; }
		}

		public JustAssemblyerences GetSelectedJustAssembly() {
			return diffMap[SelectedJustAssembly];
		}
	}

	enum JustAssemblyerences : int {
		All,
		AllPublicApi,
		PublicApiBreakingChanges
	}
}
