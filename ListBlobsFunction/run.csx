#r "Azure.Storage.Blobs"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string folderName = req.Query["folderName"];

    string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("filehost");

    List<string> blobNames = new List<string>();
    await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, folderName))
    {
        blobNames.Add(blobItem.Name);
    }

    return new OkObjectResult(blobNames);
}