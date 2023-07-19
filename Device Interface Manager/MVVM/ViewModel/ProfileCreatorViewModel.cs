using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.MSFSProfiles;
using Device_Interface_Manager.interfaceIT.USB;
using MahApps.Metro.Controls.Dialogs;
using GongSolutions.Wpf.DragDrop;
using Device_Interface_Manager.Core;

namespace Device_Interface_Manager.MVVM.ViewModel;
public partial class ProfileCreatorViewModel : ObservableObject, IDropTarget, ICloseWindowsCheck
{
    private readonly IDialogCoordinator dialogCoordinator;

    public Action Close { get; set; }

    public MessageDialogResult CanCloseAsync()
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
            _ = StartProfiles();
            foreach (var item in Devices)
            {
                InterfaceITAPI_Data.InterfaceITDisable(item);
            }
        }
        return dialogResult;
    }

    public ProfileCreatorViewModel(IDialogCoordinator dialogCoordinator)
    {
        this.dialogCoordinator = dialogCoordinator;
    }

    [ObservableProperty]
    private ProfileCreatorModel _profileCreatorModel = new();

    public List<ProfileCreatorModel> ProfileCreatorModels { get; set; } = new();

    public string PreviousProfileName { get; private set; }

    public string ProfileName
    {
        get => ProfileCreatorModel.ProfileName;
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
        get => ProfileCreatorModel.Driver;
        set
        {
            ProfileCreatorModel.Driver = value;
            switch (value)
            {
                case ProfileCreatorModel.FDSUSB:
                    foreach (var device in Devices)
                    {
                        if (!DeviceNameCollection.Contains(device.BoardType))
                        {
                            DeviceNameCollection.Add(device.BoardType);
                        }
                    }
                    break;
            }
            OnPropertyChanged(nameof(Driver));
            UpdateButtons();
        }
    }

    public static string[] Drivers => ProfileCreatorModel.Drivers;

    public string DeviceName
    {
        get => ProfileCreatorModel.DeviceName;
        set
        {
            if (string.IsNullOrEmpty(ProfileCreatorModel.DeviceName) || result.Value && !ProfileCreatorModels.Any(s => s.DeviceName == value))
            {
                ProfileCreatorModels.Add(ProfileCreatorModel);
                SetDevice(value, ProfileCreatorModel);
            }
            else if (!ProfileCreatorModels.Any(s => s.DeviceName == value))
            {
                ProfileCreatorModel profileCreatorModel = new()
                {
                    ProfileName = ProfileCreatorModel.ProfileName,
                    Driver = ProfileCreatorModel.Driver,
                };
                SetDevice(value, profileCreatorModel);
                ProfileCreatorModels.Add(profileCreatorModel);
            }
            else if (result.Value && ProfileCreatorModels.Any(s => s.DeviceName == value))
            {
                if (OverrideDevice() == MessageDialogResult.Affirmative)
                {
                    int index = ProfileCreatorModels.FindIndex(x => x.DeviceName == value);
                    if (index != -1)
                    {
                        ProfileCreatorModels[index] = ProfileCreatorModel;
                    }
                }
            }
            ProfileCreatorModel = ProfileCreatorModels.Where(s => s.DeviceName == value).FirstOrDefault();
            ProfileCreatorModel.DeviceName = value;
            ProfileName = ProfileCreatorModel.ProfileName;
            Driver = ProfileCreatorModel.Driver;
            OnPropertyChanged(nameof(DeviceName));
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

    private void SetDevice(string deviceName, ProfileCreatorModel profileCreatorModel)
    {
        switch (Driver)
        {
            case ProfileCreatorModel.FDSUSB:
                InterfaceIT_BoardInfo.Device device = Devices.Where(s => s.BoardType == deviceName).FirstOrDefault();
                for (int i = device.DeviceInfo.nSwitchFirst; i <= device.DeviceInfo.nSwitchLast; i++)
                {
                    profileCreatorModel.Switches.Add(i);
                }
                for (int i = device.DeviceInfo.nLEDFirst; i <= device.DeviceInfo.nLEDLast; i++)
                {
                    profileCreatorModel.LEDs.Add(i);
                }
                for (int i = device.DeviceInfo.n7SegmentFirst; i <= device.DeviceInfo.n7SegmentLast; i++)
                {
                    if (device.DeviceInfo.n7SegmentCount > 0)
                    {
                        profileCreatorModel.SevenSegments.Add(i);
                    }
                }
                InterfaceITAPI_Data.interfaceIT_LED_Enable(device.Session, true);
                InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(device.Session, true);
                InterfaceITAPI_Data.interfaceIT_7Segment_Enable(device.Session, true);
                break;
        }
        profileCreatorModel.DeviceName = deviceName;
    }

    public ObservableCollection<string> DeviceNameCollection { get; set; } = new();

    public InterfaceIT_BoardInfo.Device[] Devices { get; set; }

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
                    if (Devices.Any(s => s.BoardType == profileCreatorModel.DeviceName))
                    {
                        foreach (var item in profileCreatorModel.InputCreator)
                        {
                            if (string.IsNullOrEmpty(item.InputType) && item.Input is not null)
                            {
                                item.InputType = ProfileCreatorModel.SWITCH;
                            }
                        }
                        foreach (var item in profileCreatorModel.OutputCreator)
                        {
                            if (string.IsNullOrEmpty(item.OutputType) && item.Output is not null)
                            {
                                item.OutputType = ProfileCreatorModel.LED;
                            }
                        }
                        profileCreatorModel.LEDs = ProfileCreatorModel.LEDs;
                        profileCreatorModel.Switches = ProfileCreatorModel.Switches;
                        profileCreatorModel.SevenSegments = ProfileCreatorModel.SevenSegments;
                        ProfileCreatorModel = profileCreatorModel;
                        PreviousProfileName = ProfileName;
                        DeviceName = ProfileCreatorModel.DeviceName;
                        ProfileNameButtonContent = "Edit";
                        ErrorText = ProfileName + " successfully loaded";
                        result = false;
                        return;
                    }
                }
                ErrorText = "could not be loaded, because no controller for this profile was found";
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
            Directory.CreateDirectory(Path.GetDirectoryName(NewFilePath));
            File.WriteAllText(NewFilePath, JsonSerializer.Serialize(ProfileCreatorModel, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull }));
            ErrorText = ProfileName + " successfully saved";
        }
        catch (Exception e)
        {
            ErrorText = e.Message;
        }
    }

    [RelayCommand]
    private async Task SaveProfileAs()
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
            sortedInputList.Sort((x, y) => IsSortedAscending.Value ? Comparer<int?>.Default.Compare(y.Input, x.Input) : Comparer<int?>.Default.Compare(x.Input, y.Input));
            for (int i = 0; i < sortedInputList.Count; i++)
            {
                ProfileCreatorModel.InputCreator.Move(ProfileCreatorModel.InputCreator.IndexOf(sortedInputList[i]), i);
            }

            List<OutputCreator> sortedOutputList = ProfileCreatorModel.OutputCreator.ToList();
            sortedOutputList.Sort((x, y) => IsSortedAscending.Value ? Comparer<int?>.Default.Compare(y.Output, x.Output) : Comparer<int?>.Default.Compare(x.Output, y.Output));
            for (int i = 0; i < sortedOutputList.Count; i++)
            {
                ProfileCreatorModel.OutputCreator.Move(ProfileCreatorModel.OutputCreator.IndexOf(sortedOutputList[i]), i);
            }
            IsSortedAscending = !IsSortedAscending;
            ErrorText = ProfileName + " successfully sorted";
        }
    }

    [RelayCommand]
    private async Task ClearProfile()
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
            ProfileCreatorModel.UsedSwitches.Clear();
            ProfileCreatorModel.InputCreator.Clear();

            ProfileCreatorModel.UsedLEDs.Clear();
            ProfileCreatorModel.UsedSevenSegments.Clear();
            ProfileCreatorModel.OutputCreator.Clear();
        }
    }

    [RelayCommand]
    private void AddInput()
    {
        ProfileCreatorModel?.InputCreator.Add(new InputCreator());
    }

    [RelayCommand]
    private void AddOutput()
    {
        ProfileCreatorModel?.OutputCreator.Add(new OutputCreator());
    }

    private async Task<bool> ShowConfirmationDialog(string rowType)
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
    private async Task DeleteInputOutputCreatorRow(object creator)
    {
        if (creator is InputCreator inputCreator)
        {
            if (!await ShowConfirmationDialog("input row"))
            {
                return;
            }
            DeleteInputCreatorRow(inputCreator);
        }
        else if (creator is OutputCreator outputCreator)
        {
            if (!await ShowConfirmationDialog("output row"))
            {
                return;
            }
            DeleteOutputCreatorRow(outputCreator);
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
                        if (!await ShowConfirmationDialog("input rows"))
                        {
                            return;
                        }
                    }
                    isShown = true;
                    DeleteInputCreatorRow(input);
                }
                else if (item is OutputCreator output)
                {
                    if (!isShown)
                    {
                        if (!await ShowConfirmationDialog("output rows"))
                        {
                            return;
                        }
                    }
                    isShown = true;
                    DeleteOutputCreatorRow(output);
                }
            }
        }
    }

    private void DeleteInputCreatorRow(InputCreator input)
    {
        ProfileCreatorModel.UsedSwitches.Remove(input.Input);
        ProfileCreatorModel.InputCreator.Remove(input);
    }

    private void DeleteOutputCreatorRow(OutputCreator output)
    {
        if (output.OutputType == ProfileCreatorModel.LED)
        {
            ProfileCreatorModel.UsedLEDs.Remove(output.Output);
        }
        else if (output.OutputType == ProfileCreatorModel.SEVENSEGMENT)
        {
            ProfileCreatorModel.UsedSevenSegments.Remove(output.Output);
        }
        ProfileCreatorModel.OutputCreator.Remove(output);
    }

    [RelayCommand]
    private void DuplicateInputOutputCreatorRow(object creator)
    {
        if (creator is IList selectedItems)
        {
            foreach (var item in selectedItems.Cast<object>().ToList())
            {
                if (item is InputCreator input)
                {
                    ProfileCreatorModel.InputCreator.Add(input.Clone());
                }
                else if (item is OutputCreator output)
                {
                    ProfileCreatorModel.OutputCreator.Add(output.Clone());
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
            ProfileCreatorModel.UsedSwitches.Remove(inputCreator.Input);
            inputCreator.InputType ??= ProfileCreatorModel.SWITCH;
            inputCreator.EventType ??= EventDataTypePreSelection;
            navigationService.NavigateToInputCreator(inputCreator, ProfileCreatorModel.Switches.ToArray() ,Devices.FirstOrDefault(k => k.BoardType == ProfileCreatorModel.DeviceName));
            ProfileCreatorModel.UsedSwitches.Add(inputCreator.Input);
        }
        else if (inputOutputCreator is OutputCreator outputCreator)
        {
            if (outputCreator.OutputType == ProfileCreatorModel.LED)
            {
                ProfileCreatorModel.UsedLEDs.Remove(outputCreator.Output);
            }
            else if (outputCreator.OutputType == ProfileCreatorModel.SEVENSEGMENT)
            {
                ProfileCreatorModel.UsedSevenSegments.Remove(outputCreator.Output);
            }
            outputCreator.OutputType ??= ProfileCreatorModel.LED;
            outputCreator.DataType ??= EventDataTypePreSelection;
            navigationService.NavigateToOutputCreator(outputCreator, ProfileCreatorModel.LEDs.ToArray(), ProfileCreatorModel.SevenSegments.ToArray(), Devices.FirstOrDefault(k => k.BoardType == ProfileCreatorModel.DeviceName));
            if (outputCreator.OutputType == ProfileCreatorModel.LED)
            {
                ProfileCreatorModel.UsedLEDs.Add(outputCreator.Output);
            }
            else if (outputCreator.OutputType == ProfileCreatorModel.SEVENSEGMENT)
            {
                ProfileCreatorModel.UsedSevenSegments.Add(outputCreator.Output);
            }
        }
    }

    [ObservableProperty]
    private bool _isStarted;

    [RelayCommand]
    private async Task StartProfiles()
    {
        IsStarted = !IsStarted;
        if (!IsStarted)
        {
            Profiles.Instance.Stop();
            ErrorText = "Profiles stopped";
        }
        foreach (var device in Devices)
        {
            InterfaceITAPI_Data.interfaceIT_LED_Enable(device.Session, !IsStarted);
            InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(device.Session, !IsStarted);
            InterfaceITAPI_Data.interfaceIT_7Segment_Enable(device.Session, !IsStarted);
        }
        if (IsStarted)
        {
            await Profiles.Instance.StartAsync(ProfileCreatorModels.ToArray(), Devices.ToArray());
            ErrorText = "Profiles started";
        }
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
            ProfileCreatorModel.InputCreator.Remove(item);
            if (dropInfo.InsertIndex >= 0 && dropInfo.InsertIndex <= ProfileCreatorModel.InputCreator.Count)
            {
                ProfileCreatorModel.InputCreator.Insert(dropInfo.InsertIndex, item);
                return;
            }
            ProfileCreatorModel.InputCreator.Add(item);
        }
        else if (dropInfo.Data is OutputCreator item)
        {
            ProfileCreatorModel.OutputCreator.Remove(item);
            if (dropInfo.InsertIndex >= 0 && dropInfo.InsertIndex <= ProfileCreatorModel.OutputCreator.Count)
            {
                ProfileCreatorModel.OutputCreator.Insert(dropInfo.InsertIndex, item);
                return;
            }
            ProfileCreatorModel.OutputCreator.Add(item);
        }
    }
}