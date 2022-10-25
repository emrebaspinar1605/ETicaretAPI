using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage :Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobService;
        BlobContainerClient _blobContainer;

        public AzureStorage(IConfiguration configuration)
        {
            _blobService = new(configuration["Storage:Azure"]);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainer = _blobService.GetBlobContainerClient(containerName);
            BlobClient blob = _blobContainer.GetBlobClient(fileName);
            await blob.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainer = _blobService.GetBlobContainerClient(containerName);
            return _blobContainer.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainer = _blobService.GetBlobContainerClient(containerName);
            return _blobContainer.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _blobContainer =  _blobService.GetBlobContainerClient(containerName);
            await _blobContainer.CreateIfNotExistsAsync();
            await _blobContainer.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainer)> datas = new();
            foreach (var file in files)
            {
                string fileNewName = await FileRenameAsync(containerName, file.Name, HasFile);

                BlobClient blob = _blobContainer.GetBlobClient(fileNewName);
                await blob.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{containerName}\\{fileNewName}"));
            }
            return datas;
        }

    }
}
