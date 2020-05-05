using System.Text;
using System;
using System.Linq;
using System.Threading.Tasks;
using Archivist.V1;
using IronPdf;
using Scribe.V1.ValueObjects;

namespace Scribe.V1
{
    public class PDFFileRepo
    {
        private IFileSystemRepo File { get; set; }
        private PDFWorkFolder Folder { get; set; }

        public PDFFileRepo(IFileSystemRepo fileSystemRepo, PDFWorkFolder folder)
        {
            this.File = fileSystemRepo;
            this.Folder = folder;
        }

        public async Task<byte[]> GeneratePDFData(bool includeAssets)
        {
            var htmlFilePath = this.File.GetDirectoryFilesByExtension(this.Folder.Path, ".html")
                .First();
            var htmlFileData = await this.File.ReadAllBytesAsync(htmlFilePath);
            var htmlString = Encoding.UTF8.GetString(htmlFileData);

            var renderer = new IronPdf.HtmlToPdf();

            PdfDocument PDF;

            if (includeAssets)
            {
                PDF = renderer.RenderHtmlAsPdf(htmlString, this.Folder.Path);
                return PDF.BinaryData;
            }

            PDF = renderer.RenderHtmlAsPdf(htmlString);
            return PDF.BinaryData;
        }
    }
}