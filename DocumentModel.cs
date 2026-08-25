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
