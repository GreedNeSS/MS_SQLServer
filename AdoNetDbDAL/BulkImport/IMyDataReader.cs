using System.Data;

namespace AdoNetDbDAL.BulkImport
{
    public interface IMyDataReader<T> : IDataReader
    {
            List<T> Records { get; set; }
    }
}