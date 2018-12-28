using System;
using System.ComponentModel;
using System.Windows.Forms;
using Utilities;

namespace Chat_Client_GUI
{
    public partial class LoginForm : Form
    {

        private User adminUser;
        private bool isForceClose = true; 
        public string username;
        public string friendName;


        public LoginForm(User adminUser)
        {
            this.adminUser = adminUser;
            InitializeComponent();
            this.usernameTextBox.KeyPress += new KeyPressEventHandler(usernameTextBox_KeyPress);
            this.friendTextBox.KeyPress += new KeyPressEventHandler(friendTextBox_KeyPress);
            this.FormClosing += new FormClosingEventHandler(LoginFormClosing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void LoginFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            /* Please don't ask why Environment.Exit(0) is used in place of Application.Exit(),
               for some reason Application.Exit() doesn't actually exit, but calls this method again?? */
            else if (isForceClose)
            {
                adminUser.Close();
                Environment.Exit(0);
            }
        }

        private void usernameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void usernameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                submitButton.PerformClick();
                e.Handled = true;
            }
        }

        private void friendTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                submitButton.PerformClick();
                e.Handled = true;
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            username = usernameTextBox.Text;
            friendName = friendTextBox.Text;

            if (username.Equals(String.Empty))
            {
                MessageBox.Show("Error: Empty Username", "Please enter your username!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                 
            }

            if (friendName.Equals(String.Empty))
            {
                MessageBox.Show("Error: Empty Friend Name", "Please enter a friend's name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SendMessageAsync handler = new SendMessageAsync(adminUser.Client.GetStream());
            handler.SendMessage($"VERIFYUSER:{username}:{friendName}");

            string authMessage = MessageHandler.GetMessage(adminUser.Client.GetStream());
            switch (authMessage)
            {
                case "NOACCOUNT":
                    MessageBox.Show("No user with that name", "Please create an account.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case "WRONGPASSWORD":
                    MessageBox.Show("Incorrect password", "Please enter the correct password.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    this.isForceClose = false;
                    this.Close();
                    break;
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            CreateAccountForm createAccountForm = new CreateAccountForm(adminUser);
            createAccountForm.ShowDialog();
        }

    }
}
