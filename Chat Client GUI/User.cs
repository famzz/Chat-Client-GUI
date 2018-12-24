using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;

namespace Chat_Client_GUI
{
    public class User
    {

        private string username;
        private string friendName;
        private TcpClient client;

        public User(string username, string friendName, TcpClient client)
        {
            this.username = username;
            this.friendName = friendName;
            this.client = client;
        }

        public void SendMessage(string message)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true; //Either this, or you Flush manually every time you send something.

            writer.WriteLine(message); //Every message you want to send
        }

        public void GetMessage(ChatForm chatForm)
        {
            NetworkStream stream = client.GetStream();
            byte[] data = new byte[256];
            string message = null;
            int i;

            while ((i = stream.Read(data, 0, data.Length)) != 0)
            {
                message = Encoding.ASCII.GetString(data, 0, i);

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
    }
}
