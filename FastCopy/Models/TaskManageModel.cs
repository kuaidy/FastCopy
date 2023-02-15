using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastCopy.Models
{
    public class TaskManageModel
    {
        public string Guid { get; set; }
        public Task Task { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
