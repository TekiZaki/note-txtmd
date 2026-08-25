// ---
// Summary:
// - Purpose: Application entry point and startup argument dispatcher for NoteTxtMd.
// - Role: Initializes WPF application and handles command-line file/folder arguments.
// - Used by: Windows OS startup / Windows Explorer context menu.
// - Depends on: PresentationFramework, MainWindow.
// - Key Responsibilities: Processing startup parameters and passing them to MainWindow.
// - Notes: Targets .NET Framework 4.8.
// ---

using System;
using System.Windows;

namespace NoteTxtMd
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWin = new MainWindow(e.Args);
            mainWin.Show();
        }
    }
}
