using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.MVVM.Model
{
    public class InterfaceITEthernet : ObservableObject
    {
        private NetworkStream stream;
        private Byte[] data;
        private TcpClient Client;

        public InterfaceITEthernetInfo InterfaceITEthernetInfo { get; set; }

        public bool canread;

        public delegate void INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC(int nSwitch, string nDirection);

        private string hostname = "127.0.0.1";
        public string Hostname { get { return hostname; } set { hostname = value; } }

        private const int port = 10346;

        public byte ClientStatus { get; private set; }

        public void InterfaceITEthernetConnection()
        {
            System.Net.NetworkInformation.Ping ping = new();
            try
            {
                ping.Send(Hostname);
            }
            catch (Exception)
            {

            }
            finally
            {
                ping?.Dispose();
                this.ClientStatus = 1;
            }

            try
            {
                TcpClient client = new(hostname, port);
                Client = client;

                stream = client.GetStream();

                data = new byte[1024];
            }

            catch (ArgumentNullException)
            {
                
            }

            catch (SocketException)
            {
                
            }

            if (stream is not null)
            {
                canread = stream.CanRead;
                this.ClientStatus = 2;
            }
        }



        private string rc;
        private List<string> list;
        public void GetinterfaceITEthernetDataStart()
        {
            Thread.Sleep(100);
            while (stream.DataAvailable)
            {
                rc += Encoding.ASCII.GetString(data, 0, stream.Read(data, 0, data.Length));
                Thread.Sleep(100);
            }
        }

        private const string FDS_737_PRO_MX_MCP_E = "ID=0E04";
        private const string FDS_737_CDU_E = "ID=0E09";
        private const string FDS_A320_CDU_E = "ID=0E08";
        private const string IIT_E_HIO_128_256_6 = "ID=0E07";
        private const string IIT_E_HIO_64_128_3 = "ID=0E06";
        private const string IIT_OEM_128_128_3_E = "ID=0E0A";
        private const string IIT_OEM_128_256_6_E = "ID=0E05";
        private const string IIT_OEM_256_256X_6_E = "ID=0E0B";

        public void GetinterfaceITEthernetInfo()
        {
            if (rc != null)
            {
                list = rc.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                string receivedID = list[1];
                switch (receivedID)
                {
                    case FDS_737_PRO_MX_MCP_E:
                        GetIITEthernetInfo();
                        return;

                    case FDS_737_CDU_E:
                        GetIITEthernetInfo();
                        return;

                    case FDS_A320_CDU_E:
                        GetIITEthernetInfo();
                        return;

                    case IIT_E_HIO_128_256_6:
                        GetIITEthernetInfo();
                        return;

                    case IIT_E_HIO_64_128_3:
                        GetIITEthernetInfo();
                        return;

                    case IIT_OEM_128_128_3_E:
                        GetIITEthernetInfo();
                        return;

                    case IIT_OEM_128_256_6_E:
                        GetIITEthernetInfo();
                        return;

                    case IIT_OEM_256_256X_6_E:
                        GetIITEthernetInfo();
                        return;

                    default:
                        return;
                }
            }
        }


        private void GetIITEthernetInfo()
        {
            this.InterfaceITEthernetInfo = new InterfaceITEthernetInfo
            {
                ID = list.Find(s => s.Contains("ID="))?[3..],
                NAME = list.Find(s => s.Contains("NAME="))?[5..],
                SERIAL = list.Find(s => s.Contains("SERIAL="))?[7..],
                DESC = list.Find(s => s.Contains("DESC="))?[5..],
                COPYRIGHT = list.Find(s => s.Contains("COPYRIGHT="))?[10..],
                VERSION = list.Find(s => s.Contains("VERSION="))?[8..],
                FIRMWARE = list.Find(s => s.Contains("FIRMWARE="))?[9..],
                LOCATION = list.Find(s => s.Contains("LOCATION="))?[9..],
                USAGE = list.Find(s => s.Contains("USAGE="))?[6..],
                HOSTNAME = list.Find(s => s.Contains("HOSTNAME="))?[9..],
                CLIENT = list.Find(s => s.Contains("CLIENT="))?[7..],
                BOARD = list.Find(s => s.Contains("BOARD="))?[8..],
            };

            List<string> infolist = new();

            if (list.Exists(s => s.Contains("CONFIG=1:LED")))
            {
                infolist = list.Find(s => s.Contains("CONFIG=1:LED"))[13..].Split(':').ToList();
                this.InterfaceITEthernetInfo.LEDStart = infolist[1];
                this.InterfaceITEthernetInfo.LEDStop = infolist[3];
                this.InterfaceITEthernetInfo.LEDTotal = infolist[5];
                infolist.Clear();
            }

            if (list.Exists(s => s.Contains("CONFIG=1:7 SEGMENT")))
            {
                infolist = list.Find(s => s.Contains("CONFIG=1:7 SEGMENT"))[19..].Split(':').ToList();
                this.InterfaceITEthernetInfo.SEVENSEGMENTStart = infolist[1];
                this.InterfaceITEthernetInfo.SEVENSEGMENTStop = infolist[3];
                this.InterfaceITEthernetInfo.SEVENSEGMENTTotal = infolist[5];
                infolist.Clear();
            }

            if (list.Exists(s => s.Contains("CONFIG=1:SWITCH")))
            {
                infolist = list.Find(s => s.Contains("CONFIG=1:SWITCH"))[16..].Split(':').ToList();
                this.InterfaceITEthernetInfo.SWITCHStart = infolist[1];
                this.InterfaceITEthernetInfo.SWITCHStop = infolist[3];
                this.InterfaceITEthernetInfo.SWITCHTotal = infolist[5];
                infolist.Clear();
            }

            if (list.Exists(s => s.Contains("CONFIG=1:DATALINE")))
            {
                infolist = list.Find(s => s.Contains("CONFIG=1:DATALINE"))[18..].Split(':').ToList();
                this.InterfaceITEthernetInfo.DATALINEStart = infolist[1];
                this.InterfaceITEthernetInfo.DATALINEStop = infolist[3];
                this.InterfaceITEthernetInfo.DATALINETotal = infolist[5];
                infolist.Clear();
            }

            if (list.Exists(s => s.Contains("CONFIG=1:ENCODER")))
            {
                infolist = list.Find(s => s.Contains("CONFIG=1:ENCODER"))[17..].Split(':').ToList();
                this.InterfaceITEthernetInfo.ENCODERStart = infolist[1];
                this.InterfaceITEthernetInfo.ENCODERStop = infolist[3];
                this.InterfaceITEthernetInfo.ENCODERTotal = infolist[5];
                infolist = null;
            }

            list.RemoveAll(s => !s.Contains("B1="));
            list.ForEach(s => intefaceITEthernetData.Enqueue(s));
            list = null;

            GetInterfaceITEthernetInfos();
        }

        private Queue<string> intefaceITEthernetData = new();
        
        public void GetinterfaceITEthernetData(INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC pROC, CancellationToken token)
        {
            while (true)
            {
            rc = null;
                while (intefaceITEthernetData.Count > 0)
                {
                    List<string> nData = intefaceITEthernetData.Dequeue().Split(':').ToList();
                    if (nData.Count == 3)
                    {
                        int.TryParse(nData[1], out int LED);
                        pROC(LED, nData[2]);
                    }
                }
                do
                {
                    Thread.Sleep(100);
                    rc += Encoding.ASCII.GetString(data, 0, stream.Read(data, 0, data.Length));
                }
                while (stream.DataAvailable);
                {
                    rc.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(s => intefaceITEthernetData.Enqueue(s));
                }
                
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        public void CloseStream()
        {
            int.TryParse(this.InterfaceITEthernetInfo.LEDStart, out int iLEDStart);
            int.TryParse(this.InterfaceITEthernetInfo.LEDStop, out int iLEDStop);
            rc = null;
            for (int i = iLEDStart; i <= iLEDStop; i++)
            {
                SendintefaceITEthernetLED(i, 0);
                Thread.Sleep(10);
            }
            stream?.Close();
            Client?.Dispose();
        }

        public void SendintefaceITEthernetLED(int nLED, int bOn )
        {
            stream?.Write(data = Encoding.ASCII.GetBytes("B1:LED:" + nLED + ":" + bOn + "\r\n"), 0, data.Length);
        }

        public void LEDon()
        {
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:1:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:2:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:3:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:4:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:5:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:6:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:7:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:8:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:9:1\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:10:1\r\n"), 0, data.Length);
        }

        public void LEDoff()
        {
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:1:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:2:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:3:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:4:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:5:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:6:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:7:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:8:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:9:0\r\n"), 0, data.Length);
            stream.Write(data = Encoding.ASCII.GetBytes("B1:LED:10:0\r\n"), 0, data.Length);
        }

        public ObservableCollection<string> InterfaceITEthernetInfoText { get; set; }

        private void GetInterfaceITEthernetInfos()
        {
            this.InterfaceITEthernetInfoText = new()
            {
                "Board ID: " + this.InterfaceITEthernetInfo.ID,
                "Name: " + this.InterfaceITEthernetInfo.NAME,
                "Serial: " + this.InterfaceITEthernetInfo.SERIAL,
                "Description: " + this.InterfaceITEthernetInfo.DESC,
                "Version: " + this.InterfaceITEthernetInfo.VERSION,
                "Firmware: " + this.InterfaceITEthernetInfo.FIRMWARE,
                "Location: " + this.InterfaceITEthernetInfo.LOCATION,
                "Usage: " + this.InterfaceITEthernetInfo.USAGE,
                "Hostname: " + this.InterfaceITEthernetInfo.HOSTNAME,
                "Client: " + this.InterfaceITEthernetInfo.CLIENT,
                "Board " + this.InterfaceITEthernetInfo.ID + " has the flollowing features:",
            };
            if (this.InterfaceITEthernetInfo.LEDTotal is not null)
            {
                this.InterfaceITEthernetInfoText.Add(this.InterfaceITEthernetInfo.LEDTotal + " | LEDs ( " + this.InterfaceITEthernetInfo.LEDStart + " - " + this.InterfaceITEthernetInfo.LEDStop + " )");
            }
            else if (this.InterfaceITEthernetInfo.LEDTotal is null)
            {
                this.InterfaceITEthernetInfoText.Add(null);
            }
            if (this.InterfaceITEthernetInfo.SWITCHTotal is not null)
            {
                this.InterfaceITEthernetInfoText.Add(this.InterfaceITEthernetInfo.SWITCHTotal + " | Switches ( " + this.InterfaceITEthernetInfo.SWITCHStart + " - " + this.InterfaceITEthernetInfo.SWITCHStop + " )");
            }
            else if (this.InterfaceITEthernetInfo.SWITCHTotal is null)
            {
                this.InterfaceITEthernetInfoText.Add(null);
            }
            if (this.InterfaceITEthernetInfo.SEVENSEGMENTTotal is not null)
            {
                this.InterfaceITEthernetInfoText.Add(this.InterfaceITEthernetInfo.SEVENSEGMENTTotal + " | 7 Segments ( " + this.InterfaceITEthernetInfo.SEVENSEGMENTStart + " - " + this.InterfaceITEthernetInfo.SEVENSEGMENTStop + " )");
            }
            else if (this.InterfaceITEthernetInfo.SEVENSEGMENTTotal is null)
            {
                this.InterfaceITEthernetInfoText.Add(null);
            }
            if (this.InterfaceITEthernetInfo.DATALINETotal is not null)
            {
                this.InterfaceITEthernetInfoText.Add(this.InterfaceITEthernetInfo.DATALINETotal + " | Datalines ( " + this.InterfaceITEthernetInfo.DATALINEStart + " - " + this.InterfaceITEthernetInfo.DATALINEStop + " )");
            }
            else if (this.InterfaceITEthernetInfo.DATALINETotal is null)
            {
                this.InterfaceITEthernetInfoText.Add(null);
            }
            if (this.InterfaceITEthernetInfo.ENCODERTotal is not null)
            {
                this.InterfaceITEthernetInfoText.Add(this.InterfaceITEthernetInfo.ENCODERTotal + " | Encoders ( " + this.InterfaceITEthernetInfo.ENCODERStart + " - " + this.InterfaceITEthernetInfo.ENCODERStop + " )");
            }
            else if (this.InterfaceITEthernetInfo.ENCODERTotal is null)
            {
                this.InterfaceITEthernetInfoText.Add(null);
            }
        }
    }

    public class InterfaceITEthernetInfo
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string SERIAL { get; set; }
        public string DESC { get; set; }
        public string COPYRIGHT { get; set; }
        public string VERSION { get; set; }
        public string FIRMWARE { get; set; }
        public string LOCATION { get; set; }
        public string USAGE { get; set; }
        public string HOSTNAME { get; set; }
        public string CLIENT { get; set; }
        public string BOARD { get; set; }

        public string LEDStart { get; set; }
        public string LEDStop { get; set; }
        public string LEDTotal { get; set; }

        public string SWITCHStart { get; set; }
        public string SWITCHStop { get; set; }
        public string SWITCHTotal { get; set; }

        public string SEVENSEGMENTStart { get; set; }
        public string SEVENSEGMENTStop { get; set; }
        public string SEVENSEGMENTTotal { get; set; }

        public string DATALINEStart { get; set; }
        public string DATALINEStop { get; set; }
        public string DATALINETotal { get; set; }

        public string ENCODERStart { get; set; }
        public string ENCODERStop { get; set; }
        public string ENCODERTotal { get; set; }
    }
}