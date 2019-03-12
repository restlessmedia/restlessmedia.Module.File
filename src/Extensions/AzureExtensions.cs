namespace Microsoft.WindowsAzure.Storage.Blob
{
  public static class AzureExtensions
  {
    public static CloudBlobContainer GetContainer(this CloudBlobClient client, string name, bool createIfNotExists = true, bool isPublic = true)
    {
      CloudBlobContainer container = client.GetContainerReference(name);

      if (createIfNotExists)
      {
        container.CreateIfNotExists();

        if (isPublic)
        {
          MakePublic(container);
        }
      }

      return container;
    }

    public static void MakePublic(this CloudBlobContainer container)
    {
      container.SetPermissions(new BlobContainerPermissions
      {
        PublicAccess = BlobContainerPublicAccessType.Blob
      });
    }
  }
}