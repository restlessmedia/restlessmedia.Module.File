using restlessmedia.Module.Data;
using System.Data.Entity;

namespace restlessmedia.Module.File.Data
{
  public class DatabaseContext : Module.Data.EF.DatabaseContext
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

    private readonly IDataContext _dataContext;
  }
}