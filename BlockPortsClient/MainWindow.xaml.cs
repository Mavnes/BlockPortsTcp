using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace BlockPortsClient
{
    public partial class MainWindow : Window
    {
        SimpleTcpClient client;
        PortHandler portHandler;

        public MainWindow()
        {
            portHandler = new PortHandler();
            InitializeComponent();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect();
                client.Send($"Ports:{portHandler.getPorts()}");
                connectButton.IsEnabled = false;
                disconnectButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Disconnect();
                connectButton.IsEnabled = true;
                disconnectButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            client = new(serverTB.Text);
            client.Events.Connected += Events_Connected;
            client.Events.Disconnected += Events_Disconnected;
            client.Events.DataReceived += Events_DataReceived;
            disconnectButton.IsEnabled = false;
            textBlock.Text += $"{portHandler.PortsToString()}{Environment.NewLine}";
        }

        private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                string input = Encoding.UTF8.GetString(e.Data);
                string comand = input.Split(":")[0];
                string data = input.Split(":")[1];

                if (comand == "Ports")
                {
                    portHandler.setPorts(data);
                    textBlock.Text += $"{portHandler.PortsToString()}{Environment.NewLine}";
                }
            }));
        }

        private void Events_Disconnected(object? sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBlock.Text += $"Server disconnected {Environment.NewLine}";
                connectButton.IsEnabled = true;
            }));
        }

        private void Events_Connected(object? sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBlock.Text += $"Server connected {Environment.NewLine}";
            }));
        }
    }
}
