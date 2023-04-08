using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.interfaceIT.USB;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class SwitchTestViewModel : ObservableObject, ISwitchLogChanged
{
    public ObservableCollection<string> SwitchLog { get; set; } = new();

    private InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC keyNotifiyCallback;

    [ObservableProperty]
    private bool _pollModeEnabled;

    [ObservableProperty]
    private bool _callbackModeEnabled;

    public required int Session { get; init; }

    public Action SwitchLogChanged { get; set; }

    public SwitchTestViewModel() 
    {
        SwitchLog.CollectionChanged += SwitchLog_CollectionChanged;
    }

    private void SwitchLog_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            SwitchLogChanged?.Invoke();
        }
    }

    [RelayCommand]
    private void CallbackMode()
    {
        CallbackModeEnabled = !CallbackModeEnabled;
        if (keyNotifiyCallback is null)
        {
            _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(Session, true, keyNotifiyCallback = new InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC(KeyPressedProc));
            return;
        }
        _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(Session, false, keyNotifiyCallback = null);
    }

    [RelayCommand]
    private void PollMode()
    {
        _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(Session, PollModeEnabled = !PollModeEnabled);
    }

    [RelayCommand]
    private void GetSwitch()
    {
        while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(Session, out int _, out int _) == 0)
        {
            _ = InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(Session, out int key, out int direction);
            KeyPressedProc(Session, key, direction);
        }
    }

    [RelayCommand]
    private void ClearSwitchList()
    {
        SwitchLog.Clear();
    }

    private bool KeyPressedProc(int session, int key, int direction)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            SwitchLog.Add("Session " + session + ", Switch " + key + " is now " + (direction == InterfaceITAPI_Data.Data.INTERFACEIT_SWITCH_DIR_DOWN ? "on" : "off"));
        });
        return true;
    }
}