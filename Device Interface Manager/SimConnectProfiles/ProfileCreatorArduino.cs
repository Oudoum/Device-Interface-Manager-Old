using Device_Interface_Manager.Devices.interfaceIT.ENET;
using Device_Interface_Manager.Models;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Device_Interface_Manager.SimConnectProfiles;
public class ProfileCreatorArduino : ProfileCreatorBase<string>
{
    public static uint IOStart => 2;

    public static uint IOStop => 53;

    public static Tuple<uint, uint> IOStartStop => new(IOStart, IOStop);

    public int BaudRate { get; set; } = 9600;

    public Parity Parity { get; set; } = Parity.None;

    public int DataBits { get; set; } = 8;

    public StopBits StopBits { get; set; } = StopBits.One;

    SerialPort serialPort;

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (serialPort.IsOpen)
        {
            string[] data = serialPort.ReadLine().Split(':');
            string type = data[0];
            if (data.Length == 1)
            {

            }
            else if (data.Length == 3)
            {
                string pin = data[1];
                string value = data[2];
                switch (type)
                {
                    case "SW":
                        ButtonIteration(pin, Convert.ToUInt32(value));
                        break;
                }
            }
        }
    }

    private void ArduinoStop()
    {
        serialPort.DataReceived -= SerialPort_DataReceived;
        serialPort.Close();
    }

    protected override string Device { get; set; }

    protected override void CockpitLoaded(bool isLoaded)
    {

    }

    protected override void SetBooleanOuput(OutputCreator item, bool valueBool)
    {

    }

    protected override void SetDisplayOutput(OutputCreator item, string outputValue)
    {

    }

    protected override Task<InterfaceITEthernet.ConnectionStatus> StartDevice()
    {
        serialPort = new(Device, BaudRate, Parity, DataBits, StopBits)
        {
            NewLine = "\r\n"
        };
        serialPort.DataReceived += SerialPort_DataReceived;
        try
        {
            serialPort.Open();
            serialPort.WriteLine("START");
            return Task.FromResult(InterfaceITEthernet.ConnectionStatus.Connected);
        }
        catch (Exception)
        {
            ArduinoStop();
            return Task.FromResult(InterfaceITEthernet.ConnectionStatus.NotConnected);
        }
    }

    protected override void StopDevice()
    {
        ArduinoStop();
    }
}