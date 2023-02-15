using FastCopy.DataBase;
using FastCopy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Config
{
    public class ConfigService: IConfigService
    {
        private readonly FastCopyDbContext m_FastCopyDbContext;

        public  bool IsCloseToTaskBar { get; set; }

        public  bool IsAutoUpdate { get; set; }

        public  bool IsAutoStart { get; set; }

        public  bool IsCopyNewFile { get; set; }

        public  bool IsSaveModes { get; set; }

        public  List<DetailSetModel> DetailSetModels { get; set; }

        public ConfigService(FastCopyDbContext fastCopyDbContext)
        {
            m_FastCopyDbContext = fastCopyDbContext;
            InitData();
        }

        public void InitData()
        {
            DetailSetModels = m_FastCopyDbContext.DetailSetModels.ToList();

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
