using System.Collections.Generic;
using JustAssembly.Interfaces;

namespace JustAssembly.MergeUtilities {
	abstract class MergeManagerBase<IInput, IOutput>
		where IInput : class
		where IOutput : class {
		protected readonly IOldToNewTupleMap<IInput> tupleMap;

		public MergeManagerBase(IOldToNewTupleMap<IInput> typesTupleMap) {
			tupleMap = typesTupleMap;
		}

		public abstract IEnumerable<IOldToNewTupleMap<IOutput>> GetMergedCollection();
	}
}
