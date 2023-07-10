using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Presistence.IProviders
{
   public interface IAzureStorageProvider
    {
        Task<Stream> DownloadFile(string fileUrl);
        Task<string> UploadFile(IFormFile file, string dir);
        Task<bool> DeleteFile(string fileUrl);
        Task<string> UploadFile(ZipArchiveEntry zipFile, string dir);
        Task SetVersion();
    }
}
