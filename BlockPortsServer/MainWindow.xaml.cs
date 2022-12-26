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

namespace BlockPortsServer
{
    public partial class MainWindow : Window
    {
        SimpleTcpServer server;
        Dictionary<string, string> ports;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            server.Start();
            textBlock.Text += $"Starting...{Environment.NewLine}";
            startButton.IsEnabled = false;
            sendButton.IsEnabled = true;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            sendButton.IsEnabled = false;
            server = new SimpleTcpServer(serverTB.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;

            ports = new Dictionary<string, string>();
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
                    ports[e.IpPort] = data;
                    clientsList.SelectedItem = e.IpPort;
                    setDataToCheckBoxes(ports[e.IpPort]);
                }               
            }));
        }

        private void Events_ClientDisconnected(object? sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBlock.Text += $"{e.IpPort}: Disconnected {Environment.NewLine}";
                ports.Remove(e.IpPort);
                clientsList.Items.Remove(e.IpPort);
            }));
        }

        private void Events_ClientConnected(object? sender, ConnectionEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBlock.Text += $"{e.IpPort}: Connected {Environment.NewLine}";
                clientsList.Items.Add(e.IpPort);
            }));
            
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (server.IsListening)
            {
                string key = clientsList.SelectedItem.ToString();
                if (ports.ContainsKey(key))
                {
                    ports[key] = getDataFromCheckBoxes();
                    if (!string.IsNullOrEmpty(ports[key]) && clientsList.SelectedItem != null)
                    {
                        server.Send(clientsList.SelectedItem.ToString(), $"Ports:{ports[key]}");
                    }
                }
            }
        }

        private string getDataFromCheckBoxes()
        {
            string data = "";

            data += Convert.ToInt32(usbCB.IsChecked);
            data += Convert.ToInt32(comCB.IsChecked);
            data += Convert.ToInt32(lptCB.IsChecked);
            data += Convert.ToInt32(cdCB.IsChecked);
            data += Convert.ToInt32(floppyCB.IsChecked);

            return data;
        }

        private void setDataToCheckBoxes(string data)
        {
            if (data[0] == '1')
                usbCB.IsChecked = true;
            else
                usbCB.IsChecked = false;

            if (data[1] == '1')
                comCB.IsChecked = true;
            else
                comCB.IsChecked = false;

            if (data[2] == '1')
                lptCB.IsChecked = true;
            else
                lptCB.IsChecked = false;

            if (data[3] == '1')
                cdCB.IsChecked = true;
            else
                cdCB.IsChecked = false;

            if (data[4] == '1')
                floppyCB.IsChecked = true;
            else
                floppyCB.IsChecked = false;
        }

        private void clientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clientsList.SelectedItem != null && ports.Count > 0)
                setDataToCheckBoxes(ports[clientsList.SelectedItem.ToString()]);
        }
    }
}
