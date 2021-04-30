using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Password_Manager
{
    public partial class AddWindow : Form
    {
        public Password ret;
        public AddWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            ret = new Password(NameBox.Text, PassBox.Text, SiteBox.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void GenButton_Click(object sender, EventArgs e)
        {
            string alphabet = "";
            string password = "";
            PassBox.Text = "";
            if (NumsToggle.Checked)
            {
                alphabet += "1234567890";
            }
            if (LetsToggle.Checked)
            {
                alphabet += "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            }
            if (SpecsToggle.Checked)
            {
                alphabet += "&?!@<>%$:*";
            }
            if (alphabet == "")
            {
                const string message = "Вы не выбрали из каких символов генерировать пароль";
                const string caption = "Предупрждение";
                MessageBox.Show(message, caption,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            Random rnd = new Random();
            int length = rnd.Next(32);
            int lng = alphabet.Length;
            for (int i = 0; i < length; i++)
                password += alphabet[rnd.Next(lng)];
            PassBox.Text = password;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
