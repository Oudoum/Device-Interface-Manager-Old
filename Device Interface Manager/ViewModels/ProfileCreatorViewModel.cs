using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.Core;
using Device_Interface_Manager.Devices.interfaceIT.ENET;
using Device_Interface_Manager.Devices.interfaceIT.USB;
using Device_Interface_Manager.Models;
using Device_Interface_Manager.SimConnectProfiles;
using GongSolutions.Wpf.DragDrop;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Device_Interface_Manager.ViewModels;
public partial class ProfileCreatorViewModel : ObservableObject, IDropTarget, ICloseWindowsCheck
{
    private readonly IDialogCoordinator dialogCoordinator;

    public Action Close { get; set; }

    public MessageDialogResult CanClose()
    {
        MessageDialogResult dialogResult = dialogCoordinator.ShowModalMessageExternal(this, "Close", "Are you sure you want to close the ProfileCreator? All unsaved changes will be lost!",
           MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
           {
               AnimateHide = false,
               AnimateShow = false,
               AffirmativeButtonText = "Yes",
               NegativeButtonText = "No"
           });
        if (dialogResult == MessageDialogResult.Affirmative)
        {
            if (IsStarted)
            {
                _ = StartProfilesAsync();
            }
            foreach (var item in deviceList)
            {
                InterfaceITAPI_Data.InterfaceITDisable(item);
            }
            deviceList.CollectionChanged -= DeviceList_CollectionChanged;
        }
        return dialogResult;
    }

    public ProfileCreatorViewModel(IDialogCoordinator dialogCoordinator, ObservableCollection<InterfaceIT_BoardInfo.Device> devices)
    {
        deviceList = devices;
        deviceList.CollectionChanged += DeviceList_CollectionChanged;
        this.dialogCoordinator = dialogCoordinator;
    }

    [ObservableProperty]
    private ProfileCreatorModel _profileCreatorModel;

    public List<ProfileCreatorModel> ProfileCreatorModels { get; set; } = new();

    public string PreviousProfileName { get; private set; }

    public string ProfileName
    {
        get => ProfileCreatorModel?.ProfileName;
        set
        {
            PreviousProfileName = ProfileCreatorModel.ProfileName;
            ProfileCreatorModel.ProfileName = value;
            OnPropertyChanged(nameof(ProfileName));
            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        SaveProfileCommand.NotifyCanExecuteChanged();
        SaveProfileAsCommand.NotifyCanExecuteChanged();
        SortInputOutputCommand.NotifyCanExecuteChanged();
        ClearProfileCommand.NotifyCanExecuteChanged();
    }

    public string Driver
    {
        get => ProfileCreatorModel?.Driver;
        set
        {
            if (ProfileCreatorModel?.Driver != value)
            {
                ProfileCreatorModel = new()
                {
                    Driver = value
                };
                ProfileCreatorModels.Clear();
                DeviceCollection.Clear();
                Device = new();

                SetupDriver(value);

                OnPropertyChanged(nameof(Driver));
                UpdateButtons();
            }
        }
    }

    [ObservableProperty]
    private string _portName = "COM3";


    private void SetupDriver(string driver)
    {
        switch (driver)
        {
            case ProfileCreatorModel.FDSUSB:
                foreach (var iitdevice in deviceList)
                {
                    CreateDevice(iitdevice.BoardName, iitdevice.SerialNumber);
                }
                break;

            case ProfileCreatorModel.FDSENET:
                //Async RelayCommand
                break;

                case ProfileCreatorModel.Arduino:
                CreateDevice("Arduino Mega 2560", PortName);
                break;

            case ProfileCreatorModel.CPflightUSB:
                CreateDevice(Devices.CPflight.Device.MCP.DeviceName, PortName);
                break;
        }
    }

    [RelayCommand]
    private async Task<InterfaceITEthernetInfo> GetInterfaceITEthernetBoardInfoAsync()
    {
        InterfaceITEthernetDiscovery? discovery = await InterfaceITEthernet.ReceiveControllerDiscoveryDataAsync();
        InterfaceITEthernetInfo interfaceITEthernetInfo = null;
        if (discovery is not null)
        {
            CreateDevice(discovery.Value.Name, discovery.Value.IPAddress);
            InterfaceITEthernet interfaceITEthernet = new(discovery.Value.IPAddress);
            CancellationTokenSource tokenSource = new();
            InterfaceITEthernet.ConnectionStatus connectionStatus = await interfaceITEthernet.InterfaceITEthernetConnectionAsync(tokenSource.Token);
            if (connectionStatus == InterfaceITEthernet.ConnectionStatus.Connected)
            {
                interfaceITEthernetInfo = await interfaceITEthernet.GetInterfaceITEthernetDataAsync((ledNumber, direction) => { }, tokenSource.Token);
                fullDevice = interfaceITEthernet;
                ErrorText = $"{Driver} => {discovery.Value.Name} was found and has been added to the devices!";
                return interfaceITEthernetInfo;
            }
        }
        ErrorText = $"{Driver} => no devices were found!";
        return interfaceITEthernetInfo;
    }

    private void CreateDevice(string name, string serial)
    {
        KeyValuePair<string, string>  device = new(name, serial);
        if (!DeviceCollection.Contains(device))
        {
            DeviceCollection.Add(device);
        }
    }

    public static string[] Drivers => ProfileCreatorModel.Drivers;


    private KeyValuePair<string, string> _device;
    public KeyValuePair<string, string> Device
    {
        get => _device;
        set
        {
            if (value.Key is null)
            {
                _device = value;
                OnPropertyChanged(nameof(Device));
                return;
            }

            if (string.IsNullOrEmpty(ProfileCreatorModel.DeviceName) || result.Value && !ProfileCreatorModels.Any(s => s.DeviceName == Device.Key))
            {
                ProfileCreatorModels.Add(ProfileCreatorModel);
                SetDevice(value, ProfileCreatorModel);
            }
            else if (!ProfileCreatorModels.Any(s => s.DeviceName == value.Key))
            {
                ProfileCreatorModel profileCreatorModel = new()
                {
                    ProfileName = ProfileCreatorModel.ProfileName,
                    Driver = ProfileCreatorModel.Driver,
                };
                SetDevice(value, profileCreatorModel);
                ProfileCreatorModels.Add(profileCreatorModel);
            }
            else if (result.Value && ProfileCreatorModels.Any(s => s.DeviceName == value.Key))
            {
                if (OverrideDevice() == MessageDialogResult.Affirmative)
                {
                    int index = ProfileCreatorModels.FindIndex(s => s.DeviceName == value.Key);
                    if (index != -1)
                    {
                        ProfileCreatorModels[index] = ProfileCreatorModel;
                    }
                }
            }
            ProfileCreatorModel = ProfileCreatorModels.Where(s => s.DeviceName == value.Key).FirstOrDefault();
            ProfileCreatorModel.DeviceName = value.Key;
            ProfileName = ProfileCreatorModel.ProfileName;
            Driver = ProfileCreatorModel.Driver;
            _device = value;
            OnPropertyChanged(nameof(Device));
        }
    }

    private MessageDialogResult OverrideDevice()
    {
        return dialogCoordinator.ShowModalMessageExternal(this, "Override", $"Are you sure you want to override the profile for {ProfileCreatorModel.DeviceName}?",
        MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
        {
            AnimateHide = false,
            AnimateShow = false,
            AffirmativeButtonText = "Yes",
            NegativeButtonText = "No"
        });
    }

    private void SetDevice(KeyValuePair<string,string> device, ProfileCreatorModel profileCreatorModel)
    {
        switch (Driver)
        {
            case ProfileCreatorModel.FDSUSB:
                fullDevice = deviceList.Where(s => s.SerialNumber == device.Value).FirstOrDefault();
                break;

            case ProfileCreatorModel.FDSENET:
                break;

            case ProfileCreatorModel.Arduino:
                fullDevice = ProfileCreatorArduino.IOStartStop;
                break;

            case ProfileCreatorModel.CPflightUSB:
                if (device.Key == Devices.CPflight.Device.MCP.DeviceName)
                {
                    fullDevice = Devices.CPflight.Device.MCP;
                }
                break;
        }
        profileCreatorModel.DeviceName = device.Key;
    }

    public ObservableCollection<KeyValuePair<string, string>> DeviceCollection { get; set; } = new();

    private readonly ObservableCollection<InterfaceIT_BoardInfo.Device> deviceList;

    private void DeviceList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Driver = null;
    }

    public object fullDevice;

    [ObservableProperty]
    private string _errorText;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveProfileCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveProfileAsCommand))]
    [NotifyCanExecuteChangedFor(nameof(SortInputOutputCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearProfileCommand))]
    private string _profileNameButtonContent = "Ok";

    private string OldFilePath => Path.Combine("Profiles", "Creator", PreviousProfileName + ".json");
    private string NewFilePath => Path.Combine("Profiles", "Creator", ProfileName + ".json");

    bool? result = false;

    [RelayCommand]
    private async Task LoadProfileAsync()
    {
        Microsoft.Win32.OpenFileDialog dialog = new()
        {
            InitialDirectory = Path.Combine(Environment.CurrentDirectory, Path.GetDirectoryName(NewFilePath)),
            Filter = "Json files (*.json)|*.json",
            DefaultExt = ".json",
        };
        result = dialog.ShowDialog();

        if (result == true)
        {
            try
            {
                using (FileStream stream = File.OpenRead(dialog.FileName))
                {
                    ProfileCreatorModel profileCreatorModel = await JsonSerializer.DeserializeAsync<ProfileCreatorModel>(stream);
                    if (deviceList.Any(s => s.BoardName == profileCreatorModel.DeviceName) ||
                        fullDevice is InterfaceITEthernet iitENET && iitENET.InterfaceITEthernetInfo.Name == profileCreatorModel.DeviceName ||
                        profileCreatorModel.Driver == ProfileCreatorModel.Arduino ||
                        profileCreatorModel.Driver == ProfileCreatorModel.CPflightUSB
                        )
                    {
                        Driver = profileCreatorModel.Driver;
                        ProfileCreatorModel = profileCreatorModel;
                        PreviousProfileName = ProfileName;
                        foreach (var item in DeviceCollection)
                        {
                            if (item.Key == profileCreatorModel.DeviceName)
                            {
                                Device = item;
                                break;
                            }
                        }
                        ProfileNameButtonContent = "Edit";
                        ErrorText = ProfileName + " successfully loaded";
                        result = false;
                        return;
                    }
                }
                ErrorText = ProfileName + "could not be loaded, because no controller for this profile was found";
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
        }
        result = false;
    }

    [RelayCommand]
    private void ProfileNameSave()
    {
        if (ProfileNameButtonContent == "Ok" && !string.IsNullOrEmpty(ProfileName))
        {
            if (!string.IsNullOrEmpty(PreviousProfileName))
            {
                try
                {
                    File.Move(OldFilePath, NewFilePath);
                    string text = File.ReadAllText(NewFilePath);
                    text = text.Replace(PreviousProfileName, ProfileName);
                    File.WriteAllText(NewFilePath, text);
                    ErrorText = PreviousProfileName + " successfully renamed to " + ProfileName;
                }
                catch (Exception e)
                {
                    ErrorText = e.Message;
                }
            }
            ProfileNameButtonContent = "Edit";
        }
        else if (ProfileNameButtonContent == "Edit")
        {
            ProfileNameButtonContent = "Ok";
        }
    }

    [RelayCommand]
    private void SaveProfile()
    {
        try
        {
            _ = Directory.CreateDirectory(Path.GetDirectoryName(NewFilePath));
            File.WriteAllText(NewFilePath, JsonSerializer.Serialize(ProfileCreatorModel, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull }));
            ErrorText = ProfileName + " successfully saved";
        }
        catch (Exception e)
        {
            ErrorText = e.Message;
        }
    }

    [RelayCommand]
    private async Task SaveProfileAsAsync()
    {
        string inputDialogResult = await dialogCoordinator.ShowInputAsync(this, "Profile name", "Please enter your profile name.",
            new MetroDialogSettings
            {
                AnimateHide = false,
                AnimateShow = false,
                DefaultText = PreviousProfileName
            });
        if (!string.IsNullOrEmpty(inputDialogResult))
        {
            ProfileName = inputDialogResult;
            SaveProfile();
            ProfileNameButtonContent = "Edit";
        }
    }

    [ObservableProperty]
    private bool? _isSortedAscending;

    [RelayCommand]
    private void SortInputOutput()
    {
        if (ProfileCreatorModel is not null)
        {
            IsSortedAscending ??= false;
            List<InputCreator> sortedInputList = ProfileCreatorModel.InputCreator.ToList();
            sortedInputList.Sort((x, y) => IsSortedAscending.Value ? Comparer<KeyValuePair<string, string>?>.Default.Compare(y.Input, x.Input) : Comparer<KeyValuePair<string, string>?>.Default.Compare(x.Input, y.Input));
            for (int i = 0; i < sortedInputList.Count; i++)
            {
                ProfileCreatorModel.InputCreator.Move(ProfileCreatorModel.InputCreator.IndexOf(sortedInputList[i]), i);
            }

            List<OutputCreator> sortedOutputList = ProfileCreatorModel.OutputCreator.ToList();
            sortedOutputList.Sort((x, y) => IsSortedAscending.Value ? Comparer<KeyValuePair<string, string>?>.Default.Compare(y.Output, x.Output) : Comparer<KeyValuePair<string, string>?>.Default.Compare(x.Output, y.Output));
            for (int i = 0; i < sortedOutputList.Count; i++)
            {
                ProfileCreatorModel.OutputCreator.Move(ProfileCreatorModel.OutputCreator.IndexOf(sortedOutputList[i]), i);
            }
            IsSortedAscending = !IsSortedAscending;
            ErrorText = ProfileName + " successfully sorted";
        }
    }

    [RelayCommand]
    private async Task ClearProfileAsync()
    {
        if (ProfileCreatorModel is not null && MessageDialogResult.Affirmative == await dialogCoordinator
            .ShowMessageAsync(this, "Clear", "Are you sure you want to clear all inputs and outputs?",
            MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
            {
                AnimateHide = false,
                AnimateShow = false,
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            }))
        {
            ProfileCreatorModel.InputCreator.Clear();
            ProfileCreatorModel.OutputCreator.Clear();
        }
    }

    [RelayCommand]
    private void AddInput()
    {
        ProfileCreatorModel?.InputCreator.Add(new InputCreator() { Id = Guid.NewGuid(), IsActive = true });
    }

    [RelayCommand]
    private void AddOutput()
    {
        ProfileCreatorModel?.OutputCreator.Add(new OutputCreator() { Id = Guid.NewGuid(), IsActive = true });
    }

    private async Task<bool> ShowConfirmationDialogAsync(string rowType)
    {
        MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Delete", $"Are you sure you want to delete the selected {rowType}?",
            MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
            {
                AnimateHide = false,
                AnimateShow = false,
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            });
        return result == MessageDialogResult.Affirmative;
    }

    [RelayCommand]
    private async Task DeleteInputOutputCreatorRowAsync(object creator)
    {
        if (creator is InputCreator inputCreator)
        {
            if (!await ShowConfirmationDialogAsync("input row"))
            {
                return;
            }
            _ = ProfileCreatorModel.InputCreator.Remove(inputCreator);
        }
        else if (creator is OutputCreator outputCreator)
        {
            if (!await ShowConfirmationDialogAsync("output row"))
            {
                return;
            }
            _ = ProfileCreatorModel.OutputCreator.Remove(outputCreator);
        }
        else if (creator is IList selectedItems)
        {
            bool isShown = false;
            foreach (var item in selectedItems.Cast<object>().ToList())
            {
                if (item is InputCreator input)
                {
                    if (!isShown)
                    {
                        if (!await ShowConfirmationDialogAsync("input rows"))
                        {
                            return;
                        }
                    }
                    isShown = true;
                    _ = ProfileCreatorModel.InputCreator.Remove(input);
                }
                else if (item is OutputCreator output)
                {
                    if (!isShown)
                    {
                        if (!await ShowConfirmationDialogAsync("output rows"))
                        {
                            return;
                        }
                    }
                    isShown = true;
                    _ = ProfileCreatorModel.OutputCreator.Remove(output);
                }
            }
        }
    }

    [RelayCommand]
    private void DuplicateInputOutputCreatorRow(object creator)
    {
        ActivateDeactivateAllOrCloneInputOutputCreator(null, creator);
    }

    [RelayCommand]
    private void ActivateAllInputOutputCreator(object creator)
    {
        ActivateDeactivateAllOrCloneInputOutputCreator(true, creator);
    }

    [RelayCommand]
    private void DeactivateAllInputOutputCreator(object creator)
    {
        ActivateDeactivateAllOrCloneInputOutputCreator(false, creator);
    }

    private void ActivateDeactivateAllOrCloneInputOutputCreator(bool? isActive, object creator)
    {
        if (creator is IList selectedItems)
        {
            foreach (var item in selectedItems.Cast<object>().ToList())
            {
                if (item is InputCreator input)
                {
                    if (isActive is null)
                    {
                        ProfileCreatorModel.InputCreator.Add(input.Clone());
                        continue;
                    }
                    input.IsActive = isActive.Value;
                }
                else if (item is OutputCreator output)
                {
                    if (isActive is null)
                    {
                        ProfileCreatorModel.OutputCreator.Add(output.Clone());
                        continue;
                    }
                    output.IsActive = isActive.Value;
                }
            }
        }
    }

    public string[] EventDataTypePreSelections { get; set; } = { ProfileCreatorModel.MSFSSimConnect, ProfileCreatorModel.PMDG737 };

    public string EventDataTypePreSelection { get; set; } = ProfileCreatorModel.PMDG737;

    [RelayCommand]
    private void EditInputOutput(object inputOutputCreator)
    {
        NavigationService navigationService = new();
        if (inputOutputCreator is InputCreator inputCreator)
        {
            inputCreator.InputType ??= ProfileCreatorModel.SWITCH;
            inputCreator.EventType ??= EventDataTypePreSelection;

            navigationService.NavigateToInputCreator(
                inputCreator,
                ProfileCreatorModel.OutputCreator,
                fullDevice);
        }
        else if (inputOutputCreator is OutputCreator outputCreator)
        {
            outputCreator.OutputType ??= ProfileCreatorModel.LED;
            outputCreator.DataType ??= EventDataTypePreSelection;

            navigationService.NavigateToOutputCreator(
                outputCreator,
                ProfileCreatorModel.OutputCreator,
                fullDevice);
        }
    }

    [ObservableProperty]
    private bool _isStarted;

    [RelayCommand]
    private async Task StartProfilesAsync()
    {
        switch (Driver)
        {
            case ProfileCreatorModel.FDSUSB when !IsStarted:
                await Profiles.Instance.StartAsync(ProfileCreatorModel, deviceList.FirstOrDefault(k => k.SerialNumber == Device.Value));
                break;

            case ProfileCreatorModel.Arduino when !IsStarted:
                await Profiles.Instance.StartAsync(ProfileCreatorModel, PortName);
                break;

            default:
                Profiles.Instance.Stop();
                break;
        }

        IsStarted = !IsStarted;
        if (!IsStarted)
        { 
            ErrorText = ProfileCreatorModel.ProfileName + " stopped";
            return;
        }
        ErrorText = ProfileCreatorModel.ProfileName + " started";
    }

    public void DragOver(IDropInfo dropInfo)
    {
        if ((dropInfo.Data is InputCreator && dropInfo.TargetItem is InputCreator) || (dropInfo.Data is OutputCreator && dropInfo.TargetItem is OutputCreator))
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is InputCreator)
        {
            InputCreator item = (InputCreator)dropInfo.Data;
            _ = ProfileCreatorModel.InputCreator.Remove(item);
            if (dropInfo.InsertIndex >= 0 && dropInfo.InsertIndex <= ProfileCreatorModel.InputCreator.Count)
            {
                ProfileCreatorModel.InputCreator.Insert(dropInfo.InsertIndex, item);
                return;
            }
            ProfileCreatorModel.InputCreator.Add(item);
        }
        else if (dropInfo.Data is OutputCreator item)
        {
            _ = ProfileCreatorModel.OutputCreator.Remove(item);
            if (dropInfo.InsertIndex >= 0 && dropInfo.InsertIndex <= ProfileCreatorModel.OutputCreator.Count)
            {
                ProfileCreatorModel.OutputCreator.Insert(dropInfo.InsertIndex, item);
                return;
            }
            ProfileCreatorModel.OutputCreator.Add(item);
        }
    }
}