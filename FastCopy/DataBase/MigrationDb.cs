using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FastCopy.DataBase
{
    public class MigrationDb : IMigrationDb
    {
        private readonly FastCopyDbContext m_FastCopyDbContext;
        public MigrationDb(FastCopyDbContext fastCopyDbContext)
        {
            m_FastCopyDbContext = fastCopyDbContext;
            bool res = m_FastCopyDbContext.Database.EnsureCreated();
            CreateTables();
            AlterTables();
        }

        public bool AlterTables()
        {
            List<string> sqls = new List<string>();
            sqls.Add(@"alter table DetailSetModel add column Type TEXT");
            sqls.Add(@"alter table CopyInfo add column Guid TEXT");
            sqls.Add(@"alter table CopyInfo add column ParentId TEXT");
            sqls.Add(@"alter table CopyInfo add column IsExpended INTEGER");
            sqls.Add(@"alter table CopyInfo add column Remark TEXT");
            foreach (string sql in sqls)
            {
                try
                {
                    int result = m_FastCopyDbContext.Database.ExecuteSqlRaw(sql);
                    if (result == -1)
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return true;
        }

        public async Task<int> CreateTables()
        {
            int res = 0;
            List<string> sqls = new List<string>();
            sqls.Add(string.Format(" create table if not exists CopyInfo (Id INTEGER PRIMARY KEY,IsChecked INTEGER,SourceAddress TEXT,TargetAddress TEXT,CopyTime TEXT,Status TEXT,Sort INTEGER)"));
            sqls.Add(string.Format(" create table if not exists DetailSetModel (Id INTEGER PRIMARY KEY,EName TEXT,CName TEXT,Value TEXT,Type TEXT)"));
            sqls.Add(string.Format(" create table if not exists DbVersion(Id INTEGER PRIMARY KEY,Version TEXT)"));
            sqls.Add(string.Format(" create table if not exists FtpInfo(Guid TEXT PRIMARY KEY,CopyInfoId TEXT,Ip TEXT,Path TEXT,Port INTERGER,UserName TEXT,Password TEXT,IsPassiveMode INTEGER)"));

            foreach (string sql in sqls)
            {
                res = await m_FastCopyDbContext.Database.ExecuteSqlRawAsync(sql);
            }

            return res;
        }

        public bool DeleteTables()
        {
            throw new NotImplementedException();
        }
    }
}
