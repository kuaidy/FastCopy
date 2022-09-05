using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.DataBase
{
    public class SqliteHelper
    {
        public static FastCopyDbContext FastCopyDbContext;

        public static void Init()
        {
            try
            {
                if (FastCopyDbContext == null)
                {
                    FastCopyDbContext = new FastCopyDbContext();
                }
                //FastCopyDbContext.Database.EnsureCreated();
                FastCopyDbContext.Database.Migrate();
            }
            catch (Exception ex) 
            {

            }
        }
    }
}
