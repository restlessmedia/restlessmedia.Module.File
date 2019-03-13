using Autofac;
using restlessmedia.Module.File.Configuration;

namespace restlessmedia.Module.File
{
  public class Module : IModule
  {
    public void RegisterComponents(ContainerBuilder containerBuilder)
    {
      containerBuilder.RegisterType<FileSystemStorageProvider>().As<IDiskStorageProvider>().SingleInstance();
      containerBuilder.RegisterType<FileService>().As<IFileService>().SingleInstance();
      containerBuilder.RegisterSettings<IAzureSettings>("restlessmedia/azure", required: true);
      containerBuilder.RegisterSettings<IFileSettings>("restlessmedia/file", required: true);
    }
  }
}