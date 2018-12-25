using System;
using System.Net.Sockets;
using System.Threading;
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

            Int32 port = 5667;

            TcpClient adminClient = new TcpClient("86.12.83.251", port);

            User adminUser = new User("admin", adminClient);

            LoginForm loginForm = new LoginForm(adminUser);
            Application.Run(loginForm);

            string username = loginForm.username;
            string friendName = loginForm.friendName;

            TcpClient client = new TcpClient("86.12.83.251", port);
            NetworkStream stream = client.GetStream();

            User user = new User(username, friendName, client);
            user.SendMessage("CONNECTIONBEGIN");
            user.SendMessage("CONNECTIONSTART:" + username + ":" + friendName);

            ChatForm chatForm = new ChatForm(username, friendName, user);

            Thread t = new Thread(new ThreadStart(() => user.GetMessage(chatForm)));
            t.Start();

            Application.Run(chatForm);

            t.Abort();
            user.Close();
            adminUser.Close();
        }
    }
}
