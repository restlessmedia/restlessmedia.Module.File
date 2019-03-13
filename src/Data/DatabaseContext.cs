using restlessmedia.Module.Data;
using restlessmedia.Module.Data.EF;
using System.Data.Entity;

namespace restlessmedia.Module.File.Data
{
  public class DatabaseContext : restlessmedia.Module.Data.EF.DatabaseContext
  {
    public DatabaseContext(IDataContext dataContext, bool autoDetectChanges)
      : base(dataContext, autoDetectChanges)
    {
      _dataContext = dataContext;
    }

    public DbSet<VEntityFile> EntityFile
    {
      get
      {
        return Set<VEntityFile>();
      }
    }

    protected override void Configure(DbModelBuilder modelBuilder)
    {
      base.Configure(modelBuilder);

      modelBuilder.Configurations.Add(new LicensedEntityConfiguration<VFile>());
    }

    private readonly IDataContext _dataContext;
  }
}