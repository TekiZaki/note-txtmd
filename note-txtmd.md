# Code Dump: note-txtmd

Warning, MainWindow.xaml.cs is over than 601 lines of code, MUST reduce it or modularize it.
Warning, MarkdownEngine.cs is over than 601 lines of code, MUST reduce it or modularize it.

WARNING: YOU (AGENT) SHOULD NOT READ AND EDIT THIS FILE, IT IS FORBIDDEN, ANY CHANGES WILL BE OVERWRITTEN.
Instruction for AI: When making edits, please provide your changes as:
### Summary of Changes:
1. **`path/of/files/changed.ext`**:
   - What changed here.
2. **`path/of/files/changed.ext`**:
   - What changed here 1.
   - What changed here 2.
   - What changed here 3.
NOTE: for code changes, there should not any text/character/symbol/anything between the updated code. The delimiter must have exactly 7 characters.
SEARCH/REPLACE block:
## relative/path/to/file.ext
<<<<<<< SEARCH
old code
=======
new code
>>>>>>> REPLACE

To replace all code in a file (CRITICAL: MUST use this for modularization, total rewrites, or when modifying major portions of files like style.css, HTML, or large code files to avoid tedious and error-prone search/replace blocks):
## relative/path/to/file.ext
<<<<<<< REPLACE_ALL
new file content
>>>>>>> REPLACE_ALL

To create a new file, specify the new path under the ## header and leave the SEARCH block empty:
## relative/path/to/new_file.ext
<<<<<<< SEARCH
=======
new file content
>>>>>>> REPLACE

To move or rename a file, use the '->' operator in the ## header:
## relative/path/to/old.ext -> relative/path/to/new.ext
<<<<<<< SEARCH
=======
>>>>>>> REPLACE

To delete a file or folder (Delete workaround):
- To delete a file, move the file that needs to be deleted to the 'recycle-bin' folder:
## relative/path/to/file.ext -> recycle-bin/file.ext
<<<<<<< SEARCH
=======
>>>>>>> REPLACE

- To delete a folder, rename the folder to be deleted with a name like 'recycle-bin-folder-name':
## relative/path/to/folder-name -> relative/path/to/recycle-bin-folder-name
<<<<<<< SEARCH
=======
>>>>>>> REPLACE

## note-txtmd/agents/instructions.md

## Instructions (Always edit this file when making changes to make the instructions accurate)

This project is about [insert project description]

Anti Emoji, no emoji allowed

Anti Generic UI styles, use simple and modern UI, not generic AI/Colorful UI styles

### List of Changes (Always add new changes here, do not delete any previous changes)

What changes have been made?


## note-txtmd/App.xaml (287 lines)

```xml
<Application x:Class="NoteTxtMd.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <!-- Scandinavian Design Tokens (Default: Light Mode) -->
        <SolidColorBrush x:Key="CanvasBrush" Color="#FFFFFF" />
        <SolidColorBrush x:Key="SurfaceBrush" Color="#FAFAFA" />
        <SolidColorBrush x:Key="PrimaryInkBrush" Color="#111111" />
        <SolidColorBrush x:Key="SecondaryInkBrush" Color="#666666" />
        <SolidColorBrush x:Key="TertiaryInkBrush" Color="#999999" />
        <SolidColorBrush x:Key="BorderBrush" Color="#E5E5E5" />
        <SolidColorBrush x:Key="StrongBorderBrush" Color="#CCCCCC" />
        <SolidColorBrush x:Key="HoverFillBrush" Color="#F2F2F2" />
        <SolidColorBrush x:Key="PressedFillBrush" Color="#E8E8E8" />
        <SolidColorBrush x:Key="SelectedFillBrush" Color="#111111" />
        <SolidColorBrush x:Key="SelectedInkBrush" Color="#FFFFFF" />
        <SolidColorBrush x:Key="ScrollBarThumbBrush" Color="#B8B8B8" />
        <SolidColorBrush x:Key="ScrollBarThumbHoverBrush" Color="#8A8A8A" />
        <SolidColorBrush x:Key="ScrollBarThumbPressedBrush" Color="#606060" />
        <SolidColorBrush x:Key="ScrollBarTrackBrush" Color="Transparent" />

        <!-- Base Font Settings -->
        <FontFamily x:Key="SystemFont">Segoe UI, -apple-system, BlinkMacSystemFont, Roboto, sans-serif</FontFamily>
        <FontFamily x:Key="MonospaceFont">Consolas, 'Cascadia Code', 'Courier New', monospace</FontFamily>

        <!-- ToolBar / Nav Button Style (Dynamic Theme Responding) -->
        <Style x:Key="NavButtonStyle" TargetType="Button">
            <Setter Property="Background" Value="Transparent" />
            <Setter Property="Foreground" Value="{DynamicResource PrimaryInkBrush}" />
            <Setter Property="BorderThickness" Value="1" />
            <Setter Property="BorderBrush" Value="Transparent" />
            <Setter Property="Padding" Value="10,5" />
            <Setter Property="FontSize" Value="12" />
            <Setter Property="FontFamily" Value="{DynamicResource SystemFont}" />
            <Setter Property="FontWeight" Value="Medium" />
            <Setter Property="Cursor" Value="Hand" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border x:Name="border"
                                Background="{TemplateBinding Background}"
                                BorderBrush="{TemplateBinding BorderBrush}"
                                BorderThickness="{TemplateBinding BorderThickness}"
                                CornerRadius="5"
                                SnapsToDevicePixels="True">
                            <ContentPresenter HorizontalAlignment="Center"
                                              VerticalAlignment="Center"
                                              Margin="{TemplateBinding Padding}" />
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter TargetName="border" Property="Background" Value="{DynamicResource HoverFillBrush}" />
                                <Setter TargetName="border" Property="BorderBrush" Value="{DynamicResource BorderBrush}" />
                                <Setter Property="Foreground" Value="{DynamicResource PrimaryInkBrush}" />
                            </Trigger>
                            <Trigger Property="IsPressed" Value="True">
                                <Setter TargetName="border" Property="Background" Value="{DynamicResource PressedFillBrush}" />
                            </Trigger>
                            <Trigger Property="IsEnabled" Value="False">
                                <Setter Property="Foreground" Value="{DynamicResource TertiaryInkBrush}" />
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <!-- Segmented Toggle RadioButton Style -->
        <Style x:Key="SegmentedToggleStyle" TargetType="RadioButton">
            <Setter Property="Background" Value="Transparent" />
            <Setter Property="Foreground" Value="{DynamicResource SecondaryInkBrush}" />
            <Setter Property="BorderThickness" Value="0" />
            <Setter Property="Padding" Value="12,5" />
            <Setter Property="FontSize" Value="12" />
            <Setter Property="FontFamily" Value="{DynamicResource SystemFont}" />
            <Setter Property="FontWeight" Value="Medium" />
            <Setter Property="Cursor" Value="Hand" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="RadioButton">
                        <Border x:Name="border"
                                Background="{TemplateBinding Background}"
                                CornerRadius="4"
                                SnapsToDevicePixels="True">
                            <ContentPresenter HorizontalAlignment="Center"
                                              VerticalAlignment="Center"
                                              Margin="{TemplateBinding Padding}" />
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter TargetName="border" Property="Background" Value="{DynamicResource HoverFillBrush}" />
                                <Setter Property="Foreground" Value="{DynamicResource PrimaryInkBrush}" />
                            </Trigger>
                            <Trigger Property="IsChecked" Value="True">
                                <Setter TargetName="border" Property="Background" Value="{DynamicResource SelectedFillBrush}" />
                                <Setter Property="Foreground" Value="{DynamicResource SelectedInkBrush}" />
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <!-- Context Menu Style -->
        <Style TargetType="ContextMenu">
            <Setter Property="Background" Value="{DynamicResource SurfaceBrush}" />
            <Setter Property="BorderBrush" Value="{DynamicResource BorderBrush}" />
            <Setter Property="BorderThickness" Value="1" />
            <Setter Property="Padding" Value="4" />
            <Setter Property="SnapsToDevicePixels" Value="True" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="ContextMenu">
                        <Border Background="{TemplateBinding Background}"
                                BorderBrush="{TemplateBinding BorderBrush}"
                                BorderThickness="{TemplateBinding BorderThickness}"
                                CornerRadius="6"
                                Padding="{TemplateBinding Padding}">
                            <StackPanel IsItemsHost="True" KeyboardNavigation.DirectionalNavigation="Cycle" />
                        </Border>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <!-- Menu Item Style -->
        <Style TargetType="MenuItem">
            <Setter Property="Background" Value="Transparent" />
            <Setter Property="Foreground" Value="{DynamicResource PrimaryInkBrush}" />
            <Setter Property="FontSize" Value="12" />
            <Setter Property="FontFamily" Value="{DynamicResource SystemFont}" />
            <Setter Property="Padding" Value="10,6" />
            <Setter Property="Cursor" Value="Hand" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="MenuItem">
                        <Border x:Name="border"
                                Background="{TemplateBinding Background}"
                                CornerRadius="4"
                                SnapsToDevicePixels="True">
                            <Grid Margin="{TemplateBinding Padding}">
                                <ContentPresenter ContentSource="Header" RecognizesAccessKey="True" />
                            </Grid>
                        </Border>
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsHighlighted" Value="True">
                                <Setter TargetName="border" Property="Background" Value="{DynamicResource HoverFillBrush}" />
                            </Trigger>
                            <Trigger Property="IsEnabled" Value="False">
                                <Setter Property="Foreground" Value="{DynamicResource TertiaryInkBrush}" />
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <!-- ScrollBar Transparent Track Button -->
        <Style x:Key="ScrollBarPageButtonStyle" TargetType="RepeatButton">
            <Setter Property="IsTabStop" Value="False" />
            <Setter Property="Focusable" Value="False" />
            <Setter Property="Background" Value="Transparent" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="RepeatButton">
                        <Rectangle Fill="{TemplateBinding Background}" Height="{TemplateBinding Height}" Width="{TemplateBinding Width}" />
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <!-- Vertical ScrollBar Thumb Style -->
        <Style x:Key="ScrollBarThumbVerticalStyle" TargetType="Thumb">
            <Setter Property="IsTabStop" Value="False" />
            <Setter Property="Focusable" Value="False" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Thumb">
                        <Border x:Name="thumbBorder"
                                Background="{DynamicResource ScrollBarThumbBrush}"
                                CornerRadius="4"
                                Margin="2,0,2,0"
                                SnapsToDevicePixels="True" />
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter TargetName="thumbBorder" Property="Background" Value="{DynamicResource ScrollBarThumbHoverBrush}" />
                            </Trigger>
                            <Trigger Property="IsDragging" Value="True">
                                <Setter TargetName="thumbBorder" Property="Background" Value="{DynamicResource ScrollBarThumbPressedBrush}" />
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <!-- Horizontal ScrollBar Thumb Style -->
        <Style x:Key="ScrollBarThumbHorizontalStyle" TargetType="Thumb">
            <Setter Property="IsTabStop" Value="False" />
            <Setter Property="Focusable" Value="False" />
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Thumb">
                        <Border x:Name="thumbBorder"
                                Background="{DynamicResource ScrollBarThumbBrush}"
                                CornerRadius="4"
                                Margin="0,2,0,2"
                                SnapsToDevicePixels="True" />
                        <ControlTemplate.Triggers>
                            <Trigger Property="IsMouseOver" Value="True">
                                <Setter TargetName="thumbBorder" Property="Background" Value="{DynamicResource ScrollBarThumbHoverBrush}" />
                            </Trigger>
                            <Trigger Property="IsDragging" Value="True">
                                <Setter TargetName="thumbBorder" Property="Background" Value="{DynamicResource ScrollBarThumbPressedBrush}" />
                            </Trigger>
                        </ControlTemplate.Triggers>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>

        <!-- Global ScrollBar Style -->
        <Style TargetType="ScrollBar">
            <Setter Property="Background" Value="{DynamicResource ScrollBarTrackBrush}" />
            <Setter Property="SnapsToDevicePixels" Value="True" />
            <Style.Triggers>
                <Trigger Property="Orientation" Value="Vertical">
                    <Setter Property="Width" Value="10" />
                    <Setter Property="MinWidth" Value="10" />
                    <Setter Property="Template">
                        <Setter.Value>
                            <ControlTemplate TargetType="ScrollBar">
                                <Grid Background="{TemplateBinding Background}">
                                    <Track x:Name="PART_Track" IsDirectionReversed="True">
                                        <Track.DecreaseRepeatButton>
                                            <RepeatButton Style="{StaticResource ScrollBarPageButtonStyle}" Command="ScrollBar.PageUpCommand" />
                                        </Track.DecreaseRepeatButton>
                                        <Track.Thumb>
                                            <Thumb Style="{StaticResource ScrollBarThumbVerticalStyle}" />
                                        </Track.Thumb>
                                        <Track.IncreaseRepeatButton>
                                            <RepeatButton Style="{StaticResource ScrollBarPageButtonStyle}" Command="ScrollBar.PageDownCommand" />
                                        </Track.IncreaseRepeatButton>
                                    </Track>
                                </Grid>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Trigger>
                <Trigger Property="Orientation" Value="Horizontal">
                    <Setter Property="Height" Value="10" />
                    <Setter Property="MinHeight" Value="10" />
                    <Setter Property="Template">
                        <Setter.Value>
                            <ControlTemplate TargetType="ScrollBar">
                                <Grid Background="{TemplateBinding Background}">
                                    <Track x:Name="PART_Track" IsDirectionReversed="False">
                                        <Track.DecreaseRepeatButton>
                                            <RepeatButton Style="{StaticResource ScrollBarPageButtonStyle}" Command="ScrollBar.PageLeftCommand" />
                                        </Track.DecreaseRepeatButton>
                                        <Track.Thumb>
                                            <Thumb Style="{StaticResource ScrollBarThumbHorizontalStyle}" />
                                        </Track.Thumb>
                                        <Track.IncreaseRepeatButton>
                                            <RepeatButton Style="{StaticResource ScrollBarPageButtonStyle}" Command="ScrollBar.PageRightCommand" />
                                        </Track.IncreaseRepeatButton>
                                    </Track>
                                </Grid>
                            </ControlTemplate>
                        </Setter.Value>
                    </Setter>
                </Trigger>
            </Style.Triggers>
        </Style>

        <!-- ToolTip Style -->
        <Style TargetType="ToolTip">
            <Setter Property="Background" Value="{DynamicResource PrimaryInkBrush}" />
            <Setter Property="Foreground" Value="{DynamicResource CanvasBrush}" />
            <Setter Property="FontSize" Value="11" />
            <Setter Property="FontFamily" Value="{DynamicResource SystemFont}" />
            <Setter Property="BorderThickness" Value="0" />
            <Setter Property="Padding" Value="8,4" />
        </Style>
    </Application.Resources>
</Application>

```

## note-txtmd/App.xaml.cs (27 lines)

```csharp
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

```

## note-txtmd/DocumentModel.cs (232 lines)

```csharp
// ---
// Summary:
// - Purpose: State model for individual open document tabs in NoteTxtMd.
// - Role: Represents an open document with content, file metadata, cursor state, and statistics.
// - Used by: MainWindow and TabBar collections.
// - Depends on: System, System.ComponentModel, System.IO, System.Text.RegularExpressions.
// - Key Responsibilities: Managing tab state, modified flags, word/character calculations.
// - Notes: Implements INotifyPropertyChanged for data binding.
// ---

using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace NoteTxtMd
{
    public class DocumentModel : INotifyPropertyChanged
    {
        private string _id;
        private string _filePath;
        private string _content = string.Empty;
        private bool _isModified;
        private int _currentLine = 1;
        private int _currentColumn = 1;
        private int _caretIndex = 0;
        private int _viewModeIndex = 1; // 0 = Raw, 1 = Split, 2 = Preview

        public event PropertyChangedEventHandler PropertyChanged;

        public DocumentModel()
        {
            _id = Guid.NewGuid().ToString("N");
        }

        public string Id
        {
            get { return _id; }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged("FilePath");
                    OnPropertyChanged("FileName");
                    OnPropertyChanged("TabTitle");
                    OnPropertyChanged("DocumentTitle");
                    OnPropertyChanged("IsMarkdownFile");
                }
            }
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_filePath))
                    return "Untitled";
                return Path.GetFileName(_filePath);
            }
        }

        public string TabTitle
        {
            get
            {
                string name = FileName;
                return _isModified ? name + " *" : name;
            }
        }

        public string DocumentTitle
        {
            get
            {
                return TabTitle;
            }
        }

        public bool IsMarkdownFile
        {
            get
            {
                if (string.IsNullOrEmpty(_filePath))
                    return true;
                string ext = Path.GetExtension(_filePath).ToLowerInvariant();
                return ext == ".md" || ext == ".markdown" || ext == ".mdown";
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value ?? string.Empty;
                    OnPropertyChanged("Content");
                    OnPropertyChanged("WordCount");
                    OnPropertyChanged("LineCount");
                    OnPropertyChanged("CharCount");
                }
            }
        }

        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    OnPropertyChanged("IsModified");
                    OnPropertyChanged("TabTitle");
                    OnPropertyChanged("DocumentTitle");
                }
            }
        }

        public int CurrentLine
        {
            get { return _currentLine; }
            set
            {
                if (_currentLine != value)
                {
                    _currentLine = value;
                    OnPropertyChanged("CurrentLine");
                }
            }
        }

        public int CurrentColumn
        {
            get { return _currentColumn; }
            set
            {
                if (_currentColumn != value)
                {
                    _currentColumn = value;
                    OnPropertyChanged("CurrentColumn");
                }
            }
        }

        public int CaretIndex
        {
            get { return _caretIndex; }
            set
            {
                if (_caretIndex != value)
                {
                    _caretIndex = value;
                    OnPropertyChanged("CaretIndex");
                }
            }
        }

        public int ViewModeIndex
        {
            get { return _viewModeIndex; }
            set
            {
                if (_viewModeIndex != value)
                {
                    _viewModeIndex = value;
                    OnPropertyChanged("ViewModeIndex");
                }
            }
        }

        public int LineCount
        {
            get
            {
                if (string.IsNullOrEmpty(_content))
                    return 1;
                int count = 1;
                int pos = 0;
                while ((pos = _content.IndexOf('\n', pos)) != -1)
                {
                    count++;
                    pos++;
                }
                return count;
            }
        }

        public int CharCount
        {
            get { return _content != null ? _content.Length : 0; }
        }

        public int WordCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_content))
                    return 0;
                MatchCollection matches = Regex.Matches(_content, @"\S+");
                return matches.Count;
            }
        }

        public void Reset(string filePath, string content)
        {
            FilePath = filePath;
            Content = content;
            IsModified = false;
            CurrentLine = 1;
            CurrentColumn = 1;
            CaretIndex = 0;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

```

## note-txtmd/InputDialog.xaml (63 lines)

```xml
<Window x:Class="NoteTxtMd.InputDialog"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Prompt"
        Height="160" Width="380"
        WindowStartupLocation="CenterOwner"
        ResizeMode="NoResize"
        ShowInTaskbar="False"
        Background="{DynamicResource CanvasBrush}"
        Foreground="{DynamicResource PrimaryInkBrush}"
        WindowStyle="SingleBorderWindow"
        Loaded="Window_Loaded">

    <Border Padding="18,14">
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
                <RowDefinition Height="*" />
            </Grid.RowDefinitions>

        <TextBlock x:Name="LblPrompt"
                   Grid.Row="0"
                   Text="Enter name:"
                   FontSize="12"
                   FontFamily="{DynamicResource SystemFont}"
                   Foreground="{DynamicResource PrimaryInkBrush}"
                   Margin="0,0,0,8" />

        <TextBox x:Name="TxtInput"
                 Grid.Row="1"
                 FontSize="13"
                 FontFamily="{DynamicResource SystemFont}"
                 Padding="8,5"
                 Background="{DynamicResource SurfaceBrush}"
                 Foreground="{DynamicResource PrimaryInkBrush}"
                 BorderBrush="{DynamicResource BorderBrush}"
                 BorderThickness="1"
                 KeyDown="TxtInput_KeyDown" />

        <StackPanel Grid.Row="2"
                    Orientation="Horizontal"
                    HorizontalAlignment="Right"
                    VerticalAlignment="Bottom"
                    Margin="0,12,0,0">
            <Button x:Name="BtnCancel"
                    Content="Cancel"
                    Style="{StaticResource NavButtonStyle}"
                    Click="BtnCancel_Click"
                    IsCancel="True"
                    MinWidth="70"
                    Margin="0,0,8,0" />
            <Button x:Name="BtnOk"
                    Content="OK"
                    Style="{StaticResource NavButtonStyle}"
                    Click="BtnOk_Click"
                    IsDefault="True"
                    MinWidth="70" />
        </StackPanel>
        </Grid>
    </Border>
</Window>

```

## note-txtmd/InputDialog.xaml.cs (96 lines)

```csharp
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

```

## note-txtmd/MainWindow.xaml (449 lines)

```xml
<Window x:Class="NoteTxtMd.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:NoteTxtMd"
        mc:Ignorable="d"
        x:Name="RootWindow"
        Title="Note TxtMd"
        Icon="app.ico"
        Height="760" Width="1140"
        MinHeight="450" MinWidth="650"
        Background="{DynamicResource CanvasBrush}"
        WindowStartupLocation="CenterScreen"
        AllowDrop="True"
        Drop="Window_Drop"
        DragOver="Window_DragOver"
        Closing="Window_Closing">

    <Window.InputBindings>
        <KeyBinding Modifiers="Control" Key="N" Command="{x:Static local:MainWindow.NewCommand}" />
        <KeyBinding Modifiers="Control" Key="O" Command="{x:Static local:MainWindow.OpenCommand}" />
        <KeyBinding Modifiers="Control+Shift" Key="O" Command="{x:Static local:MainWindow.OpenFolderCommand}" />
        <KeyBinding Modifiers="Control" Key="S" Command="{x:Static local:MainWindow.SaveCommand}" />
        <KeyBinding Modifiers="Control+Shift" Key="S" Command="{x:Static local:MainWindow.SaveAsCommand}" />
        <KeyBinding Modifiers="Control" Key="W" Command="{x:Static local:MainWindow.CloseTabCommand}" />
        <KeyBinding Modifiers="Control" Key="B" Command="{x:Static local:MainWindow.ToggleSidebarCommand}" />
        <KeyBinding Modifiers="Control" Key="Tab" Command="{x:Static local:MainWindow.NextTabCommand}" />
        <KeyBinding Modifiers="Control+Shift" Key="Tab" Command="{x:Static local:MainWindow.PrevTabCommand}" />
        <KeyBinding Modifiers="Control" Key="D1" Command="{x:Static local:MainWindow.RawViewCommand}" />
        <KeyBinding Modifiers="Control" Key="D2" Command="{x:Static local:MainWindow.SplitViewCommand}" />
        <KeyBinding Modifiers="Control" Key="D3" Command="{x:Static local:MainWindow.PreviewViewCommand}" />
        <KeyBinding Modifiers="Control" Key="T" Command="{x:Static local:MainWindow.ToggleThemeCommand}" />
        <KeyBinding Modifiers="Alt" Key="Z" Command="{x:Static local:MainWindow.ToggleWordWrapCommand}" />
        <KeyBinding Modifiers="Control" Key="OemPlus" Command="{x:Static local:MainWindow.ZoomInCommand}" />
        <KeyBinding Modifiers="Control" Key="Add" Command="{x:Static local:MainWindow.ZoomInCommand}" />
        <KeyBinding Modifiers="Control" Key="OemMinus" Command="{x:Static local:MainWindow.ZoomOutCommand}" />
        <KeyBinding Modifiers="Control" Key="Subtract" Command="{x:Static local:MainWindow.ZoomOutCommand}" />
        <KeyBinding Modifiers="Control" Key="D0" Command="{x:Static local:MainWindow.ZoomResetCommand}" />
    </Window.InputBindings>

    <Grid>
        <Grid.RowDefinitions>
            <!-- Row 0: Top Toolbar -->
            <RowDefinition Height="Auto" />
            <!-- Row 1: Workspace Grid (Sidebar + Editor) -->
            <RowDefinition Height="*" />
            <!-- Row 2: Bottom Status Bar -->
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <!-- Top Toolbar -->
        <Border x:Name="ToolbarBorder"
                Grid.Row="0"
                Background="{DynamicResource SurfaceBrush}"
                BorderBrush="{DynamicResource BorderBrush}"
                BorderThickness="0,0,0,1"
                Padding="12,8">
            <Grid>
                <Grid.ColumnDefinitions>
                    <!-- Left: File Actions -->
                    <ColumnDefinition Width="Auto" />
                    <!-- Middle: View Switcher -->
                    <ColumnDefinition Width="*" />
                    <!-- Right: Editor Tools, Sidebar & Theme -->
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>

                <!-- File Operations -->
                <StackPanel Grid.Column="0" Orientation="Horizontal">
                    <Button x:Name="BtnNew" Content="New" Style="{StaticResource NavButtonStyle}" Click="BtnNew_Click" ToolTip="New Tab (Ctrl+N)" />
                    <Button x:Name="BtnOpen" Content="Open" Style="{StaticResource NavButtonStyle}" Click="BtnOpen_Click" ToolTip="Open File (Ctrl+O)" Margin="4,0,0,0" />
                    <Button x:Name="BtnOpenFolder" Content="Open Folder" Style="{StaticResource NavButtonStyle}" Click="BtnOpenFolder_Click" ToolTip="Open Folder in Sidebar (Ctrl+Shift+O)" Margin="4,0,0,0" />
                    <Button x:Name="BtnSave" Content="Save" Style="{StaticResource NavButtonStyle}" Click="BtnSave_Click" ToolTip="Save File (Ctrl+S)" Margin="4,0,0,0" />
                    <Button x:Name="BtnSaveAs" Content="Save As" Style="{StaticResource NavButtonStyle}" Click="BtnSaveAs_Click" ToolTip="Save As (Ctrl+Shift+S)" Margin="4,0,0,0" />
                </StackPanel>

                <!-- Middle Segmented View Mode Toggle -->
                <Border x:Name="SegmentedBorder"
                        Grid.Column="1"
                        HorizontalAlignment="Center"
                        Background="{DynamicResource SurfaceBrush}"
                        BorderBrush="{DynamicResource BorderBrush}"
                        BorderThickness="1"
                        CornerRadius="6"
                        Padding="3">
                    <StackPanel Orientation="Horizontal">
                        <RadioButton x:Name="RadioRaw"
                                     Content="Raw Edit"
                                     GroupName="ViewMode"
                                     Style="{StaticResource SegmentedToggleStyle}"
                                     Checked="RadioViewMode_Checked"
                                     ToolTip="Raw text editing mode (Ctrl+1)" />
                        <RadioButton x:Name="RadioSplit"
                                     Content="Split View"
                                     GroupName="ViewMode"
                                     IsChecked="True"
                                     Style="{StaticResource SegmentedToggleStyle}"
                                     Checked="RadioViewMode_Checked"
                                     ToolTip="Side-by-side editing and preview (Ctrl+2)"
                                     Margin="2,0" />
                        <RadioButton x:Name="RadioPreview"
                                     Content="Markdown Preview"
                                     GroupName="ViewMode"
                                     Style="{StaticResource SegmentedToggleStyle}"
                                     Checked="RadioViewMode_Checked"
                                     ToolTip="Rendered Markdown viewer (Ctrl+3)" />
                    </StackPanel>
                </Border>

                <!-- Right Editor Actions -->
                <StackPanel Grid.Column="2" Orientation="Horizontal">
                    <Button x:Name="BtnToggleSidebar"
                            Content="Sidebar"
                            Style="{StaticResource NavButtonStyle}"
                            Click="BtnToggleSidebar_Click"
                            ToolTip="Toggle File Explorer Sidebar (Ctrl+B)" />
                    <Button x:Name="BtnContextMenu"
                            Content="Context Menu"
                            Style="{StaticResource NavButtonStyle}"
                            Click="BtnContextMenu_Click"
                            ToolTip="Register or Unregister Windows Explorer Context Menu"
                            Margin="4,0,0,0" />
                    <Button x:Name="BtnTheme"
                            Content="Theme: Dark"
                            Style="{StaticResource NavButtonStyle}"
                            Click="BtnTheme_Click"
                            ToolTip="Toggle Light / Dark Mode (Ctrl+T)"
                            Margin="4,0,0,0" />
                    <Button x:Name="BtnWordWrap"
                            Content="Wrap: On"
                            Style="{StaticResource NavButtonStyle}"
                            Click="BtnWordWrap_Click"
                            ToolTip="Toggle Word Wrap (Alt+Z)"
                            Margin="4,0,0,0" />
                    <Button x:Name="BtnZoomOut"
                            Content="A-"
                            Style="{StaticResource NavButtonStyle}"
                            Click="BtnZoomOut_Click"
                            ToolTip="Decrease Zoom (Ctrl+-)"
                            Margin="4,0,0,0" />
                    <Button x:Name="BtnZoomIn"
                            Content="A+"
                            Style="{StaticResource NavButtonStyle}"
                            Click="BtnZoomIn_Click"
                            ToolTip="Increase Zoom (Ctrl++)"
                            Margin="2,0,0,0" />
                </StackPanel>
            </Grid>
        </Border>

        <!-- Main Body (Sidebar + TabBar + Editor Workspace) -->
        <Grid Grid.Row="1">
            <Grid.ColumnDefinitions>
                <!-- Column 0: Left File Explorer Sidebar -->
                <ColumnDefinition x:Name="ColSidebar" Width="230" MinWidth="140" MaxWidth="500" />
                <!-- Column 1: Sidebar Splitter -->
                <ColumnDefinition x:Name="ColSidebarSplitter" Width="Auto" />
                <!-- Column 2: Document Workspace (Tabs + Editor) -->
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>

            <!-- File Explorer Sidebar -->
            <Border x:Name="SidebarBorder"
                    Grid.Column="0"
                    Background="{DynamicResource SurfaceBrush}"
                    BorderBrush="{DynamicResource BorderBrush}"
                    BorderThickness="0,0,1,0">
                <Grid>
                    <Grid.RowDefinitions>
                        <!-- Sidebar Header -->
                        <RowDefinition Height="Auto" />
                        <!-- Filter Input -->
                        <RowDefinition Height="Auto" />
                        <!-- File Tree List -->
                        <RowDefinition Height="*" />
                    </Grid.RowDefinitions>

                    <!-- Sidebar Header with Folder title & Actions -->
                    <Border Grid.Row="0"
                            Padding="12,10,8,8"
                            BorderBrush="{DynamicResource BorderBrush}"
                            BorderThickness="0,0,0,1">
                        <Grid>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="*" />
                                <ColumnDefinition Width="Auto" />
                            </Grid.ColumnDefinitions>

                            <TextBlock x:Name="LblSidebarFolder"
                                       Text="EXPLORER"
                                       FontSize="11"
                                       FontWeight="SemiBold"
                                       FontFamily="{DynamicResource SystemFont}"
                                       Foreground="{DynamicResource SecondaryInkBrush}"
                                       VerticalAlignment="Center"
                                       TextTrimming="CharacterEllipsis" />

                            <StackPanel Grid.Column="1" Orientation="Horizontal">
                                <Button x:Name="BtnRefreshSidebar"
                                        Content="↻"
                                        FontSize="14"
                                        FontWeight="Medium"
                                        Style="{StaticResource NavButtonStyle}"
                                        Padding="4,0"
                                        Click="BtnRefreshSidebar_Click"
                                        ToolTip="Refresh Files" />
                            </StackPanel>
                        </Grid>
                    </Border>

                    <!-- Search Filter Box -->
                    <Border Grid.Row="1"
                            Padding="8,6"
                            BorderBrush="{DynamicResource BorderBrush}"
                            BorderThickness="0,0,0,1">
                        <TextBox x:Name="TxtSearchFilter"
                                 FontSize="12"
                                 FontFamily="{DynamicResource SystemFont}"
                                 Padding="6,3"
                                 Background="{DynamicResource CanvasBrush}"
                                 Foreground="{DynamicResource PrimaryInkBrush}"
                                 BorderBrush="{DynamicResource BorderBrush}"
                                 BorderThickness="1"
                                 TextChanged="TxtSearchFilter_TextChanged">
                            <TextBox.Style>
                                <Style TargetType="TextBox">
                                    <Setter Property="Template">
                                        <Setter.Value>
                                            <ControlTemplate TargetType="TextBox">
                                                <Border Background="{TemplateBinding Background}"
                                                        BorderBrush="{TemplateBinding BorderBrush}"
                                                        BorderThickness="{TemplateBinding BorderThickness}"
                                                        CornerRadius="4">
                                                    <ScrollViewer x:Name="PART_ContentHost" Margin="{TemplateBinding Padding}" />
                                                </Border>
                                            </ControlTemplate>
                                        </Setter.Value>
                                    </Setter>
                                </Style>
                            </TextBox.Style>
                        </TextBox>
                    </Border>

                    <!-- File Tree / List -->
                    <TreeView x:Name="TreeFiles"
                              Grid.Row="2"
                              Background="Transparent"
                              BorderThickness="0"
                              Padding="4"
                              Foreground="{DynamicResource PrimaryInkBrush}"
                              SelectedItemChanged="TreeFiles_SelectedItemChanged"
                              MouseDoubleClick="TreeFiles_MouseDoubleClick">
                        <TreeView.ItemContainerStyle>
                            <Style TargetType="TreeViewItem">
                                <Setter Property="FontSize" Value="12" />
                                <Setter Property="FontFamily" Value="{DynamicResource SystemFont}" />
                                <Setter Property="Foreground" Value="{DynamicResource PrimaryInkBrush}" />
                                <Setter Property="Padding" Value="4,3" />
                                <Setter Property="Cursor" Value="Hand" />
                                <EventSetter Event="PreviewMouseRightButtonDown" Handler="TreeViewItem_PreviewMouseRightButtonDown" />
                            </Style>
                        </TreeView.ItemContainerStyle>
                    </TreeView>
                </Grid>
            </Border>

            <!-- Sidebar Splitter -->
            <GridSplitter x:Name="SidebarSplitter"
                          Grid.Column="1"
                          Width="4"
                          HorizontalAlignment="Center"
                          VerticalAlignment="Stretch"
                          Background="{DynamicResource BorderBrush}"
                          Cursor="SizeWE" />

            <!-- Document Workspace (Tabs + Editor) -->
            <Grid Grid.Column="2">
                <Grid.RowDefinitions>
                    <!-- Document Tab Bar -->
                    <RowDefinition Height="Auto" />
                    <!-- Editor / Markdown Preview Area -->
                    <RowDefinition Height="*" />
                </Grid.RowDefinitions>

                <!-- Document Tab Bar -->
                <Border x:Name="TabBarBorder"
                        Grid.Row="0"
                        Background="{DynamicResource SurfaceBrush}"
                        BorderBrush="{DynamicResource BorderBrush}"
                        BorderThickness="0,0,0,1"
                        Height="34">
                    <Grid>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="Auto" />
                        </Grid.ColumnDefinitions>

                        <ScrollViewer HorizontalScrollBarVisibility="Auto"
                                      VerticalScrollBarVisibility="Disabled"
                                      Focusable="False">
                            <StackPanel x:Name="TabStackPanel"
                                        Orientation="Horizontal" />
                        </ScrollViewer>

                        <!-- New Tab Plus Button -->
                        <Button x:Name="BtnAddTab"
                                Grid.Column="1"
                                Content="+"
                                Width="28"
                                Height="26"
                                FontSize="16"
                                FontWeight="Light"
                                Padding="0"
                                Margin="4,3,8,3"
                                Style="{StaticResource NavButtonStyle}"
                                Click="BtnNew_Click"
                                ToolTip="New Tab (Ctrl+N)" />
                    </Grid>
                </Border>

                <!-- Editor Workspace Area -->
                <Grid Grid.Row="1" Background="{DynamicResource CanvasBrush}" x:Name="WorkspaceGrid"
                      PreviewMouseWheel="WorkspaceGrid_PreviewMouseWheel">
                    <Grid.ColumnDefinitions>
                        <!-- Column 0: Raw Editor Container -->
                        <ColumnDefinition x:Name="ColEditor" Width="1*" />
                        <!-- Column 1: Splitter -->
                        <ColumnDefinition x:Name="ColSplitter" Width="Auto" />
                        <!-- Column 2: Markdown Viewer Container -->
                        <ColumnDefinition x:Name="ColPreview" Width="1*" />
                    </Grid.ColumnDefinitions>

                    <!-- Raw Text Editor -->
                    <Grid Grid.Column="0" x:Name="EditorContainer">
                        <TextBox x:Name="TxtEditor"
                                 AcceptsReturn="True"
                                 AcceptsTab="True"
                                 TextWrapping="Wrap"
                                 VerticalScrollBarVisibility="Auto"
                                 HorizontalScrollBarVisibility="Auto"
                                 BorderThickness="0"
                                 Padding="24,20"
                                 FontSize="14"
                                 FontFamily="Consolas, 'Cascadia Code', 'Courier New', monospace"
                                 Foreground="{DynamicResource PrimaryInkBrush}"
                                 Background="{DynamicResource CanvasBrush}"
                                 CaretBrush="{DynamicResource PrimaryInkBrush}"
                                 TextChanged="TxtEditor_TextChanged"
                                 SelectionChanged="TxtEditor_SelectionChanged" />
                    </Grid>

                    <!-- Splitter Divider -->
                    <GridSplitter Grid.Column="1"
                                  x:Name="ViewSplitter"
                                  Width="4"
                                  HorizontalAlignment="Center"
                                  VerticalAlignment="Stretch"
                                  Background="{DynamicResource BorderBrush}"
                                  Cursor="SizeWE" />

                    <!-- Native WPF FlowDocument Markdown Viewer -->
                    <Grid Grid.Column="2" x:Name="PreviewContainer" Background="{DynamicResource CanvasBrush}">
                        <FlowDocumentScrollViewer x:Name="ViewerPreview"
                                                  IsToolBarVisible="False"
                                                  VerticalScrollBarVisibility="Auto"
                                                  HorizontalScrollBarVisibility="Disabled"
                                                  BorderThickness="0"
                                                  Background="{DynamicResource CanvasBrush}" />
                    </Grid>
                </Grid>
            </Grid>
        </Grid>

        <!-- Bottom Status Bar -->
        <Border x:Name="StatusBorder"
                Grid.Row="2"
                Background="{DynamicResource SurfaceBrush}"
                BorderBrush="{DynamicResource BorderBrush}"
                BorderThickness="0,1,0,0"
                Padding="16,6">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*" />
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>

                <!-- Left: Document File Name and Dirty Status -->
                <StackPanel Grid.Column="0" Orientation="Horizontal" VerticalAlignment="Center">
                    <TextBlock x:Name="LblDocName"
                               Text="Untitled"
                               FontSize="12"
                               FontFamily="{DynamicResource SystemFont}"
                               Foreground="{DynamicResource PrimaryInkBrush}"
                               FontWeight="Medium" />
                    <TextBlock x:Name="LblModifiedIndicator"
                               Text=" • Unsaved changes"
                               FontSize="12"
                               FontFamily="{DynamicResource SystemFont}"
                               Foreground="{DynamicResource SecondaryInkBrush}"
                               Visibility="Collapsed"
                               Margin="4,0,0,0" />
                </StackPanel>

                <!-- Right: Zoom, Cursor Position, Word Count, Format Type -->
                <StackPanel Grid.Column="1" Orientation="Horizontal" VerticalAlignment="Center">
                    <TextBlock x:Name="LblZoom"
                               Text="100%"
                               FontSize="12"
                               FontFamily="{DynamicResource SystemFont}"
                               Foreground="{DynamicResource SecondaryInkBrush}" />
                    <TextBlock Text="  |  "
                               FontSize="12"
                               Foreground="{DynamicResource TertiaryInkBrush}" />
                    <TextBlock x:Name="LblCursorPos"
                               Text="Ln 1, Col 1"
                               FontSize="12"
                               FontFamily="{DynamicResource SystemFont}"
                               Foreground="{DynamicResource SecondaryInkBrush}" />
                    <TextBlock Text="  |  "
                               FontSize="12"
                               Foreground="{DynamicResource TertiaryInkBrush}" />
                    <TextBlock x:Name="LblWordCount"
                               Text="0 words"
                               FontSize="12"
                               FontFamily="{DynamicResource SystemFont}"
                               Foreground="{DynamicResource SecondaryInkBrush}" />
                    <TextBlock Text="  |  "
                               FontSize="12"
                               Foreground="{DynamicResource TertiaryInkBrush}" />
                    <TextBlock x:Name="LblCharCount"
                               Text="0 chars"
                               FontSize="12"
                               FontFamily="{DynamicResource SystemFont}"
                               Foreground="{DynamicResource SecondaryInkBrush}" />
                    <TextBlock Text="  |  "
                               FontSize="12"
                               Foreground="{DynamicResource TertiaryInkBrush}" />
                    <TextBlock x:Name="LblDocType"
                               Text="Markdown / TXT"
                               FontSize="12"
                               FontFamily="{DynamicResource SystemFont}"
                               Foreground="{DynamicResource SecondaryInkBrush}" />
                </StackPanel>
            </Grid>
        </Border>
    </Grid>
</Window>

```

## note-txtmd/MainWindow.xaml.cs (1478 lines)

```csharp
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

```

## note-txtmd/MarkdownEngine.cs (621 lines)

```csharp
// ---
// Summary:
// - Purpose: Parses Markdown text into native WPF FlowDocument with Scandinavian typography and scalable zoom.
// - Role: Engine / Formatter for Markdown document rendering.
// - Used by: MainWindow preview FlowDocumentScrollViewer.
// - Depends on: PresentationFramework, WindowsBase, System, System.Windows.Documents, System.Windows.Controls.
// - Key Responsibilities: Converting CommonMark structures into rich FlowDocuments with dynamic font sizing.
// - Notes: 100% native WPF implementation targeting .NET Framework 4.8 without external dependencies.
// ---

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NoteTxtMd
{
    public static class MarkdownEngine
    {
        public static FlowDocument RenderToFlowDocument(string markdown, bool isDarkMode, double baseFontSize)
        {
            if (baseFontSize < 8.0) baseFontSize = 8.0;
            if (baseFontSize > 40.0) baseFontSize = 40.0;

            FlowDocument doc = new FlowDocument();
            doc.PagePadding = new Thickness(32, 24, 32, 48);
            doc.FontFamily = new FontFamily("Segoe UI, -apple-system, BlinkMacSystemFont, Roboto, sans-serif");
            doc.FontSize = baseFontSize;
            doc.LineHeight = Math.Round(baseFontSize * 1.6);

            // Scandinavian color palette
            Brush canvasBrush = new SolidColorBrush(isDarkMode ? Color.FromRgb(0x0F, 0x0F, 0x0F) : Color.FromRgb(0xFF, 0xFF, 0xFF));
            Brush textBrush = new SolidColorBrush(isDarkMode ? Color.FromRgb(0xEC, 0xEC, 0xEC) : Color.FromRgb(0x11, 0x11, 0x11));
            Brush secondaryTextBrush = new SolidColorBrush(isDarkMode ? Color.FromRgb(0x9E, 0x9E, 0x9E) : Color.FromRgb(0x66, 0x66, 0x66));
            Brush borderBrush = new SolidColorBrush(isDarkMode ? Color.FromRgb(0x28, 0x28, 0x28) : Color.FromRgb(0xE5, 0xE5, 0xE5));
            Brush codeBgBrush = new SolidColorBrush(isDarkMode ? Color.FromRgb(0x1A, 0x1A, 0x1A) : Color.FromRgb(0xF4, 0xF4, 0xF4));
            Brush quoteBgBrush = new SolidColorBrush(isDarkMode ? Color.FromRgb(0x16, 0x16, 0x16) : Color.FromRgb(0xFA, 0xFA, 0xFA));
            Brush tableHeaderBg = new SolidColorBrush(isDarkMode ? Color.FromRgb(0x1C, 0x1C, 0x1C) : Color.FromRgb(0xF5, 0xF5, 0xF5));

            doc.Background = canvasBrush;
            doc.Foreground = textBrush;

            if (string.IsNullOrEmpty(markdown))
            {
                Paragraph emptyP = new Paragraph(new Run("No content to preview."));
                emptyP.FontStyle = FontStyles.Italic;
                emptyP.Foreground = secondaryTextBrush;
                emptyP.FontSize = baseFontSize;
                doc.Blocks.Add(emptyP);
                return doc;
            }

            string[] lines = markdown.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            bool inCodeBlock = false;
            StringBuilder codeBuffer = new StringBuilder();

            List<string> listItems = new List<string>();
            bool isOrderedList = false;

            List<string> blockquoteLines = new List<string>();
            List<string> tableLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();

                // Fenced code block: ```
                if (trimmed.StartsWith("```"))
                {
                    if (inCodeBlock)
                    {
                        inCodeBlock = false;
                        doc.Blocks.Add(CreateCodeBlockElement(codeBuffer.ToString(), textBrush, secondaryTextBrush, codeBgBrush, borderBrush, baseFontSize));
                        codeBuffer.Clear();
                        continue;
                    }
                    else
                    {
                        FlushPendingFlowBlocks(doc, listItems, ref isOrderedList, blockquoteLines, tableLines, textBrush, secondaryTextBrush, quoteBgBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize);
                        inCodeBlock = true;
                        codeBuffer.Clear();
                        continue;
                    }
                }

                if (inCodeBlock)
                {
                    if (codeBuffer.Length > 0)
                        codeBuffer.AppendLine();
                    codeBuffer.Append(line);
                    continue;
                }

                // Table rows
                if (IsTableRow(trimmed))
                {
                    FlushPendingFlowBlocks(doc, listItems, ref isOrderedList, blockquoteLines, null, textBrush, secondaryTextBrush, quoteBgBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize);
                    tableLines.Add(trimmed);
                    continue;
                }
                else if (tableLines.Count > 0)
                {
                    doc.Blocks.Add(CreateTableElement(tableLines, textBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize));
                    tableLines.Clear();
                }

                // Horizontal rule: ---, ***, ___
                if (Regex.IsMatch(trimmed, @"^(\-{3,}|\*{3,}|_{3,})$"))
                {
                    FlushPendingFlowBlocks(doc, listItems, ref isOrderedList, blockquoteLines, tableLines, textBrush, secondaryTextBrush, quoteBgBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize);
                    doc.Blocks.Add(CreateHorizontalRuleElement(borderBrush, 16, 16));
                    continue;
                }

                // Headings (# Heading)
                Match headingMatch = Regex.Match(line, @"^(#{1,6})\s+(.*)$");
                if (headingMatch.Success)
                {
                    FlushPendingFlowBlocks(doc, listItems, ref isOrderedList, blockquoteLines, tableLines, textBrush, secondaryTextBrush, quoteBgBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize);
                    int level = headingMatch.Groups[1].Length;
                    string text = headingMatch.Groups[2].Value.Trim();
                    doc.Blocks.Add(CreateHeadingElement(level, text, textBrush, borderBrush, codeBgBrush, baseFontSize));
                    continue;
                }

                // Blockquotes (> Quote)
                if (line.TrimStart().StartsWith(">"))
                {
                    FlushPendingFlowBlocks(doc, listItems, ref isOrderedList, null, tableLines, textBrush, secondaryTextBrush, quoteBgBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize);
                    string quoteContent = Regex.Replace(line.TrimStart(), @"^>\s?", "");
                    blockquoteLines.Add(quoteContent);
                    continue;
                }
                else if (blockquoteLines.Count > 0)
                {
                    doc.Blocks.Add(CreateBlockquoteElement(blockquoteLines, secondaryTextBrush, quoteBgBrush, textBrush, codeBgBrush, baseFontSize));
                    blockquoteLines.Clear();
                }

                // Task list item: - [ ] or - [x]
                Match taskMatch = Regex.Match(trimmed, @"^[-*+]\s+\[([ xX])\]\s+(.*)$");
                if (taskMatch.Success)
                {
                    FlushPendingFlowBlocks(doc, listItems, ref isOrderedList, blockquoteLines, tableLines, textBrush, secondaryTextBrush, quoteBgBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize);
                    bool isChecked = taskMatch.Groups[1].Value.ToLower() == "x";
                    string itemText = taskMatch.Groups[2].Value;
                    doc.Blocks.Add(CreateTaskListItemElement(isChecked, itemText, textBrush, codeBgBrush, baseFontSize));
                    continue;
                }

                // Bullet list item: -, *, +
                Match ulMatch = Regex.Match(line, @"^(\s*)[-*+]\s+(.*)$");
                if (ulMatch.Success)
                {
                    if (isOrderedList && listItems.Count > 0)
                    {
                        doc.Blocks.Add(CreateListElement(listItems, true, textBrush, codeBgBrush, baseFontSize));
                        listItems.Clear();
                    }
                    isOrderedList = false;
                    listItems.Add(ulMatch.Groups[2].Value);
                    continue;
                }

                // Numbered list item: 1.
                Match olMatch = Regex.Match(line, @"^(\s*)\d+\.\s+(.*)$");
                if (olMatch.Success)
                {
                    if (!isOrderedList && listItems.Count > 0)
                    {
                        doc.Blocks.Add(CreateListElement(listItems, false, textBrush, codeBgBrush, baseFontSize));
                        listItems.Clear();
                    }
                    isOrderedList = true;
                    listItems.Add(olMatch.Groups[2].Value);
                    continue;
                }

                // If not in list, flush list
                if (listItems.Count > 0)
                {
                    doc.Blocks.Add(CreateListElement(listItems, isOrderedList, textBrush, codeBgBrush, baseFontSize));
                    listItems.Clear();
                }

                // Blank line
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    continue;
                }

                // Normal paragraph
                Paragraph p = new Paragraph();
                p.Margin = new Thickness(0, 0, 0, Math.Round(baseFontSize * 0.8));
                p.FontSize = baseFontSize;
                p.LineHeight = Math.Round(baseFontSize * 1.6);
                PopulateInlines(p.Inlines, trimmed, textBrush, codeBgBrush, baseFontSize);
                doc.Blocks.Add(p);
            }

            // Flush remaining buffers at EOF
            if (inCodeBlock)
            {
                doc.Blocks.Add(CreateCodeBlockElement(codeBuffer.ToString(), textBrush, secondaryTextBrush, codeBgBrush, borderBrush, baseFontSize));
            }
            FlushPendingFlowBlocks(doc, listItems, ref isOrderedList, blockquoteLines, tableLines, textBrush, secondaryTextBrush, quoteBgBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize);

            return doc;
        }

        private static void FlushPendingFlowBlocks(FlowDocument doc,
            List<string> listItems, ref bool isOrderedList,
            List<string> blockquoteLines, List<string> tableLines,
            Brush textBrush, Brush secondaryTextBrush, Brush quoteBgBrush,
            Brush borderBrush, Brush tableHeaderBg, Brush codeBgBrush, double baseFontSize)
        {
            if (listItems != null && listItems.Count > 0)
            {
                doc.Blocks.Add(CreateListElement(listItems, isOrderedList, textBrush, codeBgBrush, baseFontSize));
                listItems.Clear();
            }
            if (blockquoteLines != null && blockquoteLines.Count > 0)
            {
                doc.Blocks.Add(CreateBlockquoteElement(blockquoteLines, secondaryTextBrush, quoteBgBrush, textBrush, codeBgBrush, baseFontSize));
                blockquoteLines.Clear();
            }
            if (tableLines != null && tableLines.Count > 0)
            {
                doc.Blocks.Add(CreateTableElement(tableLines, textBrush, borderBrush, tableHeaderBg, codeBgBrush, baseFontSize));
                tableLines.Clear();
            }
        }

        private static Block CreateHeadingElement(int level, string text, Brush textBrush, Brush borderBrush, Brush codeBgBrush, double baseFontSize)
        {
            double scale = 1.7;
            switch (level)
            {
                case 1: scale = 1.75; break;
                case 2: scale = 1.45; break;
                case 3: scale = 1.25; break;
                case 4: scale = 1.10; break;
                case 5: scale = 1.00; break;
                case 6: scale = 0.92; break;
            }

            double fontSize = Math.Round(baseFontSize * scale);
            double topMargin = Math.Round(baseFontSize * 1.3);
            double bottomMargin = Math.Round(baseFontSize * 0.5);

            Paragraph p = new Paragraph();
            p.FontSize = fontSize;
            p.FontWeight = FontWeights.SemiBold;
            p.Foreground = textBrush;
            p.Margin = new Thickness(0, topMargin, 0, bottomMargin);

            PopulateInlines(p.Inlines, text, textBrush, codeBgBrush, fontSize);

            if (level <= 2)
            {
                Section sec = new Section();
                sec.Blocks.Add(p);
                sec.Blocks.Add(CreateHorizontalRuleElement(borderBrush, 0, Math.Round(baseFontSize * 0.5)));
                return sec;
            }

            return p;
        }

        private static Block CreateHorizontalRuleElement(Brush borderBrush, double top, double bottom)
        {
            Rectangle rect = new Rectangle();
            rect.Height = 1;
            rect.Fill = borderBrush;
            rect.Margin = new Thickness(0, top, 0, bottom);
            rect.HorizontalAlignment = HorizontalAlignment.Stretch;

            BlockUIContainer container = new BlockUIContainer(rect);
            container.Margin = new Thickness(0);
            return container;
        }

        private static Block CreateCodeBlockElement(string code, Brush textBrush, Brush secondaryTextBrush, Brush bgBrush, Brush borderBrush, double baseFontSize)
        {
            double codeFontSize = Math.Max(9.0, Math.Round(baseFontSize * 0.9));

            Grid grid = new Grid();

            TextBox tb = new TextBox();
            tb.Text = code;
            tb.IsReadOnly = true;
            tb.FontFamily = new FontFamily("Consolas, 'Cascadia Code', 'Courier New', monospace");
            tb.FontSize = codeFontSize;
            tb.Background = Brushes.Transparent;
            tb.Foreground = textBrush;
            tb.BorderThickness = new Thickness(0);
            tb.Padding = new Thickness(0, 0, 52, 0);
            tb.TextWrapping = TextWrapping.Wrap;
            tb.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            tb.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            grid.Children.Add(tb);

            Button copyBtn = new Button();
            copyBtn.Content = "Copy";
            copyBtn.FontSize = Math.Max(9.0, Math.Round(baseFontSize * 0.75));
            copyBtn.FontFamily = new FontFamily("Segoe UI, -apple-system, sans-serif");
            copyBtn.Foreground = secondaryTextBrush;
            copyBtn.Background = Brushes.Transparent;
            copyBtn.BorderBrush = borderBrush;
            copyBtn.BorderThickness = new Thickness(1);
            copyBtn.Padding = new Thickness(7, 2, 7, 2);
            copyBtn.HorizontalAlignment = HorizontalAlignment.Right;
            copyBtn.VerticalAlignment = VerticalAlignment.Top;
            copyBtn.Cursor = Cursors.Hand;
            copyBtn.ToolTip = "Copy code to clipboard";

            string codeToCopy = code;
            copyBtn.Click += delegate(object s, RoutedEventArgs e)
            {
                try
                {
                    Clipboard.SetText(codeToCopy);
                    copyBtn.Content = "Copied!";
                    System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1.5);
                    timer.Tick += delegate(object ts, EventArgs te)
                    {
                        timer.Stop();
                        copyBtn.Content = "Copy";
                    };
                    timer.Start();
                }
                catch { }
            };

            grid.Children.Add(copyBtn);

            Border border = new Border();
            border.Background = bgBrush;
            border.BorderBrush = borderBrush;
            border.BorderThickness = new Thickness(1);
            border.CornerRadius = new CornerRadius(5);
            border.Padding = new Thickness(14, 12, 14, 12);
            border.Margin = new Thickness(0, Math.Round(baseFontSize * 0.5), 0, Math.Round(baseFontSize * 0.9));
            border.Child = grid;

            BlockUIContainer container = new BlockUIContainer(border);
            container.Margin = new Thickness(0);
            return container;
        }

        private static Block CreateBlockquoteElement(List<string> lines, Brush textBrush, Brush bgBrush, Brush mainTextBrush, Brush codeBgBrush, double baseFontSize)
        {
            double quoteFontSize = Math.Max(9.0, Math.Round(baseFontSize * 0.95));

            StackPanel sp = new StackPanel();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                TextBlock tb = new TextBlock();
                tb.TextWrapping = TextWrapping.Wrap;
                tb.FontSize = quoteFontSize;
                tb.Foreground = textBrush;
                tb.Margin = new Thickness(0, 2, 0, 4);
                PopulateTextBlockInlines(tb.Inlines, line, textBrush, codeBgBrush, quoteFontSize);
                sp.Children.Add(tb);
            }

            Border border = new Border();
            border.Background = bgBrush;
            border.BorderBrush = mainTextBrush;
            border.BorderThickness = new Thickness(3, 0, 0, 0);
            border.CornerRadius = new CornerRadius(0, 4, 4, 0);
            border.Padding = new Thickness(12, 8, 12, 8);
            border.Margin = new Thickness(0, Math.Round(baseFontSize * 0.4), 0, Math.Round(baseFontSize * 0.8));
            border.Child = sp;

            BlockUIContainer container = new BlockUIContainer(border);
            return container;
        }

        private static Block CreateTaskListItemElement(bool isChecked, string text, Brush textBrush, Brush codeBgBrush, double baseFontSize)
        {
            CheckBox cb = new CheckBox();
            cb.IsChecked = isChecked;
            cb.IsEnabled = false;
            cb.VerticalAlignment = VerticalAlignment.Center;
            cb.Margin = new Thickness(0, 0, 8, 0);

            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontSize = baseFontSize;
            tb.Foreground = textBrush;
            PopulateTextBlockInlines(tb.Inlines, text, textBrush, codeBgBrush, baseFontSize);

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(cb);
            sp.Children.Add(tb);
            sp.Margin = new Thickness(4, 2, 0, 4);

            BlockUIContainer container = new BlockUIContainer(sp);
            return container;
        }

        private static Block CreateListElement(List<string> items, bool isOrdered, Brush textBrush, Brush codeBgBrush, double baseFontSize)
        {
            List list = new List();
            list.MarkerStyle = isOrdered ? TextMarkerStyle.Decimal : TextMarkerStyle.Disc;
            list.Margin = new Thickness(16, 4, 0, Math.Round(baseFontSize * 0.7));
            list.FontSize = baseFontSize;

            foreach (string item in items)
            {
                Paragraph p = new Paragraph();
                p.Margin = new Thickness(0, 2, 0, 2);
                p.FontSize = baseFontSize;
                p.LineHeight = Math.Round(baseFontSize * 1.5);
                PopulateInlines(p.Inlines, item, textBrush, codeBgBrush, baseFontSize);
                list.ListItems.Add(new ListItem(p));
            }

            return list;
        }

        private static Block CreateTableElement(List<string> rows, Brush textBrush, Brush borderBrush, Brush headerBg, Brush codeBgBrush, double baseFontSize)
        {
            if (rows.Count == 0) return new Paragraph();

            double tableFontSize = Math.Max(9.0, Math.Round(baseFontSize * 0.95));
            int startIndex = 0;
            bool hasHeader = rows.Count >= 2 && Regex.IsMatch(rows[1], @"^\|?\s*[:\-]+(\s*\|\s*[:\-]+)*\s*\|?$");

            string[] headerCols = hasHeader ? SplitTableRow(rows[0]) : SplitTableRow(rows[0]);
            int colCount = headerCols.Length;

            Grid grid = new Grid();
            grid.Margin = new Thickness(0, Math.Round(baseFontSize * 0.5), 0, Math.Round(baseFontSize * 0.9));

            for (int c = 0; c < colCount; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            int rowIdx = 0;

            if (hasHeader)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                for (int c = 0; c < headerCols.Length; c++)
                {
                    Border cell = new Border();
                    cell.Background = headerBg;
                    cell.BorderBrush = borderBrush;
                    cell.BorderThickness = new Thickness(1);
                    cell.Padding = new Thickness(10, 7, 10, 7);

                    TextBlock tb = new TextBlock();
                    tb.FontWeight = FontWeights.SemiBold;
                    tb.FontSize = tableFontSize;
                    tb.Foreground = textBrush;
                    tb.TextWrapping = TextWrapping.Wrap;
                    PopulateTextBlockInlines(tb.Inlines, headerCols[c], textBrush, codeBgBrush, tableFontSize);
                    cell.Child = tb;

                    Grid.SetRow(cell, 0);
                    Grid.SetColumn(cell, c);
                    grid.Children.Add(cell);
                }
                rowIdx = 1;
                startIndex = 2;
            }

            for (int r = startIndex; r < rows.Count; r++)
            {
                if (Regex.IsMatch(rows[r], @"^\|?\s*[:\-]+(\s*\|\s*[:\-]+)*\s*\|?$"))
                    continue;

                string[] cells = SplitTableRow(rows[r]);
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                for (int c = 0; c < colCount; c++)
                {
                    string content = c < cells.Length ? cells[c] : string.Empty;
                    Border cell = new Border();
                    cell.BorderBrush = borderBrush;
                    cell.BorderThickness = new Thickness(1);
                    cell.Padding = new Thickness(10, 6, 10, 6);

                    TextBlock tb = new TextBlock();
                    tb.FontSize = tableFontSize;
                    tb.Foreground = textBrush;
                    tb.TextWrapping = TextWrapping.Wrap;
                    PopulateTextBlockInlines(tb.Inlines, content, textBrush, codeBgBrush, tableFontSize);
                    cell.Child = tb;

                    Grid.SetRow(cell, rowIdx);
                    Grid.SetColumn(cell, c);
                    grid.Children.Add(cell);
                }
                rowIdx++;
            }

            return new BlockUIContainer(grid);
        }

        private static void PopulateInlines(InlineCollection inlines, string text, Brush textBrush, Brush codeBgBrush, double baseFontSize)
        {
            if (string.IsNullOrEmpty(text)) return;
            ParseInlineFormatting(text, inlines, textBrush, codeBgBrush, baseFontSize);
        }

        private static void PopulateTextBlockInlines(System.Windows.Documents.InlineCollection inlines, string text, Brush textBrush, Brush codeBgBrush, double baseFontSize)
        {
            if (string.IsNullOrEmpty(text)) return;
            ParseInlineFormatting(text, inlines, textBrush, codeBgBrush, baseFontSize);
        }

        private static void ParseInlineFormatting(string text, InlineCollection inlines, Brush textBrush, Brush codeBgBrush, double baseFontSize)
        {
            // Token regex pattern: `code`, [link](url), **bold**, *italic*, ~~strikethrough~~
            string pattern = @"(`(?<code>[^`]+)`)|(\[(?<linkText>[^\]]+)\]\((?<linkUrl>[^)]+)\))|(\*\*(?<boldText>[^*]+)\*\*)|(\*(?<italicText>[^*]+)\*)|(~~(?<delText>[^~]+)~~)";

            int lastIndex = 0;
            MatchCollection matches = Regex.Matches(text, pattern);

            foreach (Match m in matches)
            {
                if (m.Index > lastIndex)
                {
                    inlines.Add(new Run(text.Substring(lastIndex, m.Index - lastIndex)));
                }

                if (m.Groups["code"].Success)
                {
                    Span codeSpan = new Span(new Run(m.Groups["code"].Value));
                    codeSpan.FontFamily = new FontFamily("Consolas, 'Cascadia Code', monospace");
                    codeSpan.FontSize = Math.Max(9.0, Math.Round(baseFontSize * 0.9));
                    codeSpan.Background = codeBgBrush;
                    inlines.Add(codeSpan);
                }
                else if (m.Groups["linkText"].Success)
                {
                    string linkText = m.Groups["linkText"].Value;
                    string linkUrl = m.Groups["linkUrl"].Value;
                    Hyperlink link = new Hyperlink(new Run(linkText));
                    try
                    {
                        link.NavigateUri = new Uri(linkUrl);
                        link.RequestNavigate += delegate(object s, System.Windows.Navigation.RequestNavigateEventArgs e)
                        {
                            try
                            {
                                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                            }
                            catch { }
                            e.Handled = true;
                        };
                    }
                    catch { }
                    link.Foreground = textBrush;
                    link.TextDecorations = TextDecorations.Underline;
                    inlines.Add(link);
                }
                else if (m.Groups["boldText"].Success)
                {
                    Bold b = new Bold(new Run(m.Groups["boldText"].Value));
                    inlines.Add(b);
                }
                else if (m.Groups["italicText"].Success)
                {
                    Italic it = new Italic(new Run(m.Groups["italicText"].Value));
                    inlines.Add(it);
                }
                else if (m.Groups["delText"].Success)
                {
                    Span delSpan = new Span(new Run(m.Groups["delText"].Value));
                    delSpan.TextDecorations = TextDecorations.Strikethrough;
                    inlines.Add(delSpan);
                }

                lastIndex = m.Index + m.Length;
            }

            if (lastIndex < text.Length)
            {
                inlines.Add(new Run(text.Substring(lastIndex)));
            }
        }

        private static bool IsTableRow(string line)
        {
            return line.StartsWith("|") && line.EndsWith("|") && line.Length > 2;
        }

        private static string[] SplitTableRow(string row)
        {
            string clean = row.Trim();
            if (clean.StartsWith("|")) clean = clean.Substring(1);
            if (clean.EndsWith("|")) clean = clean.Substring(0, clean.Length - 1);

            string[] parts = clean.Split('|');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }
            return parts;
        }
    }
}

```

## note-txtmd/NoteTxtMd.csproj (83 lines)

```xml
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003" DefaultTargets="Build">
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{4B9D2C68-1A90-410F-8762-2BEF5234D9A0}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <RootNamespace>NoteTxtMd</RootNamespace>
    <AssemblyName>NoteTxtMd</AssemblyName>
    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <ProjectTypeGuids>{60dc8134-eba5-43b8-bcc9-bb4bc16c2548};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
    <WarningLevel>4</WarningLevel>
    <ApplicationIcon>app.ico</ApplicationIcon>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Xml" />
    <Reference Include="System.Xaml">
      <RequiredTargetFramework>4.0</RequiredTargetFramework>
    </Reference>
    <Reference Include="WindowsBase" />
    <Reference Include="PresentationCore" />
    <Reference Include="PresentationFramework" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="Microsoft.VisualBasic" />
  </ItemGroup>
  <ItemGroup>
    <ApplicationDefinition Include="App.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </ApplicationDefinition>
    <Page Include="MainWindow.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Page Include="InputDialog.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
    </Page>
    <Compile Include="App.xaml.cs">
      <DependentUpon>App.xaml</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="MainWindow.xaml.cs">
      <DependentUpon>MainWindow.xaml</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="InputDialog.xaml.cs">
      <DependentUpon>InputDialog.xaml</DependentUpon>
      <SubType>Code</SubType>
    </Compile>
    <Compile Include="DocumentModel.cs" />
    <Compile Include="MarkdownEngine.cs" />
    <Resource Include="app.ico" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>

```

## note-txtmd/build.bat (28 lines)

```batch
@echo off
setlocal
echo Building NoteTxtMd (.NET Framework 4.8)...

set MSBUILD="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
if not exist %MSBUILD% (
    set MSBUILD="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
)

if not exist %MSBUILD% (
    echo [ERROR] MSBuild for .NET Framework 4.0/4.8 was not found.
    pause
    exit /b 1
)

%MSBUILD% "%~dp0NoteTxtMd.csproj" /p:Configuration=Release /v:m

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCESS] Build succeeded!
    echo Output binary: "%~dp0bin\Release\NoteTxtMd.exe"
) else (
    echo.
    echo [ERROR] Build failed.
)

endlocal

```

## note-txtmd/register-context-menu.bat (33 lines)

```batch
@echo off
setlocal
echo Registering "Open with NoteTxtMd" in Windows Explorer context menu...

set "EXEPATH=%~dp0bin\Release\NoteTxtMd.exe"

if not exist "%EXEPATH%" (
    echo [ERROR] NoteTxtMd.exe was not found at: "%EXEPATH%"
    echo Please run build.bat first.
    pause
    exit /b 1
)

:: 1. Directory context menu (right-click folder)
reg add "HKCU\Software\Classes\Directory\shell\NoteTxtMd" /ve /d "Open with NoteTxtMd" /f >nul
reg add "HKCU\Software\Classes\Directory\shell\NoteTxtMd" /v "Icon" /d "%EXEPATH%" /f >nul
reg add "HKCU\Software\Classes\Directory\shell\NoteTxtMd\command" /ve /d "\"%EXEPATH%\" \"%%1\"" /f >nul

:: 2. Directory Background context menu (right-click empty space in folder)
reg add "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd" /ve /d "Open with NoteTxtMd" /f >nul
reg add "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd" /v "Icon" /d "%EXEPATH%" /f >nul
reg add "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd\command" /ve /d "\"%EXEPATH%\" \"%%V\"" /f >nul

:: 3. File context menu (right-click any file)
reg add "HKCU\Software\Classes\*\shell\NoteTxtMd" /ve /d "Open with NoteTxtMd" /f >nul
reg add "HKCU\Software\Classes\*\shell\NoteTxtMd" /v "Icon" /d "%EXEPATH%" /f >nul
reg add "HKCU\Software\Classes\*\shell\NoteTxtMd\command" /ve /d "\"%EXEPATH%\" \"%%1\"" /f >nul

echo.
echo [SUCCESS] "Open with NoteTxtMd" successfully added to Windows Explorer context menu!
pause
endlocal

```

## note-txtmd/unregister-context-menu.bat (13 lines)

```batch
@echo off
setlocal
echo Removing "Open with NoteTxtMd" from Windows Explorer context menu...

reg delete "HKCU\Software\Classes\Directory\shell\NoteTxtMd" /f >nul 2>&1
reg delete "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd" /f >nul 2>&1
reg delete "HKCU\Software\Classes\*\shell\NoteTxtMd" /f >nul 2>&1

echo.
echo [SUCCESS] "Open with NoteTxtMd" context menu items removed.
pause
endlocal

```

