# Note TxtMd

A lightweight, distraction-free notepad application for Windows supporting plain text (`.txt`) and Markdown (`.md`) with raw editing, split-screen, and rendered viewer modes. Built using native **.NET Framework 4.8** and WPF with Scandinavian design principles.

## Features

- **Theme Persistence (`Ctrl + T`):**
  - Remembers your chosen theme (Light / Dark) across application restarts.
- **Code Block Wrapping & One-Click Copy:**
  - Code blocks wrap text naturally to fit the preview container without horizontal scrollbars.
  - Includes a sleek **"Copy"** button on the top-right of every code block with instant "Copied!" feedback.
- **File Explorer Sidebar (`Ctrl + B`):**
  - Browse directory files and subfolders filtered for `.txt`, `.md`, and `.markdown`.
  - Search filter input to quickly locate notes in large projects.
  - **Sidebar Context Menu:** Right-click any file, folder, or empty sidebar area to access:
    - **`New File...`**: Create a new markdown/text note in that folder and open it instantly.
    - **`Rename...`**: Rename the file/folder on disk and automatically sync open document tabs.
    - **`Delete`**: Safely move the file/folder to the Windows Recycle Bin and close active tabs.
  - Resizable and collapsible sidebar.
- **Windows Explorer Context Menu:**
  - One-click toggle in the toolbar or via `register-context-menu.bat` / `unregister-context-menu.bat`.
  - Right-click any file, folder, or folder background in Windows Explorer to "Open with NoteTxtMd".
- **Modern Multi-Document Tabs:**
  - Open multiple files concurrently in separate tabs (`Ctrl + N`, `Ctrl + O`, `Ctrl + W`).
  - Active tab highlighting, unsaved changes indicator (`*`), tab close buttons (`×`), and `+` new tab button.
  - Cycle through open tabs using `Ctrl + Tab` and `Ctrl + Shift + Tab`.
- **Instant Save in Folder Workspace (`Ctrl + S`):**
  - Saves new untitled files directly into the active folder without dialog popups, auto-naming from the first header.
- **Synchronized Zoom Scaling:**
  - Adjusting font size (`Ctrl + +`, `Ctrl + -`, `Ctrl + 0`, or `Ctrl + MouseWheel`) proportionally scales both the raw text editor and rendered Markdown preview elements (headings, body text, tables, quotes, and code blocks) simultaneously.
- **Tri-Mode View:**
  - **Raw Edit (`Ctrl + 1`):** Distraction-free plain text / Markdown source code editor.
  - **Split View (`Ctrl + 2`):** Live side-by-side editing with instant native rendered Markdown preview.
  - **Markdown Preview (`Ctrl + 3`):** Focused reading mode with clean typography.
- **Native CommonMark Engine:** Parses headings, tables, task lists, code blocks, blockquotes, horizontal rules, and inline styling with zero external dependencies.

## Keyboard Shortcuts

| Shortcut | Action |
| :--- | :--- |
| `Ctrl + B` | Toggle File Explorer Sidebar |
| `Ctrl + Shift + O` | Open Folder in Sidebar |
| `Ctrl + N` | Create new document tab |
| `Ctrl + O` | Open file in new tab |
| `Ctrl + S` | Save active file |
| `Ctrl + Shift + S` | Save As active file |
| `Ctrl + W` | Close active tab |
| `Ctrl + Tab` | Next document tab |
| `Ctrl + Shift + Tab` | Previous document tab |
| `Ctrl + 1` | Switch to Raw Edit mode |
| `Ctrl + 2` | Switch to Split View mode |
| `Ctrl + 3` | Switch to Markdown Preview mode |
| `Ctrl + T` | Toggle Light / Dark theme |
| `Alt + Z` | Toggle Word Wrap |
| `Ctrl + +` / `Ctrl + -` | Increase / decrease zoom (editor + preview) |
| `Ctrl + 0` | Reset zoom |
| `Ctrl + MouseWheel` | Zoom editor and preview in sync |

## Building the Project

Run `build.bat` or compile with MSBuild:

```powershell
& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" NoteTxtMd.csproj /p:Configuration=Release
```

The output executable is generated at `bin\Release\NoteTxtMd.exe`.
