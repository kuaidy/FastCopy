using FastCopy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCopy.Config
{
    public interface IConfigService
    {
        bool IsCloseToTaskBar { get; set; }
        bool IsAutoUpdate { get; set; }
        bool IsAutoStart { get; set; }
        bool IsCopyNewFile { get; set; }
        bool IsSaveModes { get; set; }
        List<DetailSetModel> DetailSetModels { get; set; }
    }
}
