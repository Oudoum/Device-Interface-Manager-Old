using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.Models;
using Device_Interface_Manager.Devices.interfaceIT.USB;

namespace Device_Interface_Manager.ViewModels;

public partial class SwitchTestViewModel : ObservableObject, ISwitchLogChanged
{
    public ObservableCollection<string> SwitchLog { get; set; } = new();

    private InterfaceITAPI_Data.KeyNotificationCallback keyNotifiyCallback;

    [ObservableProperty]
    private bool _pollModeEnabled;

    [ObservableProperty]
    private bool _callbackModeEnabled;

    public required uint Session { get; init; }

    public Action SwitchLogChanged { get; set; }

    public SwitchTestViewModel() 
    {
        SwitchLog.CollectionChanged += SwitchLog_CollectionChanged;
    }

    private void SwitchLog_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            SwitchLogChanged?.Invoke();
        }
    }

    [RelayCommand]
    private void CallbackMode()
    {
        if (keyNotifiyCallback is null)
        {
            InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(Session, CallbackModeEnabled = true, keyNotifiyCallback = new(KeyPressedProc));
            return;
        }
        InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(Session, CallbackModeEnabled = false, keyNotifiyCallback = null);
    }

    [RelayCommand]
    private void PollMode()
    {
        InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(Session, PollModeEnabled = !PollModeEnabled);
    }

    [RelayCommand]
    private void GetSwitch()
    {
        while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(Session, out int key, out int direction) == 0)
        {
            KeyPressedProc(Session, key, direction);
        }
    }

    [RelayCommand]
    private void ClearSwitchList()
    {
        SwitchLog.Clear();
    }

    private void KeyPressedProc(uint session, int key, int direction)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            SwitchLog.Add($"Session {session}, Switch {key} is now {(direction == (int)InterfaceITAPI_Data.SwitchDirectionInfo.INTERFACEIT_SWITCH_DIR_DOWN ? "on" : "off")}");
        });
    }
}