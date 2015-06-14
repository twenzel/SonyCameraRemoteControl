using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rssdp;
using SonyCameraRemoteControl;

namespace Test
{
    public partial class Form1 : Form
    {
        SonyCameraDevice _device;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtDeviceInfo.Text = "searching ...";
            _device = RemoteControl.FindDevice(txtuuid.Text);            
            
             printDeviceInfos();       
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtDeviceInfo.Text = "searching ...";
            _device = RemoteControl.SearchDevice(10000);

            printDeviceInfos();
        }

        private void printDeviceInfos()
        {
            if (_device != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Friendly name: " + _device.FriendlyName);
                sb.AppendLine("ModelDescription: " + _device.ModelDescription);
                sb.AppendLine("ModelName: " + _device.ModelName);
                sb.AppendLine("ModelNumber: " + _device.ModelNumber);
                sb.AppendLine("Uuid: " + _device.Uuid);

                sb.AppendLine("\r\nEndpoints: ");
                foreach (var pair in _device.Endpoints)
                    sb.AppendLine(String.Format("{0}: {1}", pair.Key, pair.Value));

                txtDeviceInfo.Text = sb.ToString();
            } else
                txtDeviceInfo.Text = "No device found";
        }

        SsdpDeviceLocator _BroadcastListener;
        private void button3_Click(object sender, EventArgs e)
        {
            if (_BroadcastListener != null)
            {               
                _BroadcastListener.DeviceAvailable -= _BroadcastListener_DeviceAvailable;

                _BroadcastListener.StopListeningForNotifications();
                _BroadcastListener.Dispose();
                _BroadcastListener = null;

                txtDeviceInfo.Text += "\r\nstopped";
            }
            else
            {
                txtDeviceInfo.Text = "listen...";
                _BroadcastListener = new SsdpDeviceLocator();
                _BroadcastListener.DeviceAvailable += _BroadcastListener_DeviceAvailable;
                _BroadcastListener.StartListeningForNotifications();
            }
        }

        void _BroadcastListener_DeviceAvailable(object sender, DeviceAvailableEventArgs e)
        {
            if (e.IsNewlyDiscovered)
            {
                this.Invoke((MethodInvoker) delegate { txtDeviceInfo.Text += e.DiscoveredDevice.DescriptionLocation + "\r\n";});
                var t = SonyCameraDevice.GetSonyDevice(e.DiscoveredDevice);
                var r = t.Result;
            }
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            var result = await _device.StartRecMode();

            showError(result);
        }

        private void showError(ResultBase result)
        {
            if (result.HasError)
                MessageBox.Show(result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var result = await _device.StartLiveView();

            showError(result);

            if (!result.HasError)
                axWindowsMediaPlayer1.URL = result.Value;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = "";
            var result = await _device.SetShootMode("still");

            showError(result);
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            var result = await _device.TakePicture();

            showError(result);

            if (!result.HasError && result.Value.Length > 0)
                pictureBox1.LoadAsync(result.Value[0]);
        }
    }
}
