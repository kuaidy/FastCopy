using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace FastCopy.Behaviors
{
    public static class AvalonEditHelper
    {
        public static readonly DependencyProperty CodeTextProperty =
        DependencyProperty.RegisterAttached(
            "CodeText",
            typeof(string),
            typeof(AvalonEditHelper),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCodeTextChanged));

        public static string GetCodeText(DependencyObject obj) =>
            (string)obj.GetValue(CodeTextProperty);

        public static void SetCodeText(DependencyObject obj, string value) =>
            obj.SetValue(CodeTextProperty, value);

        private static void OnCodeTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextEditor editor)
            {
                editor.TextChanged -= Editor_TextChanged;
                editor.Text = e.NewValue as string ?? string.Empty;
                editor.TextChanged += Editor_TextChanged;
            }
        }

        private static void Editor_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextEditor editor)
            {
                SetCodeText(editor, editor.Text);
            }
        }
    }
}
