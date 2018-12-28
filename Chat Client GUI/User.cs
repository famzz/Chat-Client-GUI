using System;
using System.Net.Sockets;
using Utilities;

namespace Chat_Client_GUI
{
    public class User
    {

        private string username;
        private string friendName;
        public TcpClient Client { get; }

        public User(string username, TcpClient client)
        {
            this.username = username;
            this.Client = client;
        }

        public User(string username, string friendName, TcpClient client)
        {
            this.username = username;
            this.friendName = friendName;
            this.Client = client;
        }

        public void SendMessage(string message)
        {
            MessageHandler.SendMessage(this.Client.GetStream(), message);
        }

        public void GetMessage(ChatForm chatForm)
        {
            NetworkStream stream = Client.GetStream();

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
            this.Client.GetStream().Close();
            this.Client.Close();
        }
    }
}
