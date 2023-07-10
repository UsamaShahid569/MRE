using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MRE.Presistence.IProviders;
using System.IO.Compression;

namespace MRE.Presistence.Providers
{
    public class AzureStorageProvider : IAzureStorageProvider
    {
        private string azureAccessKey;
        private string azureAccessContainer;

        private readonly IConfiguration _configuration;

        public AzureStorageProvider(IConfiguration Configuration)
        {
            _configuration = Configuration;

            azureAccessKey = _configuration["AzureStorageConfig:AccessKey"];
            azureAccessContainer = _configuration["AzureStorageConfig:Container"];
        }

        public async Task<Stream> DownloadFile(string fileUrl)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(azureAccessKey);
            var blob = new CloudBlockBlob(new Uri(fileUrl), cloudStorageAccount.Credentials);

            Stream memStream = await blob.OpenReadAsync();

            return memStream;
        }

        public async Task<bool> DeleteFile(string fileUrl)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(azureAccessKey);
            var blob = new CloudBlockBlob(new Uri(fileUrl), cloudStorageAccount.Credentials);
            return await blob.DeleteIfExistsAsync();
           
        }

        public async Task SetVersion()
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(azureAccessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var properties = cloudBlobClient.GetServicePropertiesAsync().Result;
                properties.DefaultServiceVersion = "2019-12-12";
                await cloudBlobClient.SetServicePropertiesAsync(properties);
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UploadFile(IFormFile file, string dir)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(azureAccessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(azureAccessContainer);
                
                if (cloudBlobContainer.CreateIfNotExistsAsync().Result)
                {
                    cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(dir);
                cloudBlockBlob.Properties.ContentType = file.ContentType;
                
                await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

                return cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> UploadFile(ZipArchiveEntry zipFile, string dir)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(azureAccessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(azureAccessContainer);

                if (cloudBlobContainer.CreateIfNotExistsAsync().Result)
                {
                    cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(dir);
                //cloudBlockBlob.Properties.ContentType = zipFile.;

                await cloudBlockBlob.UploadFromStreamAsync(zipFile.Open());

                return cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
