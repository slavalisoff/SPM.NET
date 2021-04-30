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
    public partial class Form1 : Form
    {
        #region Variables
        public static SQLiteConnection db;
        bool isOpen = false;
        Password[] passwords;
        string path;
        #endregion

        #region DB Methods
        private void AddPasswordToDB(Password password)
        {
            SQLiteCommand sqlcom = db.CreateCommand();
            string[] temp = password.GetLines();
            Console.WriteLine(passwords.Length + 1);
            sqlcom.CommandText = @"INSERT INTO Passwords (
                          ID,
                          Names,
                          Passwords,
                          Websites
                      )
                      VALUES (
                          " + (passwords.Length + 1) + @", 
                          '" + temp[0] + @"',
                          '" + temp[1] + @"',
                          '" + temp[2] + @"'
                      );";
            sqlcom.ExecuteNonQuery();
        }
        private void DeletePasswordFromDB(int id)
        {
            SQLiteCommand sqlcom = db.CreateCommand();
            sqlcom.CommandText = "DELETE FROM Passwords WHERE ID = '" + id + "'";
            sqlcom.ExecuteNonQuery();
            sqlcom.CommandText = @"UPDATE Passwords SET ID = (ID - 1) WHERE ID > '" + id + "'";
            sqlcom.ExecuteNonQuery();
        }
        private void UpdatePasswordInDB(int id)
        {
            SQLiteCommand sqlcom = db.CreateCommand();
            string[] temp = passwords[id - 1].GetLines();
            sqlcom.CommandText = @"UPDATE Passwords
                SET Names = '" + temp[0] + @"',
                    Passwords = '" + temp[1] + @"',
                    Websites = '" + temp[2] + @"'
                WHERE ID = '" + id + "'";
            sqlcom.ExecuteNonQuery ();
        }
        private void PasswordListUpdater ()
        {
            PasswordsView.Clear();
            SQLiteCommand sqlcom = db.CreateCommand();
            sqlcom.CommandText = "select Names, Passwords, Websites from Passwords";
            SQLiteDataReader sqlr = sqlcom.ExecuteReader();
            List<Password> Passwords = new List<Password>();
            while (sqlr.Read ())
            {
                Passwords.Add (new Password((string)sqlr["Names"], (string)sqlr["Passwords"], (string)sqlr["Websites"]));
            }
            passwords = Passwords.ToArray ();
            for (int i = 0; i < passwords.Length; i++)
            {
                PasswordsView.Items.Add (passwords[i].name);
            }
        }
        #endregion

        #region Listeners
        public Form1 ()
        {
            InitializeComponent ();
            PasswordsView.View = View.List;
        }
        private void EVButton_Click (object sender, EventArgs e)
        {
            try
            {
                var fi = PasswordsView.FocusedItem;
                using (ViewEditWindow vew = new ViewEditWindow(passwords[fi.Index].GetLines(), fi.Index))
                {
                    var result = vew.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Password ret = vew.ret;
                        passwords[fi.Index] = ret;
                        UpdatePasswordInDB(fi.Index + 1);
                        PasswordListUpdater();
                    }
                }
            } catch (Exception ex) 
            {
                MessageBox.Show(@"¯\_(ツ)_/¯",
                            @"¯\_(ツ)_/¯",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            }
        }
        private void OpenButton_Click (object sender, EventArgs e)
        {
            PasswordsView.Items.Clear();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "База данных |*.db|Все файлы|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (isOpen)
                        {
                            db.Close();
                        }
                        string path = ofd.FileName;
                        db = new SQLiteConnection("Data Source=" + path + "; Version=3");
                        db.Open();
                        isOpen = true;
                        PasswordListUpdater();
                        AddButton.Enabled = true;
                        DelButton.Enabled = true;
                        EVButton.Enabled = true;
                    } catch(Exception ex)
                    {
                        MessageBox.Show("Не удалось открыть базу данных",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        AddButton.Enabled = false;
                        DelButton.Enabled = false;
                        EVButton.Enabled = false;
                        DecryptButton.Enabled = true;
                    }
                }
            }
        }
        private void Form1_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (isOpen)
            {
                db.Close ();
            }
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void NewButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "База данных |*.db|Все файлы|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (isOpen)
                        {
                            db.Close();
                        }
                        path = sfd.FileName;
                        SQLiteConnection.CreateFile(path);
                        db = new SQLiteConnection("Data Source=" + path + "; Version=3");
                        db.Open();
                        isOpen = true;
                        SQLiteCommand sqlcom = db.CreateCommand();
                        sqlcom.CommandText = @"CREATE TABLE IF NOT EXISTS Passwords (
                            ID INTEGER PRIMARY KEY NOT NULL,
                            Names VARCHAR NOT NULL,
                            Passwords VARCHAR NOT NULL,
                            Websites VARCHAR
                            )";
                        sqlcom.ExecuteNonQuery();
                        PasswordListUpdater();
                        AddButton.Enabled = true;
                        DelButton.Enabled = true;
                        EVButton.Enabled = true;
                        DecryptButton.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось создать базу данных",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        MessageBox.Show(ex.ToString(),
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        Clipboard.SetText(ex.ToString());
                    }
                }
            }
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (AddWindow aw = new AddWindow())
                {
                    var result = aw.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Password ret = aw.ret;
                        AddPasswordToDB(ret);
                        PasswordListUpdater();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"¯\_(ツ)_/¯",
                            @"¯\_(ツ)_/¯",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            }
        }
        private void DelButton_Click(object sender, EventArgs e)
        {
            try
            {
                var fi = PasswordsView.FocusedItem;
                DeletePasswordFromDB(fi.Index + 1);
                PasswordListUpdater();
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"¯\_(ツ)_/¯",
                            @"¯\_(ツ)_/¯",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            }
        }
        private void AboutButton_Click(object sender, EventArgs e)
        {
            AboutWindow aw = new AboutWindow();
            aw.ShowDialog();
        }
        #endregion
    }
}