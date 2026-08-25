// ---
// Summary:
// - Purpose: Modal dialog for entering file names when creating or renaming files.
// - Role: Input prompt window following Scandinavian aesthetic tokens.
// - Used by: MainWindow sidebar context menu actions (New File, Rename).
// - Depends on: PresentationFramework, WindowsBase.
// - Key Responsibilities: Collecting and validating user string input.
// - Notes: Built for .NET Framework 4.8 with C# 5 compatibility.
// ---

using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace NoteTxtMd
{
    public partial class InputDialog : Window
    {
        public string InputText
        {
            get { return TxtInput != null ? TxtInput.Text.Trim() : string.Empty; }
            set { if (TxtInput != null) TxtInput.Text = value; }
        }

        public InputDialog(string title, string prompt, string defaultText)
        {
            InitializeComponent();
            Title = title;
            LblPrompt.Text = prompt;
            TxtInput.Text = defaultText ?? string.Empty;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtInput.Focus();
            string text = TxtInput.Text;
            int dotIndex = text.LastIndexOf('.');
            if (dotIndex > 0)
            {
                TxtInput.Select(0, dotIndex);
            }
            else
            {
                TxtInput.SelectAll();
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Submit();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Submit();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        private void Submit()
        {
            string text = TxtInput.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Name cannot be empty.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            char[] invalid = Path.GetInvalidFileNameChars();
            if (text.IndexOfAny(invalid) >= 0)
            {
                MessageBox.Show("Name contains invalid characters.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
