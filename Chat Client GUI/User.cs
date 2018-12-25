using System;
using System.Net.Sockets;
using Utilities;

namespace Chat_Client_GUI
{
    public class User
    {

        private string username;
        private string friendName;
        private TcpClient client;

        public User(string username, TcpClient client)
        {
            this.username = username;
            this.client = client;
        }

        public User(string username, string friendName, TcpClient client)
        {
            this.username = username;
            this.friendName = friendName;
            this.client = client;
        }

        public void SendMessage(string message)
        {
            MessageHandler.SendMessage(this.client.GetStream(), message);
        }

        public void GetMessage(ChatForm chatForm)
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                string message = MessageHandler.GetMessage(stream);

                /* Split into seperate messages if the user had pending messages since sometimes the
                   stream is written to too quickly for multiple WriteLine()s to handle. */
                if (message.Contains("ENDOFLINE"))
                {
                    foreach (string line in message.Split(new[] { "ENDOFLINE" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        chatForm.AppendText(this.friendName + ": " + line + "\n");
                    }
                }
                else
                {
                    chatForm.AppendText(this.friendName + ": " + message + "\n");
                }

            }

        }

        public void Close()
        {
            SendMessage("exit");
            this.client.GetStream().Close();
            this.client.Close();
        }
    }
}
