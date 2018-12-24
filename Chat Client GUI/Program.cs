using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Client_GUI
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginForm = new LoginForm();
            Application.Run(loginForm);

            string username = loginForm.username;
            string friendName = loginForm.friendName;

            //IPAddress address = IPAddress.Parse("86.12.83.251");
            Int32 port = 5667;

            TcpClient client = new TcpClient("86.12.83.251", port);
            NetworkStream stream = client.GetStream();

            User user = new User(username, friendName, client);
            user.SendMessage("CONNECTIONSTART:" + username + ":" + friendName);

            ChatForm chatForm = new ChatForm(username, friendName, user);

            Thread t = new Thread(new ThreadStart(() => user.GetMessage(chatForm)));
            t.Start();

            Application.Run(chatForm);

            user.SendMessage("exit");
            t.Abort();
            stream.Close();
            client.Close();
        }
    }
}
