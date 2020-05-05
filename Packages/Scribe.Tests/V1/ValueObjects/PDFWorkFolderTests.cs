using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Archivist.V1;
using Scribe.Exceptions;
using Scribe.V1.ValueObjects;

namespace ScribeTests.V1.ValueObjects
{
    public class PDFWorkFolderTests
    {
        [Trait("Category", "UnitTests")]
        [Fact]
        public void Constructor_WithValidPath_ReturnsObject()
        {
            var foundFiles = new List<string> { "/a/nonexistent/file" };
            var fileSystem = new Mock<IFileSystemRepo>();
            fileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);
            fileSystem.Setup(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(foundFiles);

            var PDFWorkFolder = new PDFWorkFolder(fileSystem.Object, "TestValue");

            Assert.Equal("TestValue", PDFWorkFolder.Path, StringComparer.OrdinalIgnoreCase);
        }

        [Trait("Category", "UnitTests")]
        [Fact]
        public void Constructor_WithNonExistentPath_Throws()
        {
            var fileSystem = new Mock<IFileSystemRepo>();
            fileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(false);

            Assert.Throws<InvalidWorkFolderException>(() => new PDFWorkFolder(fileSystem.Object, "TestValue"));
        }

        [Trait("Category", "UnitTests")]
        [Fact]
        public void Constructor_WithMultipleHTMLFilesInFolder_Throws()
        {
            var foundFiles = new List<string>{
                "/a/nonexistent/file",
                "/another/nonexistent/file"
            };
            var fileSystem = new Mock<IFileSystemRepo>();
            fileSystem.Setup(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(foundFiles);

            Assert.Throws<InvalidWorkFolderException>(() => new PDFWorkFolder(fileSystem.Object, "TestValue"));
        }

        [Trait("Category", "UnitTests")]
        [Fact]
        public void Constructor_WithZeroHTMLFilesInFolder_Throws()
        {
            var foundFiles = new List<string>();
            var fileSystem = new Mock<IFileSystemRepo>();
            fileSystem.Setup(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(foundFiles);

            Assert.Throws<InvalidWorkFolderException>(() => new PDFWorkFolder(fileSystem.Object, "TestValue"));
        }

        [Trait("Category", "UnitTests")]
        [Fact]
        public void Constructor_WithAnyPath_ChecksForHTMLFilesOnly()
        {
            var foundFiles = new List<string> { "/a/nonexistent/file" };
            var fileSystem = new Mock<IFileSystemRepo>();
            fileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(true);
            fileSystem.Setup(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(foundFiles);

            var PDFWorkFolder = new PDFWorkFolder(fileSystem.Object, "TestValue");

            fileSystem.Verify(fs => fs.GetDirectoryFilesByExtension(It.IsAny<string>(), ".html"), Times.Once());
        }
    }
}