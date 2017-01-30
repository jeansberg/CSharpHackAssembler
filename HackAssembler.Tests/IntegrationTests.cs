using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HackAssembler.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        Assembler testAssembler;

        [TestInitialize]
        public void Initialize()
        {
            testAssembler = new Assembler(new BasicParser(), new BasicCodeGenerator());
        }

        [TestMethod]
        public void AddMatchesCompareFile()
        {
            CompareOutput("add/Add.asm");
        }

        [TestMethod]
        public void MaxMatchesCompareFile()
        {
            CompareOutput("max/Max.asm");
        }

        [TestMethod]
        public void RectMatchesCompareFile()
        {
            CompareOutput("rect/Rect.asm");
        }

        [TestMethod]
        public void PongMatchesCompareFile()
        {
            CompareOutput("pong/Pong.asm");
        }

        private void CompareOutput(string programPath)
        {
            var inputFile = File.ReadAllText(programPath);

            var programFolder = Path.GetDirectoryName(programPath);

            var programName = Path.GetFileNameWithoutExtension(programPath);

            testAssembler.Assemble(programName, inputFile);

            var outputData = File.ReadAllText(Path.ChangeExtension(programName, "hack"));

            var comparisonData = File.ReadAllText(Path.ChangeExtension(programPath, "cmp"));

            Assert.AreEqual(comparisonData, outputData);
        }
    }
}
