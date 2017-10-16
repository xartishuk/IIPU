using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabIIPU_PCI
{
    class PCIDevices
    {
        private static string[] GetSubstringOfDeviceID(string deviceID)
        {
            string[] substringOfDeviceID = deviceID.Split('\\');
            return substringOfDeviceID[1].Split('&');
        }

        public static string ParseVidFromDeviceID(string deviceId)
        {
            string[] substringOfDeviceID = GetSubstringOfDeviceID(deviceId);
            string vendorID = substringOfDeviceID[0].Replace("VID", "").Replace("VEN", "").Replace("_", "");
            return vendorID;
        }

        public static string ParsePidFromDeviceID(string deviceId)
        {
            string[] substrinOfDeviceID = GetSubstringOfDeviceID(deviceId);
            string productID = substrinOfDeviceID[1].Replace("PID", "").Replace("DEV", "").Replace("_", "");
            return productID;
        }

        public static List<string> GetDevicesID(string WIN32Class, string WIN32ClassItem)
        {
            List<string> devicesID = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32Class);
            try
            {
                foreach (ManagementObject PnPEntityObject in searcher.Get())
                {
                    if (PnPEntityObject[WIN32ClassItem].ToString().Trim().Contains("PCI\\"))
                        devicesID.Add(PnPEntityObject[WIN32ClassItem].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return devicesID;
        }

        public static List<string> ParseVIDsTranscript()
        {
            List<string> listOfVIDs = new List<string>();
            String pattern = @"^(\w{4}  (.*?)$)";
            foreach (Match match in Regex.Matches(GetFileContent(), pattern, RegexOptions.Multiline))
                listOfVIDs.Add(match.Value);
            return listOfVIDs;
        }

        public static List<string> ParsePIDsTranscript()
        {
            List<string> listOfPIDs = new List<string>();
            String pattern = @"^(\t\w{4})  (.*?)$";
            foreach (Match match in Regex.Matches(GetFileContent(), pattern, RegexOptions.Multiline))
                listOfPIDs.Add(match.Value.Replace("\t", ""));
            return listOfPIDs;
        }

        public static void DownloadFileOfDevIDs()
        {
            String fileURI = "http://pci-ids.ucw.cz/v2.2/";
            String nameOfFile = "pci.ids";
            String webResource = fileURI + nameOfFile;
            WebClient webClient = new WebClient();
            webClient.DownloadFile(webResource, nameOfFile);
        }

        private static String GetFileContent()
        {
            String path = Directory.GetCurrentDirectory() + "/pci.ids";
            String text = File.ReadAllText(path);
            return text;
        }
    }
}

