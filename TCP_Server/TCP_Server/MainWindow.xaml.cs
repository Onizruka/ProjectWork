using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace TCP_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer DispatcherTimer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TCPServer.Start_TCPServer();
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dataGrid.ItemsSource = TCPServer.clientObjects;
            dataGrid.Items.Refresh();
            dataGrid1.ItemsSource = TCPServer.clientAnswerObjects;
            dataGrid1.Items.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DispatcherTimer.Stop();
            TCPServer.StopThread();
        }
    }
}
