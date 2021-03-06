﻿using restlessmedia.Module.Data;
using restlessmedia.Module.Data.EF;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace restlessmedia.Module.File.Data
{
  public class DatabaseContext : restlessmedia.Module.Data.EF.DatabaseContext
  {
    public DatabaseContext(IDataContext dataContext, bool autoDetectChanges)
      : base(dataContext, autoDetectChanges) { }

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
      modelBuilder.Configurations.Add(new EntityTypeConfiguration<VEntityFile>());
    }
  }
}