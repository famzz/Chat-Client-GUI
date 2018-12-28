using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Chat_Client_GUI
{
    class SendMessageAsync
    {

        private BackgroundWorker worker = new BackgroundWorker();
        private NetworkStream stream; 

        public SendMessageAsync(NetworkStream stream)
        {
            worker.DoWork += this.SendMessage;
            this.stream = stream;
        }

        public void SendMessage(string message)
        {
            worker.RunWorkerAsync(message);
        }

        private void SendMessage(object sender, DoWorkEventArgs e)
        {
            string message = (string)e.Argument;
            MessageHandler.SendMessage(this.stream, message);
        }

    }
}
