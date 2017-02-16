using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HackAssembler.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        Assembler testAssembler;
        JsonLookupHandler testConverter;

        [TestInitialize]
        public void Initialize()
        {
            var testLookupHandler = new JsonLookupHandler();
            testAssembler = new Assembler(new BasicParser(testLookupHandler), new BasicCodeGenerator(testLookupHandler));
            testConverter = new JsonLookupHandler("");
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

        // Assembles a program from a specified .asm file and compares the output in the .hack fileto a pregenerated .cmp file
        // The files are expected to have the same name
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
