using System;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Device_Interface_Manager.Devices.interfaceIT.ENET;

namespace Device_Interface_Manager.ViewModels;

public partial class TestENETViewModel : ObservableObject, IRecipient<string>
{
    public bool[] States { get; set; } = new bool[74];

    [ObservableProperty]
    private int _counter;

    [ObservableProperty]
    private bool[] _isChecked = new bool[10];

    [ObservableProperty]
    private bool _airbusBoeingIsChecked;

    [ObservableProperty]
    private bool _isConnected;

    public InterfaceITEthernet.ConnectionStatus ConnectionStatus;

    public TestENETViewModel() 
    {
        WeakReferenceMessenger.Default.Register(this);
        WeakReferenceMessenger.Default.Register<TestENETViewModel, StatusRequestMessage>(this, (r, m) =>
        {
            m.Reply(ConnectionStatus);
        });
    }

    [RelayCommand]
    private void ResetLabels()
    {
        Counter = 0;
        States = new bool[74];
        IsChecked = new bool[10];
        interfaceITEthernet?.SendinterfaceITEthernetLEDAllOff();
        OnPropertyChanged(nameof(States));
    }

    [RelayCommand]
    private void ButtonLED(Tuple<bool, int> tuple)
    {
        interfaceITEthernet?.SendinterfaceITEthernetLED(tuple.Item2, tuple.Item1);
    }

    [RelayCommand]
    private void SwitchAirbusBoeing()
    {
        ResetLabels();
        AirbusBoeingIsChecked = !AirbusBoeingIsChecked;
    }

    InterfaceITEthernet interfaceITEthernet;
    CancellationTokenSource ethernetCancellationTokenSource;

    private void KeyPressedAction(int key, uint direction)
    {
        if (direction == 1)
        {
            Counter++;
            States[key - 1] = true;
            OnPropertyChanged(nameof(States));
        }
    }

    public async void Receive(string message)
    {
        if (message == string.Empty)
        {
            if (interfaceITEthernet is null) 
            {
                return;
            }
            WeakReferenceMessenger.Default.Send<InterfaceITEthernetInfo>(new());
            ethernetCancellationTokenSource?.Cancel();
            interfaceITEthernet?.CloseStream();
            interfaceITEthernet = null;
            IsConnected = false;
            return;
        }

        interfaceITEthernet = new(message);
        if ((ConnectionStatus = await interfaceITEthernet.InterfaceITEthernetConnectionAsync((ethernetCancellationTokenSource = new()).Token)) == InterfaceITEthernet.ConnectionStatus.Connected)
        {
            WeakReferenceMessenger.Default.Send(await interfaceITEthernet.GetInterfaceITEthernetDataAsync(KeyPressedAction, ethernetCancellationTokenSource.Token));
            switch (interfaceITEthernet.InterfaceITEthernetInfo.ID)
            {
                case "0E08":
                    AirbusBoeingIsChecked = true;
                    IsConnected = true;
                    break;
                case "0E09":
                    AirbusBoeingIsChecked = false;
                    IsConnected = true;
                    break;
            }
        }
    }

    public sealed class StatusRequestMessage : AsyncRequestMessage<InterfaceITEthernet.ConnectionStatus>
    {

    }
}