using JustAssembly.Core.Comparers;

namespace JustAssembly.Core {
	public class APIDiffHelper {
		public static IMetadataDiffItem GetAPIDifferences(string oldAssemblyPath, string newAssemblyPath) {
			if (oldAssemblyPath == null || newAssemblyPath == null) {
				return null;
			}

			AssemblyDefinition oldAssembly = GlobalAssemblyResolver.Instance.GetAssemblyDefinition(oldAssemblyPath);
			AssemblyDefinition newAssembly = GlobalAssemblyResolver.Instance.GetAssemblyDefinition(newAssemblyPath);

			if (oldAssembly == null || newAssembly == null) {
				return null;
			}

			return GetAPIDifferences(oldAssembly, newAssembly);
		}

		internal static IMetadataDiffItem<AssemblyDefinition> GetAPIDifferences(AssemblyDefinition oldAssembly, AssemblyDefinition newAssembly) {
			return new AssemblyComparer(oldAssembly, newAssembly).Compare();
		}
	}
}
