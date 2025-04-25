using System;
using System.Windows.Forms;

namespace lpr21oop2
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            LanguageManager.LanguageChanged += (s, e) => UpdateFormLanguage();

            UpdateFormLanguage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void UpdateFormLanguage()
        {
            LanguageManager.UpdateFormTitle(this);
            LanguageManager.UpdateControlsLanguage(this);

            foreach (Control control in this.Controls)
            {
                if (control is ToolStrip toolStrip)
                {
                    LanguageManager.UpdateToolStripItems(toolStrip.Items);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/cenehanna"; 
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
                ((LinkLabel)sender).LinkVisited = true; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося відкрити посилання: {ex.Message}");
            }
        }
    }
}
