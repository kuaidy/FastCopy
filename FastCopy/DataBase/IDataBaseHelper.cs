using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.DataBase
{
    public interface IDataBaseHelper
    {
        bool CreateDataBase();
        bool CreateTable();
        bool OpenDb();
        bool CloseDb();
        bool Add();
        bool Update();
        bool Delete();
        bool Query();
    }
}
