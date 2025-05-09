﻿using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace lpr21oop2
{
    public partial class Form1 : Form
    {

        public Form1()
        {

            InitializeComponent();

            новийToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.N;
            відкритиToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            зберегтиToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            зберегтиЯкToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            вихідToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;

            вирізатиToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            копіюватиToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            вставитиToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            видалитиToolStripMenuItem.ShortcutKeys = Keys.Delete;
            виділитиВсеToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            знайтиToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;

            шрифтToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.T;
            колірТекстуToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
        }

        public static int documentCounter = 0;



        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Form2 newChild = new Form2();
                newChild.MdiParent = this;

                newChild.Text = ofd.FileName;

                newChild.Show();

                if (Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                    newChild.richTextBox1.LoadFile(ofd.FileName);
                else
                    newChild.richTextBox1.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                if (child.Tag != null && !string.IsNullOrEmpty(child.Tag.ToString()))
                {
                    if (Path.GetExtension(child.Tag.ToString()).ToLower() == ".rtf")
                        child.richTextBox1.SaveFile(child.Tag.ToString());
                    else
                        File.WriteAllText(child.Tag.ToString(), child.richTextBox1.Text);
                }
                else
                {
                    SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt"
                    };
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                            child.richTextBox1.SaveFile(sfd.FileName);
                        else
                            File.WriteAllText(sfd.FileName, child.richTextBox1.Text);

                        child.Tag = sfd.FileName;
                        child.Text = sfd.FileName;
                    }
                }
            }
        }

        private void зберегтиЯкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                        child.richTextBox1.SaveFile(sfd.FileName);
                    else
                        File.WriteAllText(sfd.FileName, child.richTextBox1.Text);

                    child.Tag = sfd.FileName;
                    child.Text = sfd.FileName;
                }
            }
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void вирізатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
                child.richTextBox1.Cut();
        }

        private void копіюватиєєToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
                child.richTextBox1.Copy();
        }

        private void вставитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
                child.richTextBox1.Paste();
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
                child.richTextBox1.SelectedText = "";
        }

        private void виділитиВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
                child.richTextBox1.SelectAll();
        }

        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void плиткоюГоризонтальноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void плиткоюВертикальноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                using (FontDialog fontDlg = new FontDialog())
                {
                    if (fontDlg.ShowDialog() == DialogResult.OK)
                    {
                        child.richTextBox1.SelectionFont = fontDlg.Font;
                    }
                }
            }
        }

        private void колірТекстуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                using (ColorDialog colorDlg = new ColorDialog())
                {
                    if (colorDlg.ShowDialog() == DialogResult.OK)
                    {
                        child.richTextBox1.SelectionColor = colorDlg.Color;
                    }
                }
            }
        }

        private void знайтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.ShowDialog(this);

        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4();
            frm.Show();
        }


        private void tsNew_Click(object sender, EventArgs e)
        {
            Form2 newChild = new Form2();
            newChild.MdiParent = this;
            documentCounter++;
            newChild.Text = "Документ " + documentCounter;

            newChild.Show();
        }

        private void tsOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Form2 newChild = new Form2();
                newChild.MdiParent = this;
                newChild.Text = ofd.FileName;
                newChild.Show();

                if (Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                    newChild.richTextBox1.LoadFile(ofd.FileName);
                else
                    newChild.richTextBox1.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                if (child.Tag != null && !string.IsNullOrEmpty(child.Tag.ToString()))
                {
                    if (Path.GetExtension(child.Tag.ToString()).ToLower() == ".rtf")
                        child.richTextBox1.SaveFile(child.Tag.ToString());
                    else
                        File.WriteAllText(child.Tag.ToString(), child.richTextBox1.Text);
                }
                else
                {
                    SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt"
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                            child.richTextBox1.SaveFile(sfd.FileName);
                        else
                            File.WriteAllText(sfd.FileName, child.richTextBox1.Text);

                        child.Tag = sfd.FileName;
                        child.Text = sfd.FileName;
                    }
                }
            }
        }

        private void tsCut_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                child.richTextBox1.Cut();
            }
        }

        private void tsCopy_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                child.richTextBox1.Copy();
            }
        }

        private void tsPaste_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                child.richTextBox1.Paste();
            }
        }
        private void SetAlignment(HorizontalAlignment alignment)
        {
            if (ActiveMdiChild is Form2 child)
            {
                child.richTextBox1.SelectionAlignment = alignment;
                UpdateAlignmentUI();
            }
        }

        private void UpdateAlignmentUI()
        {
            if (ActiveMdiChild is Form2 child)
            {
                var alignment = child.richTextBox1.SelectionAlignment;
                ліворучToolStripMenuItem.Checked = alignment == HorizontalAlignment.Left;
                посерединіToolStripMenuItem.Checked = alignment == HorizontalAlignment.Center;
                праворучToolStripMenuItem.Checked = alignment == HorizontalAlignment.Right;
            }
        }
        private void ліворучToolStripMenuItem_Click(object sender, EventArgs e) => SetAlignment(HorizontalAlignment.Left);
        private void посерединіToolStripMenuItem_Click(object sender, EventArgs e) => SetAlignment(HorizontalAlignment.Center);
        private void праворучToolStripMenuItem_Click(object sender, EventArgs e) => SetAlignment(HorizontalAlignment.Right);

        private void поШириніToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void зображенняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Зображення (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
                    openFileDialog.Title = "Виберіть зображення для вставки";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            Image image = Image.FromFile(openFileDialog.FileName);

                            child.richTextBox1.InsertImage(image);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Помилка при вставці зображення: {ex.Message}", "Помилка",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void tsUndo_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                child.Undo();
            }
        }

        private void tsRedo_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Form2 child)
            {
                child.Redo();
            }
        }

        private void Left_Click(object sender, EventArgs e) => SetAlignment(HorizontalAlignment.Left);

        private void Center_Click(object sender, EventArgs e) => SetAlignment(HorizontalAlignment.Center);

        private void Right_Click(object sender, EventArgs e) => SetAlignment(HorizontalAlignment.Right);


        private void новийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 newChild = new Form2();
            newChild.MdiParent = this;
            documentCounter++;

            string localizedDocumentName = LanguageManager.GetString("DocumentTitle");
            if (string.IsNullOrEmpty(localizedDocumentName))
            {
                localizedDocumentName = "Документ";
            }

            newChild.Text = $"{localizedDocumentName} {documentCounter}";
            newChild.Show();
        }

        private void українськаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageManager.CurrentCulture = new CultureInfo("uk-UA");
            UpdateMenuLanguageSelection();
        }

        private void англійськаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageManager.CurrentCulture = new CultureInfo("en-US");
            UpdateMenuLanguageSelection();
        }

        private void UpdateMenuLanguageSelection()
        {
            українськаToolStripMenuItem.Checked = false;
            англійськаToolStripMenuItem.Checked = false;

            switch (LanguageManager.CurrentCulture.Name)
            {
                case "uk-UA":
                    українськаToolStripMenuItem.Checked = true;
                    break;
                case "en-US":
                    англійськаToolStripMenuItem.Checked = true;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LanguageManager.LanguageChanged += (s, args) =>
{
    LanguageManager.UpdateFormLanguage(this);
};

            LanguageManager.UpdateFormLanguage(this);
            UpdateMenuLanguageSelection();
        }

        private void виділенняКодуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form2 activeForm = this.ActiveMdiChild as Form2;

            if (activeForm != null)
            {
                
                activeForm.IsSyntaxHighlightingEnabled = !activeForm.IsSyntaxHighlightingEnabled;

                
                виділенняКодуToolStripMenuItem.Checked = activeForm.IsSyntaxHighlightingEnabled;

               
                if (activeForm.IsSyntaxHighlightingEnabled)
                {
                    activeForm.HighlightSyntax(activeForm.richTextBox1);
                }
                else
                {
                    
                    activeForm.richTextBox1.SelectAll();
                    activeForm.richTextBox1.SelectionColor = activeForm.richTextBox1.ForeColor;
                    activeForm.richTextBox1.SelectionBackColor = activeForm.richTextBox1.BackColor;
                    activeForm.richTextBox1.DeselectAll();
                }
            }
            else
            {
                MessageBox.Show("Активне вікно не є текстовим редактором.", "Помилка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
