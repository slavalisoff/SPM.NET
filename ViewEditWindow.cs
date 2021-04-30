using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Password_Manager
{
    public partial class ViewEditWindow : Form
    {
        private string name, password, website;
        int id;
        public Password ret;

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ret = new Password(NameBox.Text, PassBox.Text, SiteBox.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ViewEditWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        public ViewEditWindow(string[] args, int b)
        {
            InitializeComponent();
            name = args[0];
            password = args[1];
            website = args[2];
            NameBox.Text = name;
            PassBox.Text = password;
            SiteBox.Text = website;
            id = b;
        }
        public ViewEditWindow(string[] args)
        {
            InitializeComponent();
            name = args[0];
            password = args[1];
            website = args[2];
            NameBox.Text = name;
            PassBox.Text = password;
            SiteBox.Text = website;
            SaveButton.Enabled = false;
        }
    }
}
