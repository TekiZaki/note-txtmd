// ---
// Summary:
// - Purpose: Code-behind logic for MainWindow in NoteTxtMd notepad application with file explorer sidebar and context menu setup.
// - Role: Primary presentation controller handling file browsing, multi-document tabs, view switching, theme toggling, and Explorer integration.
// - Used by: Application runtime / MainWindow.xaml / Windows Explorer context menu.
// - Depends on: PresentationFramework, WindowsBase, System.IO, System.Windows.Threading, MarkdownEngine, DocumentModel, Microsoft.Win32.
// - Key Responsibilities: Managing file tree sidebar, document tabs, synchronized zoom, Explorer registry integration.
// - Notes: Built for .NET Framework 4.8 following Scandinavian design guidelines with C# 5 compatibility.
// ---

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;

namespace NoteTxtMd
{
    public partial class MainWindow : Window
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        private const int SHCNE_ASSOCCHANGED = 0x08000000;
        private const uint SHCNF_IDLIST = 0x0000;

        public static readonly RoutedUICommand NewCommand = new RoutedUICommand("New", "New", typeof(MainWindow));
        public static readonly RoutedUICommand OpenCommand = new RoutedUICommand("Open", "Open", typeof(MainWindow));
        public static readonly RoutedUICommand OpenFolderCommand = new RoutedUICommand("OpenFolder", "OpenFolder", typeof(MainWindow));
        public static readonly RoutedUICommand SaveCommand = new RoutedUICommand("Save", "Save", typeof(MainWindow));
        public static readonly RoutedUICommand SaveAsCommand = new RoutedUICommand("SaveAs", "SaveAs", typeof(MainWindow));
        public static readonly RoutedUICommand CloseTabCommand = new RoutedUICommand("CloseTab", "CloseTab", typeof(MainWindow));
        public static readonly RoutedUICommand ToggleSidebarCommand = new RoutedUICommand("ToggleSidebar", "ToggleSidebar", typeof(MainWindow));
        public static readonly RoutedUICommand NextTabCommand = new RoutedUICommand("NextTab", "NextTab", typeof(MainWindow));
        public static readonly RoutedUICommand PrevTabCommand = new RoutedUICommand("PrevTab", "PrevTab", typeof(MainWindow));
        public static readonly RoutedUICommand RawViewCommand = new RoutedUICommand("RawView", "RawView", typeof(MainWindow));
        public static readonly RoutedUICommand SplitViewCommand = new RoutedUICommand("SplitView", "SplitView", typeof(MainWindow));
        public static readonly RoutedUICommand PreviewViewCommand = new RoutedUICommand("PreviewView", "PreviewView", typeof(MainWindow));
        public static readonly RoutedUICommand ToggleThemeCommand = new RoutedUICommand("ToggleTheme", "ToggleTheme", typeof(MainWindow));
        public static readonly RoutedUICommand ToggleWordWrapCommand = new RoutedUICommand("ToggleWordWrap", "ToggleWordWrap", typeof(MainWindow));
        public static readonly RoutedUICommand ZoomInCommand = new RoutedUICommand("ZoomIn", "ZoomIn", typeof(MainWindow));
        public static readonly RoutedUICommand ZoomOutCommand = new RoutedUICommand("ZoomOut", "ZoomOut", typeof(MainWindow));
        public static readonly RoutedUICommand ZoomResetCommand = new RoutedUICommand("ZoomReset", "ZoomReset", typeof(MainWindow));

        private readonly ObservableCollection<DocumentModel> _tabs = new ObservableCollection<DocumentModel>();
        private DocumentModel _activeDocument;
        private readonly DispatcherTimer _previewDebounceTimer;
        private double _currentFontSize = 14.0;
        private bool _isUpdatingTextInternally = false;
        private bool _isDarkMode = false;
        private bool _isSidebarVisible = true;
        private string _currentFolderPath = string.Empty;
        private readonly string[] _startupArgs;

        public MainWindow() : this(new string[0])
        {
        }

        public MainWindow(string[] args)
        {
            _startupArgs = args ?? new string[0];
            InitializeComponent();
            RegisterCommandBindings();

            _previewDebounceTimer = new DispatcherTimer();
            _previewDebounceTimer.Interval = TimeSpan.FromMilliseconds(150);
            _previewDebounceTimer.Tick += PreviewDebounceTimer_Tick;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadThemePreference();
            ApplyTheme();
            ApplyZoom();
            UpdateContextMenuButtonLabel();

            ContextMenu bgMenu = new ContextMenu();
            MenuItem bgNewFileItem = new MenuItem();
            bgNewFileItem.Header = "New File...";
            bgNewFileItem.Click += delegate(object s, RoutedEventArgs ev)
            {
                if (!string.IsNullOrEmpty(_currentFolderPath) && Directory.Exists(_currentFolderPath))
                {
                    SidebarNewFile(_currentFolderPath, true);
                }
            };
            bgMenu.Items.Add(bgNewFileItem);
            TreeFiles.ContextMenu = bgMenu;

            string targetFileToOpen = null;
            string targetFolderToOpen = null;

            if (_startupArgs.Length > 0)
            {
                string firstArg = _startupArgs[0];
                if (Directory.Exists(firstArg))
                {
                    targetFolderToOpen = Path.GetFullPath(firstArg);
                }
                else if (File.Exists(firstArg))
                {
                    targetFileToOpen = Path.GetFullPath(firstArg);
                    targetFolderToOpen = Path.GetDirectoryName(targetFileToOpen);
                }
            }

            if (!string.IsNullOrEmpty(targetFolderToOpen))
            {
                LoadFolder(targetFolderToOpen);
            }
            else
            {
                string cwd = Directory.GetCurrentDirectory();
                if (Directory.Exists(cwd))
                {
                    LoadFolder(cwd);
                }
            }

            if (!string.IsNullOrEmpty(targetFileToOpen))
            {
                OpenFile(targetFileToOpen);
            }
            else
            {
                string sampleMarkdown = "# Welcome to Note TxtMd\r\n\r\n"
                    + "A clean, distraction-free notepad built with the **Scandinavian aesthetic** for both plain text and Markdown.\r\n\r\n"
                    + "## Key Features\r\n\r\n"
                    + "- **File Explorer Sidebar (`Ctrl + B`):** Browse all `.txt` and `.md` files in the current folder.\r\n"
                    + "- **Windows Explorer Context Menu:** Right-click any file, folder, or folder background to open in NoteTxtMd.\r\n"
                    + "- **Multi-Document Tabs:** Work on multiple files simultaneously (`Ctrl + N`, `Ctrl + W`, `Ctrl + Tab`).\r\n"
                    + "- **Synchronized Zoom:** Text editor and Markdown preview scale seamlessly in sync (`Ctrl + +`, `Ctrl + -`, `Ctrl + 0`, `Ctrl + Scroll`).\r\n"
                    + "- **Dual-Mode Workspace:** Switch between raw text, side-by-side split view, and distraction-free preview.\r\n"
                    + "- **Nordic Themes:** Scandinavian Dark & Light modes with crisp button hover contrast.\r\n\r\n"
                    + "---\r\n\r\n"
                    + "### Task List\r\n"
                    + "- [x] File Explorer sidebar with search filter\r\n"
                    + "- [x] Windows Explorer Context Menu integration\r\n"
                    + "- [x] Multi-tab workspace and synchronized zoom\r\n"
                    + "- [ ] Enjoy distraction-free writing\r\n\r\n"
                    + "### Shortcuts Table\r\n\r\n"
                    + "| Shortcut | Action |\r\n"
                    + "| :--- | :--- |\r\n"
                    + "| `Ctrl + B` | Toggle File Explorer Sidebar |\r\n"
                    + "| `Ctrl + Shift + O` | Open Folder in Sidebar |\r\n"
                    + "| `Ctrl + N` | New Document Tab |\r\n"
                    + "| `Ctrl + O` | Open File in Tab |\r\n"
                    + "| `Ctrl + W` | Close Active Tab |\r\n"
                    + "| `Ctrl + Tab` | Next Document Tab |\r\n"
                    + "| `Ctrl + 1 / 2 / 3` | Raw / Split / Preview View |\r\n"
                    + "| `Ctrl + T` | Toggle Dark / Light Theme |\r\n"
                    + "| `Alt + Z` | Toggle Word Wrap |\r\n"
                    + "| `Ctrl + +/-/0` | Synchronized Zoom In / Out / Reset |\r\n";

                CreateNewTab(null, sampleMarkdown, false);
            }
        }

        private void RegisterCommandBindings()
        {
            CommandBindings.Add(new CommandBinding(NewCommand, delegate(object s, ExecutedRoutedEventArgs e) { PerformNew(); }));
            CommandBindings.Add(new CommandBinding(OpenCommand, delegate(object s, ExecutedRoutedEventArgs e) { PerformOpen(); }));
            CommandBindings.Add(new CommandBinding(OpenFolderCommand, delegate(object s, ExecutedRoutedEventArgs e) { PerformOpenFolder(); }));
            CommandBindings.Add(new CommandBinding(SaveCommand, delegate(object s, ExecutedRoutedEventArgs e) { PerformSave(); }));
            CommandBindings.Add(new CommandBinding(SaveAsCommand, delegate(object s, ExecutedRoutedEventArgs e) { PerformSaveAs(); }));
            CommandBindings.Add(new CommandBinding(CloseTabCommand, delegate(object s, ExecutedRoutedEventArgs e) { PerformCloseActiveTab(); }));
            CommandBindings.Add(new CommandBinding(ToggleSidebarCommand, delegate(object s, ExecutedRoutedEventArgs e) { ToggleSidebar(); }));
            CommandBindings.Add(new CommandBinding(NextTabCommand, delegate(object s, ExecutedRoutedEventArgs e) { CycleTab(1); }));
            CommandBindings.Add(new CommandBinding(PrevTabCommand, delegate(object s, ExecutedRoutedEventArgs e) { CycleTab(-1); }));
            CommandBindings.Add(new CommandBinding(RawViewCommand, delegate(object s, ExecutedRoutedEventArgs e) { SwitchMode(0); }));
            CommandBindings.Add(new CommandBinding(SplitViewCommand, delegate(object s, ExecutedRoutedEventArgs e) { SwitchMode(1); }));
            CommandBindings.Add(new CommandBinding(PreviewViewCommand, delegate(object s, ExecutedRoutedEventArgs e) { SwitchMode(2); }));
            CommandBindings.Add(new CommandBinding(ToggleThemeCommand, delegate(object s, ExecutedRoutedEventArgs e) { ToggleTheme(); }));
            CommandBindings.Add(new CommandBinding(ToggleWordWrapCommand, delegate(object s, ExecutedRoutedEventArgs e) { ToggleWordWrap(); }));
            CommandBindings.Add(new CommandBinding(ZoomInCommand, delegate(object s, ExecutedRoutedEventArgs e) { ZoomIn(); }));
            CommandBindings.Add(new CommandBinding(ZoomOutCommand, delegate(object s, ExecutedRoutedEventArgs e) { ZoomOut(); }));
            CommandBindings.Add(new CommandBinding(ZoomResetCommand, delegate(object s, ExecutedRoutedEventArgs e) { ZoomReset(); }));
        }

        #region File Explorer Sidebar

        private void ToggleSidebar()
        {
            _isSidebarVisible = !_isSidebarVisible;
            if (_isSidebarVisible)
            {
                ColSidebar.Width = new GridLength(230);
                ColSidebarSplitter.Width = GridLength.Auto;
                SidebarBorder.Visibility = Visibility.Visible;
                SidebarSplitter.Visibility = Visibility.Visible;
            }
            else
            {
                ColSidebar.Width = new GridLength(0);
                ColSidebarSplitter.Width = new GridLength(0);
                SidebarBorder.Visibility = Visibility.Collapsed;
                SidebarSplitter.Visibility = Visibility.Collapsed;
            }
        }

        public void LoadFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                return;

            _currentFolderPath = Path.GetFullPath(folderPath);
            string folderName = Path.GetFileName(_currentFolderPath);
            if (string.IsNullOrEmpty(folderName))
                folderName = _currentFolderPath;

            LblSidebarFolder.Text = "EXPLORER: " + folderName.ToUpperInvariant();
            PopulateFileTree(TxtSearchFilter != null ? TxtSearchFilter.Text : string.Empty);
        }

        private void PopulateFileTree(string filter)
        {
            if (TreeFiles == null || string.IsNullOrEmpty(_currentFolderPath) || !Directory.Exists(_currentFolderPath))
                return;

            TreeFiles.Items.Clear();

            try
            {
                DirectoryInfo rootDir = new DirectoryInfo(_currentFolderPath);
                bool hasAny = PopulateFolderItems(TreeFiles.Items, rootDir, filter, true);

                if (!hasAny)
                {
                    TreeViewItem emptyItem = new TreeViewItem();
                    emptyItem.Header = "(No .txt or .md files found)";
                    emptyItem.Foreground = (Brush)Application.Current.Resources["SecondaryInkBrush"];
                    emptyItem.FontStyle = FontStyles.Italic;
                    TreeFiles.Items.Add(emptyItem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error populating file tree: " + ex.Message);
            }
        }

        private bool PopulateFolderItems(ItemCollection targetCollection, DirectoryInfo dirInfo, string filter, bool isRoot)
        {
            bool hasMatchingContent = false;
            string lowerFilter = string.IsNullOrEmpty(filter) ? string.Empty : filter.Trim().ToLowerInvariant();

            try
            {
                // Subdirectories
                DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                Array.Sort(subDirs, delegate(DirectoryInfo a, DirectoryInfo b) { return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase); });

                foreach (DirectoryInfo subDir in subDirs)
                {
                    if ((subDir.Attributes & FileAttributes.Hidden) != 0 || subDir.Name.StartsWith("."))
                        continue;

                    TreeViewItem subDirNode = new TreeViewItem();
                    subDirNode.Header = "📁 " + subDir.Name;
                    subDirNode.Tag = subDir.FullName;
                    subDirNode.FontWeight = FontWeights.Medium;
                    subDirNode.ContextMenu = CreateSidebarContextMenu(subDir.FullName, true);

                    bool subHasMatch = PopulateFolderItems(subDirNode.Items, subDir, filter, false);
                    if (subHasMatch)
                    {
                        subDirNode.IsExpanded = !string.IsNullOrEmpty(lowerFilter);
                        targetCollection.Add(subDirNode);
                        hasMatchingContent = true;
                    }
                }

                // Files (.txt, .md, .markdown)
                FileInfo[] files = dirInfo.GetFiles();
                Array.Sort(files, delegate(FileInfo a, FileInfo b) { return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase); });

                foreach (FileInfo file in files)
                {
                    if ((file.Attributes & FileAttributes.Hidden) != 0)
                        continue;

                    string ext = file.Extension.ToLowerInvariant();
                    if (ext == ".txt" || ext == ".md" || ext == ".markdown" || ext == ".mdown")
                    {
                        if (string.IsNullOrEmpty(lowerFilter) || file.Name.ToLowerInvariant().Contains(lowerFilter))
                        {
                            TreeViewItem fileNode = new TreeViewItem();
                            string icon = (ext == ".txt") ? "📄 " : "📝 ";
                            fileNode.Header = icon + file.Name;
                            fileNode.Tag = file.FullName;
                            fileNode.FontWeight = FontWeights.Normal;
                            fileNode.ContextMenu = CreateSidebarContextMenu(file.FullName, false);
                            targetCollection.Add(fileNode);
                            hasMatchingContent = true;
                        }
                    }
                }
            }
            catch
            {
            }

            return hasMatchingContent;
        }

        private ContextMenu CreateSidebarContextMenu(string targetPath, bool isDirectory)
        {
            ContextMenu menu = new ContextMenu();

            MenuItem newFileItem = new MenuItem();
            newFileItem.Header = "New File...";
            newFileItem.Click += delegate(object s, RoutedEventArgs e)
            {
                SidebarNewFile(targetPath, isDirectory);
            };
            menu.Items.Add(newFileItem);

            MenuItem renameItem = new MenuItem();
            renameItem.Header = "Rename...";
            renameItem.Click += delegate(object s, RoutedEventArgs e)
            {
                SidebarRename(targetPath, isDirectory);
            };
            menu.Items.Add(renameItem);

            MenuItem deleteItem = new MenuItem();
            deleteItem.Header = "Delete";
            deleteItem.Click += delegate(object s, RoutedEventArgs e)
            {
                SidebarDelete(targetPath, isDirectory);
            };
            menu.Items.Add(deleteItem);

            return menu;
        }

        private void SidebarNewFile(string targetPath, bool isDirectory)
        {
            string targetDir = isDirectory ? targetPath : Path.GetDirectoryName(targetPath);
            if (string.IsNullOrEmpty(targetDir) || !Directory.Exists(targetDir))
            {
                targetDir = _currentFolderPath;
            }
            if (string.IsNullOrEmpty(targetDir) || !Directory.Exists(targetDir))
            {
                return;
            }

            InputDialog dlg = new InputDialog("New File", "Enter file name:", "note.md");
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                string fileName = dlg.InputText;
                if (!Path.HasExtension(fileName))
                {
                    fileName += ".md";
                }

                string fullPath = Path.Combine(targetDir, fileName);
                if (File.Exists(fullPath))
                {
                    MessageBox.Show(string.Format("File \"{0}\" already exists.", fileName), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    File.WriteAllText(fullPath, string.Empty, Encoding.UTF8);
                    PopulateFileTree(TxtSearchFilter != null ? TxtSearchFilter.Text : string.Empty);
                    OpenFile(fullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SidebarRename(string targetPath, bool isDirectory)
        {
            if (string.IsNullOrEmpty(targetPath) || (!File.Exists(targetPath) && !Directory.Exists(targetPath)))
                return;

            string oldName = Path.GetFileName(targetPath);
            string parentDir = Path.GetDirectoryName(targetPath);

            InputDialog dlg = new InputDialog(isDirectory ? "Rename Folder" : "Rename File", "Enter new name:", oldName);
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                string newName = dlg.InputText;
                if (string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
                    return;

                string newPath = Path.Combine(parentDir, newName);
                if (File.Exists(newPath) || Directory.Exists(newPath))
                {
                    MessageBox.Show(string.Format("\"{0}\" already exists.", newName), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    if (isDirectory)
                    {
                        Directory.Move(targetPath, newPath);
                    }
                    else
                    {
                        File.Move(targetPath, newPath);

                        foreach (DocumentModel doc in _tabs)
                        {
                            if (string.Equals(doc.FilePath, targetPath, StringComparison.OrdinalIgnoreCase))
                            {
                                doc.FilePath = newPath;
                            }
                        }
                        UpdateStatusInfo();
                        RebuildTabBar();
                    }

                    PopulateFileTree(TxtSearchFilter != null ? TxtSearchFilter.Text : string.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error renaming:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SidebarDelete(string targetPath, bool isDirectory)
        {
            if (string.IsNullOrEmpty(targetPath) || (!File.Exists(targetPath) && !Directory.Exists(targetPath)))
                return;

            string itemName = Path.GetFileName(targetPath);
            MessageBoxResult result = MessageBox.Show(
                string.Format("Are you sure you want to move \"{0}\" to the Recycle Bin?", itemName),
                "Delete Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (isDirectory)
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(
                            targetPath,
                            Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                    else
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(
                            targetPath,
                            Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

                        DocumentModel docToClose = null;
                        foreach (DocumentModel doc in _tabs)
                        {
                            if (string.Equals(doc.FilePath, targetPath, StringComparison.OrdinalIgnoreCase))
                            {
                                docToClose = doc;
                                break;
                            }
                        }

                        if (docToClose != null)
                        {
                            docToClose.IsModified = false;
                            CloseTab(docToClose);
                        }
                    }

                    PopulateFileTree(TxtSearchFilter != null ? TxtSearchFilter.Text : string.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting item:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = sender as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                treeViewItem.IsSelected = true;
            }
        }

        private void TreeFiles_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = TreeFiles.SelectedItem as TreeViewItem;
            if (item != null && item.Tag != null)
            {
                string path = item.Tag.ToString();
                if (File.Exists(path))
                {
                    OpenFile(path);
                }
            }
        }

        private void TreeFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = TreeFiles.SelectedItem as TreeViewItem;
            if (item != null && item.Tag != null)
            {
                string path = item.Tag.ToString();
                if (File.Exists(path))
                {
                    OpenFile(path);
                }
            }
        }

        private void TxtSearchFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            PopulateFileTree(TxtSearchFilter.Text);
        }

        private void BtnRefreshSidebar_Click(object sender, RoutedEventArgs e)
        {
            PopulateFileTree(TxtSearchFilter.Text);
        }

        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            PerformOpenFolder();
        }

        private void PerformOpenFolder()
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.Description = "Select Folder to Open in NoteTxtMd";
            dlg.ShowNewFolderButton = true;
            if (!string.IsNullOrEmpty(_currentFolderPath) && Directory.Exists(_currentFolderPath))
            {
                dlg.SelectedPath = _currentFolderPath;
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadFolder(dlg.SelectedPath);
                if (!_isSidebarVisible)
                {
                    ToggleSidebar();
                }
            }
        }

        #endregion

        #region Windows Explorer Context Menu Integration

        private bool IsExplorerContextMenuRegistered()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Directory\shell\NoteTxtMd"))
                {
                    return rk != null;
                }
            }
            catch
            {
                return false;
            }
        }

        private void UpdateContextMenuButtonLabel()
        {
            bool isReg = IsExplorerContextMenuRegistered();
            BtnContextMenu.Content = isReg ? "Context Menu: On" : "Context Menu: Off";
        }

        private void ToggleExplorerContextMenu()
        {
            bool isReg = IsExplorerContextMenuRegistered();
            if (isReg)
            {
                MessageBoxResult res = MessageBox.Show("Remove \"Open with NoteTxtMd\" from Windows Explorer right-click context menu?",
                    "Context Menu Integration", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    UnregisterExplorerContextMenu();
                    UpdateContextMenuButtonLabel();
                    MessageBox.Show("Context menu items successfully removed.", "Context Menu Integration", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("Add \"Open with NoteTxtMd\" to Windows Explorer right-click context menu for files and folders?",
                    "Context Menu Integration", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    RegisterExplorerContextMenu();
                    UpdateContextMenuButtonLabel();
                    MessageBox.Show("Context menu successfully registered! You can now right-click any folder or file to open in NoteTxtMd.", "Context Menu Integration", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void RegisterExplorerContextMenu()
        {
            try
            {
                string exePath = Process.GetCurrentProcess().MainModule.FileName;

                // 1. Directory context menu (right click folder)
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\Directory\shell\NoteTxtMd"))
                {
                    if (key != null)
                    {
                        key.SetValue("", "Open with NoteTxtMd");
                        key.SetValue("Icon", exePath);
                        using (RegistryKey cmd = key.CreateSubKey("command"))
                        {
                            if (cmd != null) cmd.SetValue("", "\"" + exePath + "\" \"%1\"");
                        }
                    }
                }

                // 2. Directory background context menu (right click inside folder background)
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\Directory\Background\shell\NoteTxtMd"))
                {
                    if (key != null)
                    {
                        key.SetValue("", "Open with NoteTxtMd");
                        key.SetValue("Icon", exePath);
                        using (RegistryKey cmd = key.CreateSubKey("command"))
                        {
                            if (cmd != null) cmd.SetValue("", "\"" + exePath + "\" \"%V\"");
                        }
                    }
                }

                // 3. File context menu (right click any file)
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\*\shell\NoteTxtMd"))
                {
                    if (key != null)
                    {
                        key.SetValue("", "Open with NoteTxtMd");
                        key.SetValue("Icon", exePath);
                        using (RegistryKey cmd = key.CreateSubKey("command"))
                        {
                            if (cmd != null) cmd.SetValue("", "\"" + exePath + "\" \"%1\"");
                        }
                    }
                }

                // 4. Drive context menu (right click drive root)
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\Drive\shell\NoteTxtMd"))
                {
                    if (key != null)
                    {
                        key.SetValue("", "Open with NoteTxtMd");
                        key.SetValue("Icon", exePath);
                        using (RegistryKey cmd = key.CreateSubKey("command"))
                        {
                            if (cmd != null) cmd.SetValue("", "\"" + exePath + "\" \"%1\"");
                        }
                    }
                }

                // Notify Windows Explorer immediately of context menu changes
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering context menu:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnregisterExplorerContextMenu()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\Directory\shell\NoteTxtMd", false);
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\Directory\Background\shell\NoteTxtMd", false);
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\*\shell\NoteTxtMd", false);
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\Drive\shell\NoteTxtMd", false);

                // Notify Windows Explorer immediately
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unregistering context menu:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Tab Management

        private DocumentModel CreateNewTab(string filePath, string content, bool isModified)
        {
            DocumentModel doc = new DocumentModel();
            doc.FilePath = filePath;
            doc.Content = content ?? string.Empty;
            doc.IsModified = isModified;
            doc.ViewModeIndex = _activeDocument != null ? _activeDocument.ViewModeIndex : 1;

            _tabs.Add(doc);
            SelectTab(doc);
            RebuildTabBar();
            return doc;
        }

        private void SelectTab(DocumentModel doc)
        {
            if (doc == null || !_tabs.Contains(doc))
                return;

            if (_activeDocument != null && _activeDocument != doc)
            {
                _activeDocument.Content = TxtEditor.Text;
                _activeDocument.CaretIndex = TxtEditor.CaretIndex;
            }

            _activeDocument = doc;

            _isUpdatingTextInternally = true;
            try
            {
                TxtEditor.Text = doc.Content ?? string.Empty;
                if (doc.CaretIndex <= TxtEditor.Text.Length)
                {
                    TxtEditor.CaretIndex = doc.CaretIndex;
                }
            }
            finally
            {
                _isUpdatingTextInternally = false;
            }

            SwitchMode(doc.ViewModeIndex);
            UpdateStatusInfo();
            RenderPreviewNow();
            RebuildTabBar();
        }

        private bool CloseTab(DocumentModel doc)
        {
            if (doc == null || !_tabs.Contains(doc))
                return true;

            if (doc.IsModified)
            {
                SelectTab(doc);
                MessageBoxResult result = MessageBox.Show(
                    string.Format("Do you want to save changes to \"{0}\"?", doc.FileName),
                    "Note TxtMd",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (!PerformSave()) return false;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }

            int index = _tabs.IndexOf(doc);
            _tabs.Remove(doc);

            if (_tabs.Count == 0)
            {
                CreateNewTab(null, string.Empty, false);
            }
            else if (_activeDocument == doc)
            {
                int newIndex = Math.Min(index, _tabs.Count - 1);
                SelectTab(_tabs[newIndex]);
            }
            else
            {
                RebuildTabBar();
            }

            return true;
        }

        private void PerformCloseActiveTab()
        {
            if (_activeDocument != null)
            {
                CloseTab(_activeDocument);
            }
        }

        private void CycleTab(int direction)
        {
            if (_tabs.Count <= 1 || _activeDocument == null)
                return;

            int currentIndex = _tabs.IndexOf(_activeDocument);
            int nextIndex = (currentIndex + direction + _tabs.Count) % _tabs.Count;
            SelectTab(_tabs[nextIndex]);
        }

        private void RebuildTabBar()
        {
            if (TabStackPanel == null)
                return;

            TabStackPanel.Children.Clear();

            Brush activeBg = (Brush)Application.Current.Resources["CanvasBrush"];
            Brush inactiveBg = (Brush)Application.Current.Resources["SurfaceBrush"];
            Brush primaryInk = (Brush)Application.Current.Resources["PrimaryInkBrush"];
            Brush secondaryInk = (Brush)Application.Current.Resources["SecondaryInkBrush"];
            Brush borderBrush = (Brush)Application.Current.Resources["BorderBrush"];

            foreach (DocumentModel doc in _tabs)
            {
                bool isActive = (doc == _activeDocument);

                Border tabBorder = new Border();
                tabBorder.Background = isActive ? activeBg : inactiveBg;
                tabBorder.BorderBrush = borderBrush;
                tabBorder.BorderThickness = new Thickness(0, 0, 1, 0);
                tabBorder.Padding = new Thickness(12, 0, 6, 0);
                tabBorder.Height = 34;
                tabBorder.Cursor = Cursors.Hand;
                tabBorder.SnapsToDevicePixels = true;

                StackPanel tabContent = new StackPanel();
                tabContent.Orientation = Orientation.Horizontal;
                tabContent.VerticalAlignment = VerticalAlignment.Center;

                TextBlock titleBlock = new TextBlock();
                titleBlock.Text = doc.TabTitle;
                titleBlock.FontSize = 12;
                titleBlock.FontWeight = isActive ? FontWeights.Medium : FontWeights.Normal;
                titleBlock.Foreground = isActive ? primaryInk : secondaryInk;
                titleBlock.VerticalAlignment = VerticalAlignment.Center;
                titleBlock.Margin = new Thickness(0, 0, 8, 0);
                tabContent.Children.Add(titleBlock);

                Button closeBtn = new Button();
                closeBtn.Content = "×";
                closeBtn.FontSize = 13;
                closeBtn.FontWeight = FontWeights.Normal;
                closeBtn.Foreground = secondaryInk;
                closeBtn.Background = Brushes.Transparent;
                closeBtn.BorderThickness = new Thickness(0);
                closeBtn.Padding = new Thickness(4, 0, 4, 1);
                closeBtn.Width = 18;
                closeBtn.Height = 18;
                closeBtn.VerticalAlignment = VerticalAlignment.Center;
                closeBtn.Cursor = Cursors.Hand;
                closeBtn.ToolTip = "Close Tab (Ctrl+W)";

                DocumentModel targetDoc = doc;
                closeBtn.Click += delegate(object s, RoutedEventArgs e)
                {
                    CloseTab(targetDoc);
                };

                tabContent.Children.Add(closeBtn);
                tabBorder.Child = tabContent;

                tabBorder.MouseLeftButtonDown += delegate(object s, MouseButtonEventArgs e)
                {
                    SelectTab(targetDoc);
                };

                TabStackPanel.Children.Add(tabBorder);
            }
        }

        #endregion

        #region Theme Switching

        private void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
            SaveThemePreference();
            ApplyTheme();
            RebuildTabBar();
            PopulateFileTree(TxtSearchFilter != null ? TxtSearchFilter.Text : string.Empty);
            RenderPreviewNow();
        }

        private void LoadThemePreference()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\NoteTxtMd"))
                {
                    if (rk != null)
                    {
                        object val = rk.GetValue("Theme");
                        if (val != null && string.Equals(val.ToString(), "Dark", StringComparison.OrdinalIgnoreCase))
                        {
                            _isDarkMode = true;
                        }
                        else if (val != null && string.Equals(val.ToString(), "Light", StringComparison.OrdinalIgnoreCase))
                        {
                            _isDarkMode = false;
                        }
                    }
                }
            }
            catch { }
        }

        private void SaveThemePreference()
        {
            try
            {
                using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(@"Software\NoteTxtMd"))
                {
                    if (rk != null)
                    {
                        rk.SetValue("Theme", _isDarkMode ? "Dark" : "Light");
                    }
                }
            }
            catch { }
        }

        private void ApplyTheme()
        {
            BtnTheme.Content = _isDarkMode ? "Theme: Dark" : "Theme: Light";

            Color canvasColor = _isDarkMode ? Color.FromRgb(0x0F, 0x0F, 0x0F) : Color.FromRgb(0xFF, 0xFF, 0xFF);
            Color surfaceColor = _isDarkMode ? Color.FromRgb(0x16, 0x16, 0x16) : Color.FromRgb(0xFA, 0xFA, 0xFA);
            Color primaryInk = _isDarkMode ? Color.FromRgb(0xEC, 0xEC, 0xEC) : Color.FromRgb(0x11, 0x11, 0x11);
            Color secondaryInk = _isDarkMode ? Color.FromRgb(0x9E, 0x9E, 0x9E) : Color.FromRgb(0x66, 0x66, 0x66);
            Color tertiaryInk = _isDarkMode ? Color.FromRgb(0x55, 0x55, 0x55) : Color.FromRgb(0x99, 0x99, 0x99);
            Color borderColor = _isDarkMode ? Color.FromRgb(0x28, 0x28, 0x28) : Color.FromRgb(0xE5, 0xE5, 0xE5);
            Color strongBorderColor = _isDarkMode ? Color.FromRgb(0x38, 0x38, 0x38) : Color.FromRgb(0xCC, 0xCC, 0xCC);
            Color hoverFill = _isDarkMode ? Color.FromRgb(0x26, 0x26, 0x26) : Color.FromRgb(0xF0, 0xF0, 0xF0);
            Color pressedFill = _isDarkMode ? Color.FromRgb(0x33, 0x33, 0x33) : Color.FromRgb(0xE2, 0xE2, 0xE2);
            Color selectedFill = _isDarkMode ? Color.FromRgb(0xEC, 0xEC, 0xEC) : Color.FromRgb(0x11, 0x11, 0x11);
            Color selectedInk = _isDarkMode ? Color.FromRgb(0x0F, 0x0F, 0x0F) : Color.FromRgb(0xFF, 0xFF, 0xFF);
            Color scrollThumb = _isDarkMode ? Color.FromRgb(0x4A, 0x4A, 0x4A) : Color.FromRgb(0xB8, 0xB8, 0xB8);
            Color scrollThumbHover = _isDarkMode ? Color.FromRgb(0x6A, 0x6A, 0x6A) : Color.FromRgb(0x8E, 0x8E, 0x8E);
            Color scrollThumbPressed = _isDarkMode ? Color.FromRgb(0x8A, 0x8A, 0x8A) : Color.FromRgb(0x6E, 0x6E, 0x6E);

            Application.Current.Resources["CanvasBrush"] = new SolidColorBrush(canvasColor);
            Application.Current.Resources["SurfaceBrush"] = new SolidColorBrush(surfaceColor);
            Application.Current.Resources["PrimaryInkBrush"] = new SolidColorBrush(primaryInk);
            Application.Current.Resources["SecondaryInkBrush"] = new SolidColorBrush(secondaryInk);
            Application.Current.Resources["TertiaryInkBrush"] = new SolidColorBrush(tertiaryInk);
            Application.Current.Resources["BorderBrush"] = new SolidColorBrush(borderColor);
            Application.Current.Resources["StrongBorderBrush"] = new SolidColorBrush(strongBorderColor);
            Application.Current.Resources["HoverFillBrush"] = new SolidColorBrush(hoverFill);
            Application.Current.Resources["PressedFillBrush"] = new SolidColorBrush(pressedFill);
            Application.Current.Resources["SelectedFillBrush"] = new SolidColorBrush(selectedFill);
            Application.Current.Resources["SelectedInkBrush"] = new SolidColorBrush(selectedInk);
            Application.Current.Resources["ScrollBarThumbBrush"] = new SolidColorBrush(scrollThumb);
            Application.Current.Resources["ScrollBarThumbHoverBrush"] = new SolidColorBrush(scrollThumbHover);
            Application.Current.Resources["ScrollBarThumbPressedBrush"] = new SolidColorBrush(scrollThumbPressed);
            Application.Current.Resources["ScrollBarTrackBrush"] = new SolidColorBrush(Colors.Transparent);
        }

        #endregion

        #region Synchronized Zoom

        private void ZoomIn()
        {
            if (_currentFontSize < 36.0)
            {
                _currentFontSize += 2.0;
                ApplyZoom();
            }
        }

        private void ZoomOut()
        {
            if (_currentFontSize > 8.0)
            {
                _currentFontSize -= 2.0;
                ApplyZoom();
            }
        }

        private void ZoomReset()
        {
            _currentFontSize = 14.0;
            ApplyZoom();
        }

        private void ApplyZoom()
        {
            TxtEditor.FontSize = _currentFontSize;
            LblZoom.Text = Math.Round((_currentFontSize / 14.0) * 100.0) + "%";
            RenderPreviewNow();
        }

        private void WorkspaceGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Delta > 0)
                    ZoomIn();
                else if (e.Delta < 0)
                    ZoomOut();
                e.Handled = true;
            }
        }

        #endregion

        #region View Mode Switching

        private void SwitchMode(int modeIndex)
        {
            if (_activeDocument != null)
            {
                _activeDocument.ViewModeIndex = modeIndex;
            }

            if (modeIndex == 0 && RadioRaw != null) RadioRaw.IsChecked = true;
            else if (modeIndex == 1 && RadioSplit != null) RadioSplit.IsChecked = true;
            else if (modeIndex == 2 && RadioPreview != null) RadioPreview.IsChecked = true;

            UpdateViewMode();
        }

        private void RadioViewMode_Checked(object sender, RoutedEventArgs e)
        {
            if (RadioRaw != null && RadioRaw.IsChecked == true)
                SwitchMode(0);
            else if (RadioSplit != null && RadioSplit.IsChecked == true)
                SwitchMode(1);
            else if (RadioPreview != null && RadioPreview.IsChecked == true)
                SwitchMode(2);
        }

        private void UpdateViewMode()
        {
            if (EditorContainer == null || PreviewContainer == null || ViewSplitter == null)
                return;

            if (RadioRaw != null && RadioRaw.IsChecked == true)
            {
                EditorContainer.Visibility = Visibility.Visible;
                PreviewContainer.Visibility = Visibility.Collapsed;
                ViewSplitter.Visibility = Visibility.Collapsed;

                ColEditor.Width = new GridLength(1, GridUnitType.Star);
                ColSplitter.Width = new GridLength(0);
                ColPreview.Width = new GridLength(0);
            }
            else if (RadioPreview != null && RadioPreview.IsChecked == true)
            {
                EditorContainer.Visibility = Visibility.Collapsed;
                PreviewContainer.Visibility = Visibility.Visible;
                ViewSplitter.Visibility = Visibility.Collapsed;

                ColEditor.Width = new GridLength(0);
                ColSplitter.Width = new GridLength(0);
                ColPreview.Width = new GridLength(1, GridUnitType.Star);

                RenderPreviewNow();
            }
            else
            {
                EditorContainer.Visibility = Visibility.Visible;
                PreviewContainer.Visibility = Visibility.Visible;
                ViewSplitter.Visibility = Visibility.Visible;

                ColEditor.Width = new GridLength(1, GridUnitType.Star);
                ColSplitter.Width = GridLength.Auto;
                ColPreview.Width = new GridLength(1, GridUnitType.Star);

                RenderPreviewNow();
            }
        }

        #endregion

        #region Preview Rendering

        private void TxtEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingTextInternally || _activeDocument == null)
                return;

            _activeDocument.Content = TxtEditor.Text;
            _activeDocument.IsModified = true;

            UpdateStatusInfo();
            RebuildTabBar();

            _previewDebounceTimer.Stop();
            _previewDebounceTimer.Start();
        }

        private void PreviewDebounceTimer_Tick(object sender, EventArgs e)
        {
            _previewDebounceTimer.Stop();
            RenderPreviewNow();
        }

        private void RenderPreviewNow()
        {
            if (ViewerPreview == null || _activeDocument == null)
                return;

            try
            {
                string markdown = TxtEditor.Text ?? string.Empty;
                ViewerPreview.Document = MarkdownEngine.RenderToFlowDocument(markdown, _isDarkMode, _currentFontSize);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Preview rendering error: " + ex.Message);
            }
        }

        #endregion

        #region File Operations

        private void PerformNew()
        {
            CreateNewTab(null, string.Empty, false);
        }

        private void PerformOpen()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Text or Markdown File";
            dlg.Filter = "Supported Files (*.md;*.txt;*.markdown)|*.md;*.txt;*.markdown|Markdown Files (*.md;*.markdown)|*.md;*.markdown|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == true)
            {
                foreach (string file in dlg.FileNames)
                {
                    OpenFile(file);
                }
            }
        }

        public void OpenFile(string filePath)
        {
            try
            {
                foreach (DocumentModel existing in _tabs)
                {
                    if (string.Equals(existing.FilePath, filePath, StringComparison.OrdinalIgnoreCase))
                    {
                        SelectTab(existing);
                        return;
                    }
                }

                string content = File.ReadAllText(filePath, Encoding.UTF8);

                if (_activeDocument != null && string.IsNullOrEmpty(_activeDocument.FilePath) && !_activeDocument.IsModified && string.IsNullOrEmpty(_activeDocument.Content))
                {
                    _activeDocument.Reset(filePath, content);
                    _isUpdatingTextInternally = true;
                    try
                    {
                        TxtEditor.Text = content;
                    }
                    finally
                    {
                        _isUpdatingTextInternally = false;
                    }
                    UpdateStatusInfo();
                    RebuildTabBar();
                    RenderPreviewNow();
                }
                else
                {
                    CreateNewTab(filePath, content, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool PerformSave()
        {
            if (_activeDocument == null)
                return false;

            if (string.IsNullOrEmpty(_activeDocument.FilePath))
            {
                // If a folder is open in workspace, save directly into that folder without opening file dialog
                if (!string.IsNullOrEmpty(_currentFolderPath) && Directory.Exists(_currentFolderPath))
                {
                    string fileName = DeriveDefaultFileName(TxtEditor.Text, _activeDocument.IsMarkdownFile);
                    string targetPath = GetUniqueFilePath(_currentFolderPath, fileName);

                    try
                    {
                        File.WriteAllText(targetPath, TxtEditor.Text, Encoding.UTF8);
                        _activeDocument.FilePath = targetPath;
                        _activeDocument.IsModified = false;
                        UpdateStatusInfo();
                        RebuildTabBar();
                        PopulateFileTree(TxtSearchFilter != null ? TxtSearchFilter.Text : string.Empty);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    return PerformSaveAs();
                }
            }

            try
            {
                File.WriteAllText(_activeDocument.FilePath, TxtEditor.Text, Encoding.UTF8);
                _activeDocument.IsModified = false;
                UpdateStatusInfo();
                RebuildTabBar();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool PerformSaveAs()
        {
            if (_activeDocument == null)
                return false;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save Document As";
            dlg.Filter = "Markdown File (*.md)|*.md|Text Document (*.txt)|*.txt|All Files (*.*)|*.*";
            dlg.DefaultExt = ".md";
            dlg.FilterIndex = _activeDocument.IsMarkdownFile ? 1 : 2;

            if (!string.IsNullOrEmpty(_currentFolderPath) && Directory.Exists(_currentFolderPath))
            {
                dlg.InitialDirectory = _currentFolderPath;
            }

            dlg.FileName = _activeDocument.FileName == "Untitled" 
                ? DeriveDefaultFileName(TxtEditor.Text, _activeDocument.IsMarkdownFile) 
                : _activeDocument.FileName;

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, TxtEditor.Text, Encoding.UTF8);
                    _activeDocument.FilePath = dlg.FileName;
                    _activeDocument.IsModified = false;
                    UpdateStatusInfo();
                    RebuildTabBar();
                    PopulateFileTree(TxtSearchFilter != null ? TxtSearchFilter.Text : string.Empty);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return false;
        }

        private string DeriveDefaultFileName(string content, bool isMarkdown)
        {
            string ext = isMarkdown ? ".md" : ".txt";
            if (!string.IsNullOrWhiteSpace(content))
            {
                string[] lines = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith("#"))
                    {
                        trimmed = trimmed.TrimStart('#').Trim();
                    }
                    if (!string.IsNullOrWhiteSpace(trimmed))
                    {
                        string safeName = SanitizeFileName(trimmed);
                        if (!string.IsNullOrEmpty(safeName))
                        {
                            if (safeName.Length > 36) safeName = safeName.Substring(0, 36).Trim();
                            return safeName + ext;
                        }
                    }
                }
            }
            return "note" + ext;
        }

        private string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "note";
            char[] invalid = Path.GetInvalidFileNameChars();
            StringBuilder sb = new StringBuilder();
            foreach (char c in name)
            {
                if (Array.IndexOf(invalid, c) < 0 && c != '/' && c != '\\')
                {
                    sb.Append(c);
                }
            }
            string result = sb.ToString().Trim();
            return string.IsNullOrEmpty(result) ? "note" : result;
        }

        private string GetUniqueFilePath(string folderPath, string fileName)
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            string fullPath = Path.Combine(folderPath, fileName);

            int counter = 1;
            while (File.Exists(fullPath))
            {
                fullPath = Path.Combine(folderPath, string.Format("{0}-{1}{2}", baseName, counter, ext));
                counter++;
            }
            return fullPath;
        }

        #endregion

        #region Editor Toolbar & Status Updates

        private void UpdateStatusInfo()
        {
            if (_activeDocument == null)
                return;

            Title = "Note TxtMd - " + _activeDocument.DocumentTitle;
            LblDocName.Text = _activeDocument.FileName;
            LblModifiedIndicator.Visibility = _activeDocument.IsModified ? Visibility.Visible : Visibility.Collapsed;

            LblWordCount.Text = _activeDocument.WordCount + (_activeDocument.WordCount == 1 ? " word" : " words");
            LblCharCount.Text = _activeDocument.CharCount + " chars";

            string ext = Path.GetExtension(_activeDocument.FilePath ?? string.Empty).ToLowerInvariant();
            if (ext == ".md" || ext == ".markdown")
                LblDocType.Text = "Markdown";
            else if (ext == ".txt")
                LblDocType.Text = "Plain Text";
            else
                LblDocType.Text = "Markdown / TXT";
        }

        private void TxtEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_activeDocument == null)
                return;

            int caretIndex = TxtEditor.CaretIndex;
            int lineIndex = TxtEditor.GetLineIndexFromCharacterIndex(caretIndex);
            int lineStart = TxtEditor.GetCharacterIndexFromLineIndex(lineIndex);
            int columnIndex = caretIndex - lineStart;

            _activeDocument.CurrentLine = lineIndex + 1;
            _activeDocument.CurrentColumn = columnIndex + 1;
            _activeDocument.CaretIndex = caretIndex;

            LblCursorPos.Text = string.Format("Ln {0}, Col {1}", _activeDocument.CurrentLine, _activeDocument.CurrentColumn);
        }

        private void ToggleWordWrap()
        {
            if (TxtEditor.TextWrapping == TextWrapping.Wrap)
            {
                TxtEditor.TextWrapping = TextWrapping.NoWrap;
                BtnWordWrap.Content = "Wrap: Off";
            }
            else
            {
                TxtEditor.TextWrapping = TextWrapping.Wrap;
                BtnWordWrap.Content = "Wrap: On";
            }
        }

        #endregion

        #region UI Event Handlers

        private void BtnNew_Click(object sender, RoutedEventArgs e) { PerformNew(); }
        private void BtnOpen_Click(object sender, RoutedEventArgs e) { PerformOpen(); }
        private void BtnSave_Click(object sender, RoutedEventArgs e) { PerformSave(); }
        private void BtnSaveAs_Click(object sender, RoutedEventArgs e) { PerformSaveAs(); }
        private void BtnTheme_Click(object sender, RoutedEventArgs e) { ToggleTheme(); }
        private void BtnToggleSidebar_Click(object sender, RoutedEventArgs e) { ToggleSidebar(); }
        private void BtnContextMenu_Click(object sender, RoutedEventArgs e) { ToggleExplorerContextMenu(); }
        private void BtnWordWrap_Click(object sender, RoutedEventArgs e) { ToggleWordWrap(); }
        private void BtnZoomIn_Click(object sender, RoutedEventArgs e) { ZoomIn(); }
        private void BtnZoomOut_Click(object sender, RoutedEventArgs e) { ZoomOut(); }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<DocumentModel> copy = new List<DocumentModel>(_tabs);
            foreach (DocumentModel doc in copy)
            {
                if (!CloseTab(doc))
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    foreach (string item in files)
                    {
                        if (Directory.Exists(item))
                        {
                            LoadFolder(item);
                        }
                        else if (File.Exists(item))
                        {
                            OpenFile(item);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
