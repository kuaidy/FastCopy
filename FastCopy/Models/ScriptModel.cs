using System;
using System.Collections.Generic;
using System.Text;

namespace FastCopy.Models
{
    public class ScriptModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Order { get; set; }
        public int CopyInfoId { get; set; }
    }
}
