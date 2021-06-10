using Entitys;
using Entitys.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AdminApi
{
    public class SomeContextfactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            //optionsBuilder.UseOracle(dbConfig.ConnectionString, k=> k.UseOracleSQLCompatibility("11"));
            
            return new DataContext(optionsBuilder.Options);
        }
    }
}
