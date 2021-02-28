using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCP_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 1470);
            Thread.Sleep(5000);
            byte[] vs = Encoding.UTF8.GetBytes("313233343536");
            byte[] vs1 = new byte[512];
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.Read(vs1, 0, vs1.Length);
            string s = Encoding.UTF8.GetString(vs1);
            Console.WriteLine(s);
            networkStream.Write(vs, 0, vs.Length);
            networkStream.Read(vs1, 0, vs1.Length);
            s = Encoding.UTF8.GetString(vs1);
            Console.WriteLine(s);
            Thread.Sleep(5000);
            networkStream.Write(vs, 0, vs.Length);
            Thread.Sleep(5000);
            Console.WriteLine(tcpClient.Connected);
            //networkStream.Close();
            Console.WriteLine(tcpClient.Connected);

        }
    }
}
