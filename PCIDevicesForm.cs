using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LabIIPU_PCI
{
    public partial class PCIDevicesList : Form
    {
        public PCIDevicesList()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {}

        private string GetProductName(List<string> transcriptOfPIDs, List<string> PIDs)
        {
            foreach (String nameOfProduct in transcriptOfPIDs)
            {
                if (nameOfProduct.Contains(PIDs[0].ToLower()))
                {
                    PIDs.RemoveAt(0);
                    return nameOfProduct;
                }
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PCIDevices.DownloadFileOfDevIDs();

            var VIDs = new List<string>();
            var PIDs = new List<string>();
            var DevicesOnPci = PCIDevices.GetDevicesID("Win32_PnPEntity", "PnPDeviceID");

            foreach (string device in DevicesOnPci)
            {
                VIDs.Add(PCIDevices.ParseVidFromDeviceID(device));
                PIDs.Add(PCIDevices.ParsePidFromDeviceID(device));
            }

            for (int i = 0; i < VIDs.Count; i++)
            {
                textBox1.Text = textBox1.Text + "VID: " + PCIDevices.ParseVIDTranscript(VIDs[i]) + Environment.NewLine;
                textBox1.Text = textBox1.Text + "PID: " + PCIDevices.ParsePIDTranscript(PIDs[i]) + Environment.NewLine + Environment.NewLine;
            }
        }
    }
}
