using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Client_GUI
{
    public partial class CreateAccountForm : Form
    {

        private User adminUser;

        public CreateAccountForm(User adminUser)
        {
            InitializeComponent();
            this.adminUser = adminUser;
        }

        private void CreateAccountForm_Load(object sender, EventArgs e)
        {

        }


        private void createButton_Click(object sender, EventArgs e)
        {
            if (passwordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Passwords do not match", "Passwords do not match", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (usernameTextBox.Text.ToLower().Equals("admin"))
            {
                MessageBox.Show("Reserved username", "Unfortunately that username is taken.", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                adminUser.SendMessage($"NEWUSER:{usernameTextBox.Text}:{passwordTextBox.Text}");
            }
        }
    }
}
