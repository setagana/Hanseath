using System.Linq;
using System.IO;
using System;
using Archivist.V1;
using Domain.V1;
using Scribe.Exceptions;
using System.Collections.Generic;

namespace Scribe.V1.ValueObjects
{
    public class PDFWorkFolder : ValueObject
    {
        private IFileSystemRepo FileSystemRepo { get; set; }
        public string Path { get; private set; }

        public PDFWorkFolder(IFileSystemRepo fileSystemRepo, string path)
        {
            this.FileSystemRepo = fileSystemRepo;

            this.ValidatePath(path);

            this.Path = path;
        }

        private void ValidatePath(string path)
        {
            if (!this.FileSystemRepo.DirectoryExists(path))
            {
                throw new InvalidWorkFolderException($"The provided path '{path}' does not exist or is not a directory.");
            }

            var htmlFiles = this.FileSystemRepo.GetDirectoryFilesByExtension(path, ".html");
            if (htmlFiles.Count() > 1 || htmlFiles.Count() == 0)
            {
                throw new InvalidWorkFolderException($"A valid working folder should contain a single HTML file. '{path}' contains {htmlFiles.Count()}");
            }
        }

        public static explicit operator string(PDFWorkFolder folder)
        {
            return folder.ToString();
        }

        public override String ToString()
        {
            return this.Path;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Path;
        }
    }
}