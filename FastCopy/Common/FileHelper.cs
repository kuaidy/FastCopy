using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FastCopy.Common
{
    public class FileHelper
    {
        public static List<Process> GetFileOccupiedProcesses(string file)
        {
            List<Process> processes = new List<Process>();
            try
            {
                Process tool = new Process();
                tool.StartInfo.FileName = @"handle64.exe";
                tool.StartInfo.Arguments = file + " /accepteula";
                tool.StartInfo.UseShellExecute = false;
                tool.StartInfo.RedirectStandardOutput = true;
                tool.Start();
                tool.WaitForExit();
                string outputTool = tool.StandardOutput.ReadToEnd();
                string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
                foreach (Match match in Regex.Matches(outputTool, matchPattern))
                {
                    //Process.GetProcessById(int.Parse(match.Value)).Kill();
                    var pro = Process.GetProcessById(int.Parse(match.Value));
                    processes.Add(pro);
                }
            }
            catch (Exception ex)
            {

            }
            return processes;
        }
    }
}
