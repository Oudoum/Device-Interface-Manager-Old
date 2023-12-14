using DevDecoder.HIDDevices.Converters;
using Device_Interface_Manager.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text.Json;
using System.Threading;
using System.Windows.Input;

namespace Device_Interface_Manager.Devices.COM;
public class SerialDevice
{
    public string PortName { get; set; } = "COM3";

    public int BaudRate { get; set; } = 9600;

    public Parity Parity { get; set; } = Parity.None;

    public int DataBits { get; set; } = 8;

    public StopBits StopBits { get; set; } = StopBits.One;

    SerialPort serialPort;

    public SerialDevice()
    {

        serialPort = new(PortName, BaudRate, Parity, DataBits, StopBits)
        {
            NewLine = "\r\n"
        };
        serialPort.DataReceived += SerialPort_DataReceived;
        serialPort.ErrorReceived += SerialPort_ErrorReceived;
        serialPort.PinChanged += SerialPort_PinChanged;
        serialPort.Disposed += SerialPort_Disposed;

        try
        {
            serialPort.Open();
        }
        catch (Exception)
        {
            serialPort.DataReceived -= SerialPort_DataReceived;
            serialPort.ErrorReceived -= SerialPort_ErrorReceived;
            serialPort.PinChanged -= SerialPort_PinChanged;
            serialPort.Disposed -= SerialPort_Disposed;
            serialPort.Close();
        }

        //serialPort.WriteLine("LED:13:1");
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //serialPort.WriteLine("LED:13:0");
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //serialPort.WriteLine("LED:13:1");
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //serialPort.WriteLine("LED:13:0");

        serialPort.WriteLine("START");
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //Thread.Sleep(TimeSpan.FromSeconds(3));
        //Thread.Sleep(TimeSpan.FromSeconds(3));

        //serialPort.DataReceived -= SerialPort_DataReceived;
        //serialPort.ErrorReceived -= SerialPort_ErrorReceived;
        //serialPort.PinChanged -= SerialPort_PinChanged;
        //serialPort.Disposed -= SerialPort_Disposed;
        //serialPort.Close();
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (serialPort.IsOpen)
        {
            string[] data = serialPort.ReadLine().Split(':');
            string type = data[0];
            if (data.Length == 1)
            {
                Debug.WriteLine(type);
            }
            else if (data.Length == 3)
            {
                string pin = data[1];
                string value = data[2];
                switch (type)
                {
                    case "SW":
                        break;
                }
                Debug.WriteLine(type + pin + value);
            }
        }
    }

    private void SerialPort_Disposed(object sender, EventArgs e)
    {

    }
    private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {

    }

    private void SerialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
    {

    }



}