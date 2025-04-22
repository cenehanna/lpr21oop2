using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lpr21oop2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public string FindText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public RichTextBoxFinds FindCondition
        {
            get
            {
                RichTextBoxFinds options = RichTextBoxFinds.None;

                if (checkBox1.Checked)
                    options |= RichTextBoxFinds.MatchCase;
                if (checkBox2.Checked)
                    options |= RichTextBoxFinds.WholeWord;

                return options;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FindText))
            {
                MessageBox.Show("Будь ласка, введіть текст для пошуку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form1 main = this.Owner as Form1;
            Form2 active = main?.ActiveMdiChild as Form2;

            if (active != null)
            {
                active.FindAll(FindText, FindCondition);
            }
            else
            {
                MessageBox.Show("Немає активного документа для пошуку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            Form2 active = main?.ActiveMdiChild as Form2;

            if (active != null)
            {
                if (string.IsNullOrWhiteSpace(FindText))
                {
                    MessageBox.Show("Будь ласка, введіть текст для пошуку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                active.FindPrev(FindText);
            }
        }

        private void Next_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            Form2 active = main?.ActiveMdiChild as Form2;

            if (active != null)
            {
                if (string.IsNullOrWhiteSpace(FindText))
                {
                    MessageBox.Show("Будь ласка, введіть текст для пошуку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                active.FindNext(FindText);
            }
        }
    }
}