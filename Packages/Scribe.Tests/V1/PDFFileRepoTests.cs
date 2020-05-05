using System.Linq;
using System.IO;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Archivist.V1;
using Scribe.V1;
using Scribe.V1.ValueObjects;

namespace ScribeTests.V1
{
    public class PDFFileRepoTests
    {
        private IFileSystemRepo FileSystem { get; set; }

        public PDFFileRepoTests()
        {
            this.FileSystem = new FileSystemRepo();
        }

        [Trait("Category", "UnitTests")]
        [Fact]
        public async void GeneratePDFData_WithAnyInput_GetsHTMLFilesOnly()
        {
            var fileSystem = new Mock<IFileSystemRepo>();

            fileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);

            fileSystem.Setup(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "Nonexistent/File" });

            var testHtml = "<h1>Here's a little HTML for ya face!</h1>";
            fileSystem.Setup(fs => fs.ReadAllBytesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Encoding.ASCII.GetBytes(testHtml)));

            var workFolder = new PDFWorkFolder(fileSystem.Object, "Nonexistent/Path");
            fileSystem.Invocations.Clear();

            var PDFRepo = new PDFFileRepo(fileSystem.Object, workFolder);

            await PDFRepo.GeneratePDFData(false);

            fileSystem.Verify(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), ".html"), Times.Once());
        }

        // The binary data of the PDF seems to be different to the comparison file, but only when run in the pipeline.
        // Run this, and other ExploratoryTests, by hand to evaluate changes to the Scribe package.
        [Trait("Category", "ExploratoryTests")]
        [Fact]
        public async void GeneratePDFData_WithKnownInput_ReturnsExpectedOutput()
        {
            var fileSystem = new Mock<IFileSystemRepo>();

            fileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);

            fileSystem.Setup(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "Nonexistent/File" });

            var testHtml = "<h1>Here's a little HTML for ya face!</h1>";
            fileSystem.Setup(fs => fs.ReadAllBytesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Encoding.ASCII.GetBytes(testHtml)));

            var startupPath = Environment.CurrentDirectory;
            var workFolder = new PDFWorkFolder(fileSystem.Object, "Nonexistent/Path");
            var PDFRepo = new PDFFileRepo(fileSystem.Object, workFolder);

            var actualData = await PDFRepo.GeneratePDFData(false);

            var expectedData = await this.FileSystem.ReadAllBytesAsync(Path.Combine(startupPath, "V1", "comparison-file.pdf"));

            Assert.True(expectedData.SequenceEqual(actualData));
        }
    }
}