using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Archivist.V1
{
    public class FileSystemRepo : IFileSystemRepo
    {
        public Task<byte[]> ReadAllBytesAsync(string path, System.Threading.CancellationToken cancellationToken = default)
        {
            return File.ReadAllBytesAsync(path, cancellationToken);
        }

        public Task WriteAllBytesAsync(string path, byte[] bytes, System.Threading.CancellationToken cancellationToken = default)
        {
            return File.WriteAllBytesAsync(path, bytes, cancellationToken);
        }

        public IEnumerable<string> GetDirectoryFilesByExtension(string directoryPath, string extension)
        {
            return Directory
                .EnumerateFiles(directoryPath)
                .Where(filePath => String.Equals(System.IO.Path.GetExtension(filePath), extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}