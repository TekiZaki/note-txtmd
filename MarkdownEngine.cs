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
