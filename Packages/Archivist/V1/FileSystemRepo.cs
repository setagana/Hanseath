using System;
using System.Threading.Tasks;
using System.IO;

namespace Archivist.V1
{
    public class FileSystemRepo : IFileSystemRepo
    {
        public Task<byte[]> ReadAllBytesAsync(string path, System.Threading.CancellationToken cancellationToken = default)
        {
            return File.ReadAllBytesAsync(path, cancellationToken);
        }

        public Task WriteAllBytesAsync (string path, byte[] bytes, System.Threading.CancellationToken cancellationToken = default)
        {
            return File.WriteAllBytesAsync(path, bytes, cancellationToken);
        }
    }
}