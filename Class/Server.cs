using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dispetcher2.Class
{
    public enum ServerType
    {
        Production = 0,
        Testing = 1,
    }
    public class Server
    {
        public string Name { get; set; }
        public ServerType Type { get; set; }
    }
    public class ServerViewModel
    {
        public BindingSource ServerList { get; set; }
        public Action ServerChangeHandler { get; set; }
        Server ss;
        public Server SelectedServer
        {
            get { return ss; }
            set
            {
                ss = value;
                if (ServerChangeHandler != null) ServerChangeHandler();
            }
        }
        public ServerViewModel()
        {
            ServerList = new BindingSource();

            Server v = new Server()
            {
                Name = "Рабочий сервер",
                Type = ServerType.Production,
            };
            ServerList.Add(v);

            v = new Server()
            {
                Name = "Тестовый сервер",
                Type = ServerType.Testing,
            };
            ServerList.Add(v);
        }
    }
}
