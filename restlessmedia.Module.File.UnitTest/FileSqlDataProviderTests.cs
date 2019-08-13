using FakeItEasy;
using restlessmedia.Module.Data;
using restlessmedia.Module.File.Data;
using System;

namespace restlessmedia.Module.File.UnitTest
{
  public class FileSqlDataProviderTests
  {
    public FileSqlDataProviderTests()
    {
      _dataContext = A.Fake<IDataContext>();
      _dataProvider = new FileSqlDataProvider(_dataContext);

      A.CallTo(() => _dataContext.ConnectionFactory).Returns(new IntegrationTestConnetionFactory());
      A.CallTo(() => _dataContext.LicenseSettings.LicenseKey).Returns(new Guid(licenseKey));
    }

    //[Fact]
    public void Save_creates_record()
    {
      FileEntity file = new FileEntity
      {
        FileName = "test2.jpg",
        SystemFileName = "test2.jpg"
      };

      //  A.CallTo(() => _dataContext.ConnectionFactory.CreateConnection()).Invokes<SqlConnection>(() =>
      //{
      //  return new SqlConnection(connectionString);
      //});


      //(new SqlConnection(connectionString));


      FileEntity fileEntity = _dataProvider.Read(54552); 

      _dataProvider.Save(EntityType.Property, 21643, file);

      fileEntity = _dataProvider.Read(file.FileId.Value);

      _dataProvider.Delete(fileEntity.FileId.Value);
    }

    //[Fact]
    public void list_files()
    {
      var list = _dataProvider.List(EntityType.Property, 21643);
    }

    //[Fact]
    public void read_file()
    {
      var file = _dataProvider.Read(35866);
    }

    private readonly FileSqlDataProvider _dataProvider;

    private readonly IDataContext _dataContext;

    private const string connectionString = @"Server=.\SQLEXPRESS;database=restlessmedia.UnitTest;Trusted_Connection=Yes;APP=PC";

    private const string licenseKey = "BF812A79-9AE3-48F7-8210-FDA1DF411318";
  }
}
