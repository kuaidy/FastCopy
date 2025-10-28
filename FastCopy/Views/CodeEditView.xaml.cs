using FastCopy.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FastCopy.Views
{
    /// <summary>
    /// CodeEditView.xaml 的交互逻辑
    /// </summary>
    public partial class CodeEditView : Window
    {
        public CodeEditView()
        {
            InitializeComponent();

            // ViewModel 绑定示例
            this.DataContextChanged += (s, e) =>
            {
                if (e.NewValue is CodeEditViewModel vm)
                {
                    CodeEditor.Text = vm.CodeText ?? string.Empty;

                    // 编辑器内容变化时更新 ViewModel
                    CodeEditor.TextChanged += (sender, args) =>
                    {
                        vm.CodeText = CodeEditor.Text;
                    };

                    // 反向更新：ViewModel 改变时更新编辑器
                    vm.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == nameof(vm.CodeText) &&
                            CodeEditor.Text != vm.CodeText)
                        {
                            CodeEditor.Text = vm.CodeText;
                        }
                    };
                }
            };

        }
    }
}
