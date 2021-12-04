using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JustAssembly.Tests {
	[TestClass]
	public class SimpleTestClass {
		[TestMethod]
		public void SimpleMonoCecilTest() {
			AssemblyDefinition thisAssembly = AssemblyDefinition.ReadAssembly("JustAssembly.Tests.dll", new ReaderParameters(GlobalAssemblyResolver.Instance));
			Assert.IsTrue(thisAssembly.MainModule.Types.Any(type => type.Name == "SimpleTestClass"));
		}
	}
}
