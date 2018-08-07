using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SSH.Core.Helper
{
    public enum ContentType
    {
        Image,
        Pdf
    }

    public class BlobStorageHelper
    {
        public const string ContentTypeImage = "image/png";
        public const string ContentTypePdf = "application/pdf";

        public static string AccountName
        {
            get
            {
                return ConfigurationManager.AppSettings["AzureBlobStorageName"];
            }
        }

        public static string Key
        {
            get
            {
                return ConfigurationManager.AppSettings["AzureBlobStorageKey"];
            }
        }

        public static string ContainerName
        {
            get
            {
                return ConfigurationManager.AppSettings["AzureBlobStorageContainer"];
            }
        }

        public static StorageCredentials _storageCredentials
        {
            get
            {
                return new StorageCredentials(AccountName, Key);
            }
        }

        public static CloudStorageAccount _account
        {
            get
            {
                return new CloudStorageAccount(_storageCredentials, true);
            }
        }

        public static CloudBlobClient _client
        {
            get
            {
                return _account.CreateCloudBlobClient();
            }
        }

        public static CloudBlobContainer _container
        {
            get
            {
                var container = _client.GetContainerReference(ContainerName);
                container.CreateIfNotExistsAsync().Wait();
                BlobContainerPermissions permissions = container.GetPermissionsAsync().Result;
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissionsAsync(permissions);
                return container;
            }
        }

        public static async Task<string> GetBlobAsync(string blobName, ContentType contentType = ContentType.Image)
        {
            if (await BlobExistsOnCloudAsync(blobName))
            {
                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
                blockBlob.Properties.ContentType = contentType.Equals(ContentType.Image) ? ContentTypeImage : ContentTypePdf;
                await blockBlob.SetPropertiesAsync();
                //return string.Format("data:{0};base64, {1}", CONTENTTYPE, await this.ConvertUriToBase64(blockBlob.Uri.AbsoluteUri));
                return await ConvertUriToBase64(blockBlob.Uri.AbsoluteUri);
            }

            return null;
        }

        public static async Task<string> GetBlobAsync(string blobName, string contentType)
        {
            if (await BlobExistsOnCloudAsync(blobName))
            {
                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
                blockBlob.Properties.ContentType = contentType;
                await blockBlob.SetPropertiesAsync();
                //return string.Format("data:{0};base64, {1}", ContentTypeImage, await this.ConvertUriToBase64(blockBlob.Uri.AbsoluteUri));
                return await ConvertUriToBase64(blockBlob.Uri.AbsoluteUri);
            }

            return null;
        }

        public static async Task<string> UploadBlobAsync(string fileName, Stream stream, ContentType contentType = ContentType.Image)
        {            
            string blobContentType = contentType.Equals(ContentType.Image) ? ContentTypeImage : ContentTypePdf;
            var newBlob = _container.GetBlockBlobReference(fileName);
            await newBlob.UploadFromStreamAsync(stream);
            return await GetBlobAsync(fileName, blobContentType);
        }

        public static async Task<bool> DeleteBlobAsync(string blobName)
        {
            if (await BlobExistsOnCloudAsync(blobName))
            {
                CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
                return blockBlob.DeleteIfExists();
            }

            return false;
        }

        public static async Task<bool> BlobExistsOnCloudAsync(string blobName)
        {
            return await _client.GetContainerReference(ContainerName)
                         .GetBlockBlobReference(blobName)
                         .ExistsAsync();
        }

        public static bool BlobExistsOnCloud(string blobName)
        {
            return _client.GetContainerReference(ContainerName)
                         .GetBlockBlobReference(blobName)
                         .Exists();
        }

        public static async Task<string> ConvertUriToBase64(string absoluteUri)
        {
            using (var httpClient = new HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(absoluteUri);
                byte[] imageBytes = ConvertStreamIntoBytes(stream);

                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static byte[] ConvertStreamIntoBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        public static string GetBlobUrl(string blobName)
        {
            if (BlobExistsOnCloud(blobName))
            {
                return ConfigurationManager.AppSettings["AzureBlobStorageUrl"] + "/" + ConfigurationManager.AppSettings["AzureBlobStorageContainer"] + "/" + blobName;
            }

            return null;
        }
    }
}
