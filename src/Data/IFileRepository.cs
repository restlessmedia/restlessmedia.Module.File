using BlakeStanley.Data.DataModel;
using System.Collections.Generic;

namespace BlakeStanley.Data.Repositories
{
  public interface IFileRepository : IRepository<File>
  {
    IList<File> GetFiles(int sourceEntityGuid);
  }
}