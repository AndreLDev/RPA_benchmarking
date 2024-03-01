using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CrawlerContext : DbContext
{
    public DbSet<Log> Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=PC03LAB2538\\SENAI;Database=WebScrapingDb2;User Id=sa;Password=senai.123;"); // Substitua "YourConnectionString" pela sua string de conexão
    }
}
