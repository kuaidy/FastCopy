using FastCopy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.DataBase
{
    public class FastCopyDbContext : DbContext
    {
        public DbSet<CopyInfoModel> CopyInfos { get; set; }
        public DbSet<DetailSetModel> DetailSetModels { get; set; }
        public DbSet<FtpInfoModel> FtpInfos { get; set; }

        public string DbPath { get; }

        public string DbConnectionString { get; set; }

        public FastCopyDbContext()
        {
            string folder = Environment.CurrentDirectory + "\\data";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string dbFile = folder + "\\fastcopy.db";
            DbConnectionString = "Data Source=" + dbFile;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlite(DbConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CopyInfoModel>().ToTable("CopyInfo");
            modelBuilder.Entity<DetailSetModel>().ToTable("DetailSetModel");
            modelBuilder.Entity<FtpInfoModel>().ToTable("FtpInfo");
        }
    }
}
