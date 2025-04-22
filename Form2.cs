using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static lpr21oop2.Form1;

namespace lpr21oop2

{
 
    public partial class Form2 : Form
    {
        private List<int> foundIndexes = new List<int>();
        private int currentIndex = -1;
        // Переменная для отслеживания сохранения документа
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
          


            // Налаштування таймера
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // Оновлення кожну секунду
            timer.Tick += Timer_Tick;
            timer.Start();

            // Інші ініціалізації...
            sbAmount.Text = "Кількість символів: " + richTextBox1.Text.Length.ToString();
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
                // Використовуємо правильне перевантаження методу Find
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
            // Знімаємо попереднє виділення
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
            richTextBox1.SelectionColor = richTextBox1.ForeColor;

            // Виділяємо знайдений текст
            richTextBox1.Select(foundIndexes[index], length);
            richTextBox1.SelectionBackColor = Color.Yellow;  // або будь-який інший колір
            richTextBox1.SelectionColor = Color.Black;

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

        
        // Обработчик события "Вырезать"
        private void вирізатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        // Обработчик события "Копировать"
        private void копіюватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        // Обработчик события "Вставить"
        private void вставитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        // Обработчик события "Удалить"
        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        // Обработчик события "Выделить все"
        private void виділитиВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        // Обработчик события "Сохранить"
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
                    SavedFileName = filePath; // Зберігаємо шлях до файлу

                    if (Path.GetExtension(filePath).ToLower() == ".rtf")
                        richTextBox1.SaveFile(filePath);
                    else
                        File.WriteAllText(filePath, richTextBox1.Text);

                    this.Tag = filePath;
                    this.DocName = Path.GetFileName(filePath);
                    this.Text = this.DocName;
                    IsSaved = true;
                    isTextChanged = false; // Скидаємо прапорець змін
                }
            }
            else
            {
                if (Path.GetExtension(filePath).ToLower() == ".rtf")
                    richTextBox1.SaveFile(filePath);
                else
                    File.WriteAllText(filePath, richTextBox1.Text);

                IsSaved = true;
                isTextChanged = false; // Скидаємо прапорець змін
            }
        }


        // Обработчик события "Сохранить как"
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем файл по новому пути и помечаем как сохранённый
                if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                    richTextBox1.SaveFile(sfd.FileName);
                else
                    File.WriteAllText(sfd.FileName, richTextBox1.Text);

                // Обновляем имя документа и его состояние
                this.Tag = sfd.FileName;
                this.DocName = Path.GetFileName(sfd.FileName);
                IsSaved = true;
                this.Text = this.DocName;
            }
        }

        // Обработчик события закрытия формы
        // Обробник події закриття форми
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Визначаємо ім'я файлу для відображення
            string fileNameToShow = !string.IsNullOrEmpty(SavedFileName)
                ? Path.GetFileName(SavedFileName)
                : this.Text; // Якщо файл не зберігався, беремо заголовок форми

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

                    // Якщо користувач скасував збереження, відміняємо закриття
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
            // Зберігаємо позицію курсора
            int selectionStart = rtb.SelectionStart;
            int selectionLength = rtb.SelectionLength;

            rtb.SuspendLayout();

            // Встановлюємо весь текст у чорний
            rtb.SelectAll();
            rtb.SelectionColor = Color.Black;

            // 🔷 Ключові слова
            string[] keywords = {
        "if", "else", "while", "for", "foreach", "return", "break", "continue",
        "switch", "case", "default", "do", "try", "catch", "finally", "throw",
        "public", "private", "protected", "internal", "static", "void", "new", "class", "namespace",
        "using", "this", "base", "override", "virtual", "abstract", "sealed", "readonly", "const"
    };

            // 🟣 Типи даних
            string[] types = {
        "int", "string", "bool", "float", "double", "decimal", "char", "object", "var", "long", "short"
    };

            // 🟠 Літерали
            string[] literals = { "true", "false", "null" };

            // 🔶 Коментарі
            MatchCollection comments = Regex.Matches(rtb.Text, @"//.*?$", RegexOptions.Multiline);
            foreach (Match match in comments)
            {
                rtb.Select(match.Index, match.Length);
                rtb.SelectionColor = Color.Green;
            }

            // 🔵 Рядки в лапках
            MatchCollection strings = Regex.Matches(rtb.Text, "\".*?\"");
            foreach (Match match in strings)
            {
                rtb.Select(match.Index, match.Length);
                rtb.SelectionColor = Color.Brown;
            }

            // 🔷 Ключові слова
            foreach (string keyword in keywords)
            {
                MatchCollection matches = Regex.Matches(rtb.Text, $@"\b{keyword}\b");
                foreach (Match match in matches)
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.Blue;
                }
            }

            // 🟣 Типи
            foreach (string type in types)
            {
                MatchCollection matches = Regex.Matches(rtb.Text, $@"\b{type}\b");
                foreach (Match match in matches)
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.DarkCyan;
                }
            }

            // 🟠 Літерали
            foreach (string literal in literals)
            {
                MatchCollection matches = Regex.Matches(rtb.Text, $@"\b{literal}\b");
                foreach (Match match in matches)
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.Purple;
                }
            }

            // Відновлюємо попереднє виділення
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

                // Зберігаємо стан для Undo/Redo
                undoStack.Push(richTextBox1.Rtf);
                redoStack.Clear();
            }

            sbAmount.Text = "Кількість символів: " + richTextBox1.Text.Length.ToString();

            // Викликаємо підсвічування синтаксису, якщо потрібно
             HighlightSyntax(richTextBox1); 
        }
        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                try
                {
                    isUndoRedoOperation = true;

                    // Зберігаємо поточний стан для redo
                    redoStack.Push(richTextBox1.Rtf);

                    // Відновлюємо попередній стан
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

                    // Зберігаємо поточний стан для undo
                    undoStack.Push(richTextBox1.Rtf);

                    // Відновлюємо наступний стан
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
            // Зберігаємо поточну позицію курсора
            int position = richTextBox.SelectionStart;

            // Копіюємо зображення в буфер обміну
            Clipboard.SetImage(image);

            // Вставляємо з буфера
            richTextBox.Paste();

            // Відновлюємо позицію курсора після вставки
            richTextBox.SelectionStart = position + 1;
            richTextBox.ScrollToCaret();
        }
    }

}
