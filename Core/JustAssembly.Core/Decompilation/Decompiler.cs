using System;
using System.Collections.Generic;
using System.Threading;

namespace JustAssembly.Core.Decompilation {
	public static class Decompiler {
		public static bool IsValidCLRAssembly(string assemblyFilePath) {
			throw new NotImplementedException();
		}

		public static ICollection<uint> GetAssemblyModules(string assemblyFilePath) {
			throw new NotImplementedException();
		}

		public static string GetModuleName(string assemblyFilePath, uint moduleToken) {
			throw new NotImplementedException();
		}

		public static ICollection<uint> GetModuleTypes(string assemblyFilePath, uint moduleToken) {
			throw new NotImplementedException();
		}

		public static ICollection<Tuple<MemberType, uint>> GetTypeMembers(string assemblyFilePath, uint moduleToken, uint typeToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static string GetTypeNamespace(string assemblyFilePath, uint moduleToken, uint typeToken) {
			throw new NotImplementedException();
		}

		public static byte[] GetImageData(string assemblyFilePath, uint moduleToken) {
			throw new NotImplementedException();
		}

		public static string GetTypeName(string assemblyFilePath, uint moduleToken, uint typeToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static string GetTypeFullName(string assemblyFilePath, uint moduleToken, uint typeToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static string GetMemberName(string assemblyFilePath, uint moduleToken, uint typeToken, uint memberToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static ICollection<IPropertyMethod> GetPropertyMethods(string assemblyFilePath, uint moduleToken, uint typeToken, uint propertyToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static ICollection<IEventMethod> GetEventMethods(string assemblyFilePath, uint moduleToken, uint typeToken, uint eventToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static AccessModifier GetTypeAccessModifier(string assemblyFilePath, uint moduleToken, uint typeToken) {
			throw new NotImplementedException();
		}

		public static AccessModifier GetMemberAccessModifier(string assemblyFilePath, uint moduleToken, uint typeToken, uint memberToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static ICollection<string> GetTypeAttributes(string assemblyFilePath, uint moduleToken, uint typeToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static ICollection<string> GetMemberAttributes(string assemblyFilePath, uint moduleToken, uint typeToken, uint memberToken, SupportedLanguage language) {
			throw new NotImplementedException();
		}

		public static int GetMaximumPossibleTargetPathLength(string assemblyFilePath, SupportedLanguage language, bool decompileDangerousResources) {
			throw new NotImplementedException();
		}

		public static IAssemblyDecompilationResults GenerateFiles(string assemblyFilePath, AssemblyDefinition assembly, string targetPath, SupportedLanguage language, CancellationToken cancellationToken, bool decompileDangerousResources, IFileGenerationNotifier notifier = null) {
			throw new NotImplementedException();
		}
	}
}
