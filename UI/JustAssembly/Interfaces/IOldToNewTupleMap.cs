namespace JustAssembly.Interfaces {
	public interface IOldToNewTupleMap<out T>
		where T : class {
		T OldType { get; }

		T NewType { get; }

		T GetFirstNotNullItem();
	}
}
