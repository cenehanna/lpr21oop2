using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace lpr21oop2

{

    public partial class Form2 : Form
    {
        private List<int> foundIndexes = new List<int>();
        private int currentIndex = -1;
        public bool IsSaved = false;
        public string DocName = "Документ";
        private System.Windows.Forms.Timer timer;
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();
        private bool isUndoRedoOperation = false;
        private const int MaxHistorySteps = 30;

        public string SavedFileName { get; set; } = null;
        private bool isTextChanged = false;




        public Form2()
        {
            InitializeComponent();
            UpdateTime();



            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; timer.Tick += Timer_Tick;
            timer.Start();

            sbAmount.Text = "Кількість символів: " + richTextBox1.Text.Length.ToString();

            LanguageManager.LanguageChanged += (s, e) => UpdateFormLanguage();

            UpdateFormLanguage();

        }


        private void UpdateFormLanguage()
        {

            foreach (Control control in this.Controls)
            {
                if (control is ToolStrip toolStrip)
                {
                    LanguageManager.UpdateToolStripItems(toolStrip.Items);
                }
            }
        }

        private void UpdateTime()
        {
            sbTime.Text = DateTime.Now.ToLongTimeString();
            sbTime.ToolTipText = DateTime.Today.ToLongDateString();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }
        public void FindAll(string searchText, RichTextBoxFinds options)
        {
            foundIndexes.Clear();
            currentIndex = -1;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Введіть текст для пошуку", "Помилка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int start = 0;
            while (start < richTextBox1.TextLength)
            {
                int index = richTextBox1.Find(searchText, start, richTextBox1.TextLength, options);
                if (index == -1)
                    break;

                foundIndexes.Add(index);
                start = index + searchText.Length;
            }

            if (foundIndexes.Count > 0)
            {
                currentIndex = 0;
                HighlightFound(currentIndex, searchText.Length);
            }
            else
            {
                MessageBox.Show("Жодного збігу не знайдено!", "Пошук",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void HighlightFound(int index, int length)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
            richTextBox1.SelectionColor = richTextBox1.ForeColor;

            richTextBox1.Select(foundIndexes[index], length);
            richTextBox1.SelectionBackColor = Color.Yellow; richTextBox1.SelectionColor = Color.Black;

            richTextBox1.ScrollToCaret();
            richTextBox1.Focus();
        }

        public void FindNext(string searchText)
        {
            if (foundIndexes.Count == 0)
            {
                MessageBox.Show("Немає результатів пошуку.");
                return;
            }

            currentIndex++;
            if (currentIndex >= foundIndexes.Count)
                currentIndex = 0;

            HighlightFound(currentIndex, searchText.Length);
        }

        public void FindPrev(string searchText)
        {
            if (foundIndexes.Count == 0)
            {
                MessageBox.Show("Немає результатів пошуку.");
                return;
            }

            currentIndex--;
            if (currentIndex < 0)
                currentIndex = foundIndexes.Count - 1;

            HighlightFound(currentIndex, searchText.Length);
        }


        private void вирізатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void копіюватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void вставитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        private void виділитиВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            string filePath = this.Tag?.ToString();

            if (string.IsNullOrEmpty(filePath))
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    filePath = sfd.FileName;
                    SavedFileName = filePath;
                    if (Path.GetExtension(filePath).ToLower() == ".rtf")
                        richTextBox1.SaveFile(filePath);
                    else
                        File.WriteAllText(filePath, richTextBox1.Text);

                    this.Tag = filePath;
                    this.DocName = Path.GetFileName(filePath);
                    this.Text = this.DocName;
                    IsSaved = true;
                    isTextChanged = false;
                }
            }
            else
            {
                if (Path.GetExtension(filePath).ToLower() == ".rtf")
                    richTextBox1.SaveFile(filePath);
                else
                    File.WriteAllText(filePath, richTextBox1.Text);

                IsSaved = true;
                isTextChanged = false;
            }
        }


        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                    richTextBox1.SaveFile(sfd.FileName);
                else
                    File.WriteAllText(sfd.FileName, richTextBox1.Text);

                this.Tag = sfd.FileName;
                this.DocName = Path.GetFileName(sfd.FileName);
                IsSaved = true;
                this.Text = this.DocName;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            string fileNameToShow = !string.IsNullOrEmpty(SavedFileName)
    ? Path.GetFileName(SavedFileName)
    : this.Text;
            if (isTextChanged && !IsSaved)
            {
                var result = MessageBox.Show(
                    $"Ви хочете зберегти зміни у файлі \"{fileNameToShow}\"?",
                    "Підтвердження закриття",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    mnuSave_Click(sender, e);

                    if (!IsSaved)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }


        private void HighlightSyntax(RichTextBox rtb)
        {
            int selectionStart = rtb.SelectionStart;
            int selectionLength = rtb.SelectionLength;

            rtb.SuspendLayout();

            rtb.SelectAll();
            rtb.SelectionColor = Color.Black;

            string[] keywords = {
        "if", "else", "while", "for", "foreach", "return", "break", "continue",
        "switch", "case", "default", "do", "try", "catch", "finally", "throw",
        "public", "private", "protected", "internal", "static", "void", "new", "class", "namespace",
        "using", "this", "base", "override", "virtual", "abstract", "sealed", "readonly", "const"
    };

            string[] types = {
        "int", "string", "bool", "float", "double", "decimal", "char", "object", "var", "long", "short"
    };

            string[] literals = { "true", "false", "null" };

            MatchCollection comments = Regex.Matches(rtb.Text, @"//.*?$", RegexOptions.Multiline);
            foreach (Match match in comments)
            {
                rtb.Select(match.Index, match.Length);
                rtb.SelectionColor = Color.Green;
            }

            MatchCollection strings = Regex.Matches(rtb.Text, "\".*?\"");
            foreach (Match match in strings)
            {
                rtb.Select(match.Index, match.Length);
                rtb.SelectionColor = Color.Brown;
            }

            foreach (string keyword in keywords)
            {
                MatchCollection matches = Regex.Matches(rtb.Text, $@"\b{keyword}\b");
                foreach (Match match in matches)
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.Blue;
                }
            }

            foreach (string type in types)
            {
                MatchCollection matches = Regex.Matches(rtb.Text, $@"\b{type}\b");
                foreach (Match match in matches)
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.DarkCyan;
                }
            }

            foreach (string literal in literals)
            {
                MatchCollection matches = Regex.Matches(rtb.Text, $@"\b{literal}\b");
                foreach (Match match in matches)
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.Purple;
                }
            }

            rtb.Select(selectionStart, selectionLength);
            rtb.SelectionColor = Color.Black;

            rtb.ResumeLayout();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isUndoRedoOperation)
            {
                isTextChanged = true;
                IsSaved = false;

                undoStack.Push(richTextBox1.Rtf);
                redoStack.Clear();
            }

            sbAmount.Text = "Кількість символів: " + richTextBox1.Text.Length.ToString();

            HighlightSyntax(richTextBox1);
        }
        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                try
                {
                    isUndoRedoOperation = true;

                    redoStack.Push(richTextBox1.Rtf);

                    richTextBox1.Rtf = undoStack.Pop();
                }
                finally
                {
                    isUndoRedoOperation = false;
                }
            }
            else
            {
                MessageBox.Show("Немає дій для скасування", "Інформація",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                try
                {
                    isUndoRedoOperation = true;

                    undoStack.Push(richTextBox1.Rtf);

                    richTextBox1.Rtf = redoStack.Pop();
                }
                finally
                {
                    isUndoRedoOperation = false;
                }
            }
            else
            {
                MessageBox.Show("Немає дій для повторення", "Інформація",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void InsertImage(this RichTextBox richTextBox, Image image)
        {
            int position = richTextBox.SelectionStart;

            Clipboard.SetImage(image);

            richTextBox.Paste();

            richTextBox.SelectionStart = position + 1;
            richTextBox.ScrollToCaret();
        }
    }

}
