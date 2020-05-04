using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archivist.V1
{
    public interface IFileSystemRepo
    {
        Task<byte[]> ReadAllBytesAsync(string path, System.Threading.CancellationToken cancellationToken = default);

        Task WriteAllBytesAsync(string path, byte[] bytes, System.Threading.CancellationToken cancellationToken = default);

        IEnumerable<string> GetDirectoryFilesByExtension(string directoryPath, string extension);
    }
}