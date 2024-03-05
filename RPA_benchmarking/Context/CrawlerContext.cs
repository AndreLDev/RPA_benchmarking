using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CrawlerContext : DbContext
{
    public DbSet<Log> LOGROBO { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=SQL9001.site4now.net; Initial Catalog=db_aa5b20_apialmoxarifado; User id=db_aa5b20_apialmoxarifado_admin; Password=master@123");
    }
}
