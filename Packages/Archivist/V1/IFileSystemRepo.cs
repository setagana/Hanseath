using System;
using System.Threading.Tasks;
using System.IO;

namespace Archivist.V1
{
    public interface IFileSystemRepo
    {
        Task<byte[]> ReadAllBytesAsync(string path, System.Threading.CancellationToken cancellationToken = default);

        Task WriteAllBytesAsync (string path, byte[] bytes, System.Threading.CancellationToken cancellationToken = default);
    }
}