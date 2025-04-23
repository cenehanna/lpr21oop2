using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace lpr21oop2
{
    public static class LanguageManager
    {
        public static event EventHandler LanguageChanged;

        private static CultureInfo _currentCulture = CultureInfo.CurrentCulture;

        public static CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != value)
                {
                    _currentCulture = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                    OnLanguageChanged();
                }
            }
        }

        public static string GetString(string key)
        {
            return Resources.ResourceManager.GetString(key, _currentCulture);
        }

        private static void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void UpdateFormLanguage(Form form)
        {
            if (form == null) return;

            UpdateFormTitle(form);
            UpdateControlsLanguage(form);
            UpdateToolStrips(form);

            if (form.IsMdiContainer)
            {
                foreach (Form childForm in form.MdiChildren)
                {
                    UpdateControlsLanguage(childForm);
                    UpdateToolStrips(childForm);
                }
            }
        }

        public static void UpdateFormTitle(Form form)
        {
            if (form == null) return;

            if (form.Tag != null && !string.IsNullOrEmpty(form.Tag.ToString()))
            {
                form.Text = GetString(form.Tag.ToString());
            }
            else
            {
                string formTitleKey = $"{form.Name}Title";
                string localizedTitle = GetString(formTitleKey);
                if (!string.IsNullOrEmpty(localizedTitle))
                {
                    form.Text = localizedTitle;
                }
            }
        }

        public static void UpdateControlsLanguage(Control parentControl)
        {
            if (parentControl == null) return;

            foreach (Control ctrl in parentControl.Controls)
            {
                if (ctrl is MdiClient) continue;

                if (ctrl.Tag != null && !string.IsNullOrEmpty(ctrl.Tag.ToString()))
                {
                    ctrl.Text = GetString(ctrl.Tag.ToString());
                }

                if (ctrl.HasChildren)
                {
                    UpdateControlsLanguage(ctrl);
                }

                if (ctrl is MenuStrip menuStrip)
                {
                    UpdateMenuItemsLanguage(menuStrip.Items);
                }
            }
        }

        public static void UpdateMenuItemsLanguage(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Tag != null && !string.IsNullOrEmpty(item.Tag.ToString()))
                {
                    item.Text = GetString(item.Tag.ToString());
                }

                if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                {
                    UpdateMenuItemsLanguage(menuItem.DropDownItems);
                }
            }
        }

        public static void UpdateToolStrips(Control parentControl)
        {
            if (parentControl == null) return;

            foreach (Control ctrl in parentControl.Controls)
            {
                if (ctrl is StatusStrip statusStrip)
                {
                    UpdateStatusStripItems(statusStrip.Items);
                }
                else if (ctrl is ToolStrip toolStrip)
                {
                    UpdateToolStripItems(toolStrip.Items);
                }

                if (ctrl.HasChildren)
                {
                    UpdateToolStrips(ctrl);
                }
            }
        }

        public static void UpdateStatusStripItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripStatusLabel statusLabel &&
                    statusLabel.Tag != null &&
                    !string.IsNullOrEmpty(statusLabel.Tag.ToString()))
                {
                    statusLabel.Text = GetString(statusLabel.Tag.ToString());
                }

                if (item is ToolStripDropDownItem dropDownItem &&
                    dropDownItem.HasDropDownItems)
                {
                    UpdateStatusStripItems(dropDownItem.DropDownItems);
                }
            }
        }

        public static void UpdateToolStripItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Tag != null && !string.IsNullOrEmpty(item.Tag.ToString()))
                {
                    item.Text = GetString(item.Tag.ToString());
                    item.ToolTipText = GetString(item.Tag.ToString() + "_ToolTip");
                }

                if (item is ToolStripDropDownItem dropDownItem &&
                    dropDownItem.HasDropDownItems)
                {
                    UpdateToolStripItems(dropDownItem.DropDownItems);
                }
            }
        }

        public static void UpdateLanguageMenuSelection(ToolStripMenuItem languageMenu, string cultureName)
        {
            foreach (ToolStripItem item in languageMenu.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Checked = (menuItem.Tag?.ToString() == cultureName);
                }
            }
        }
    }
}