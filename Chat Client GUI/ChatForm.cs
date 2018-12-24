using System;
using System.Windows.Forms;

namespace Chat_Client_GUI
{
    public partial class ChatForm : Form
    {

        private string username;
        private string friendName;
        private User user;

        public ChatForm(string username, string friendName, User user)
        {
            this.username = username;
            this.friendName = friendName;
            this.user = user;
            InitializeComponent();

            this.Text = "Chatting to: " + friendName;

            this.replyTextBox.KeyPress += new KeyPressEventHandler(replyTextBox_KeyPress);
        }

        private void chatTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void replyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                sendButton.PerformClick();
                e.Handled = true;
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string message = replyTextBox.Text;

            this.user.SendMessage(message);

            chatTextBox.AppendText("Me: " + message + "\n");

            replyTextBox.Text = String.Empty;
        }

        public void AppendText(String text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendText), new object[] { text });
                return;
            }
            this.chatTextBox.Text += text;
        }

    }
}
