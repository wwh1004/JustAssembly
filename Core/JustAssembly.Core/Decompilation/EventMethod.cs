namespace JustAssembly.Core.Decompilation {
	class EventMethod : IEventMethod {
		public EventMethodType EventMethodType { get; private set; }

		public uint EventMethodToken { get; private set; }

		public EventMethod(EventMethodType eventMethodType, uint eventMethodToken) {
			EventMethodType = eventMethodType;
			EventMethodToken = eventMethodToken;
		}
	}
}
