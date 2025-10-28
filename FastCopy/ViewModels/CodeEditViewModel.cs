using FastCopy.Basic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FastCopy.ViewModels
{
    public class CodeEditViewModel : NotifactionObject
    {
        public CodeEditViewModel()
        {
            InitCommand();
        }
        private string m_CodeText;
        public string CodeText
        {
            get { return m_CodeText; }
            set
            {
                m_CodeText = value;
                RaisePropertyChange(nameof(CodeText));
            }
        }
        private string m_CodeType;
        public string CodeType
        {
            get { return m_CodeType; }
            set
            {
                m_CodeType = value;
                RaisePropertyChange(nameof(CodeType));
            }
        }
        public ICommand RunCodeCommand { get; set; }
        public ICommand SaveCodeCommand { get; set; }
        private void InitCommand()
        {
            RunCodeCommand = new DelegateCommand(RunCodeCommandExecute);
            SaveCodeCommand = new DelegateCommand(SaveCodeCommandExecute);
        }
        private async void RunCodeCommandExecute()
        {
            if (string.IsNullOrEmpty(CodeText)) return;
            var options = ScriptOptions.Default
                 .WithImports("System", "System.IO", "System.Linq", "System.Windows") // 导入命名空间
                 .WithReferences(
                     typeof(object).Assembly,            // mscorlib
                     typeof(Enumerable).Assembly,        // System.Linq
                     typeof(Uri).Assembly,               // System
                     typeof(MessageBox).Assembly         // PresentationFramework.dll
                 );

            try
            {
                var result = await CSharpScript.EvaluateAsync<object>(CodeText, options);
            }
            catch (Exception ex)
            {

            }
        }
        private void SaveCodeCommandExecute()
        {
            string filePath = $"./data/scripts/";
            if (CodeType == "C#")
            {
                filePath += $"{Guid.NewGuid().ToString()}.cs";
            }
            else if (CodeType == "Python")
            {
                filePath += $"{Guid.NewGuid().ToString()}.py";
            }
            else if (CodeType == "JavaScript")
            {
                filePath += $"{Guid.NewGuid().ToString()}.js";
            }
            else
            {
                filePath += $"{Guid.NewGuid().ToString()}.cs";
            }
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(filePath, CodeText);
            MessageBox.Show("脚本保存成功！", "提示", MessageBoxButton.OK);
        }
    }
}
