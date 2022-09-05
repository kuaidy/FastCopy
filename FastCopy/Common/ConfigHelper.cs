using FastCopy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using FastCopy.DataBase;

namespace FastCopy.Common
{
    public class ConfigHelper
    {
        public static bool IsCloseToTaskBar { get; set; }

        public static bool IsAutoUpdate { get; set; }

        public static bool IsAutoStart { get; set; }

        public static bool IsCopyNewFile { get; set; }

        public static bool IsSaveModes { get; set; }

        public static List<DetailSetModel> DetailSetModels = new List<DetailSetModel>();

        public static void Init()
        {
            DetailSetModels = SqliteHelper.FastCopyDbContext.DetailSetModels.ToList();

            foreach (DetailSetModel detailSetModel in DetailSetModels)
            {
                switch (detailSetModel.EName)
                {
                    case "IsCloseToTaskBar":
                        {
                            IsCloseToTaskBar = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsAutoUpdate":
                        {
                            IsAutoUpdate = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsAutoStart":
                        {
                            IsAutoStart = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsCopyNewFile":
                        {
                            IsCopyNewFile = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                    case "IsSaveModes":
                        {
                            IsSaveModes = detailSetModel.Value == "1" ? true : false;
                            break;
                        }
                }
            }
        }


    }
}
