using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyDiffTests.APIDiff {
	[TestClass]
	class ExistingAPIDiffTestFixture : BaseExistingAPIDiffTestFixture {
		[TestMethod]
		public void SimpleAPIDiffTest() {
			RunTestCase("SimpleAPIDIffTest");
		}

		[TestMethod]
		public void VariousAPIDiffsTest() {
			RunTestCase("VariousAPIDiffsTest");
		}
	}
}
